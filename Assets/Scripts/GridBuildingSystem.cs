using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GridCell 클레스는 각 그리드 셀의 상태와 데이터를 관리 합니다.
public class GridCell
{
    public Vector3Int Position;             //셀의 그리드 내 위치
    public bool IsOccupied;                 //셀의 건물로 차있지 여부
    public GameObject Building;             //셀의 배치된 건물 객체

    public GridCell(Vector3Int position)      //클레스 이름과 동일한 함수
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }
}

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private int width = 10;    //그리드의 가로 크기
    [SerializeField] private int height = 10;   //그리드의 세로 크기
    [SerializeField] private float cellSize = 10;       //각 셀의 크기
    [SerializeField] private GameObject cellPrefabs;    //셀 프리팹
    [SerializeField] private GameObject builingPrefabs; //빌딩 프라팸

    [SerializeField] private PlayerController playerController;  //플레이어 컨트롤러 참조

    [SerializeField] private Grid grid;
    private GridCell[,] cells;
    private Camera firstPersonCamera;

    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera; //플레이어의 카메라 객체를 가져온다
        CreateGrid();
    }

    //그리드 셀을 Glzmo 표기하는 메서드
    private void OnDrawGizmos()               //유니티 Scene창에 보이는 Debug 그림
    {
        Gizmos.color = Color.blue;
        for(int x = 0; x < width; x++)
        {
            for(int z =0; z < height; z++)
            {
                Vector3 cellCenter = grid.GetCellCenterWorld(new Vector3Int(x, 0, z));
                Gizmos.DrawWireCube(cellCenter,new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }


    //그리드를 생선하고 셀을 초기화하는 메서드
    private void CreateGrid()
    {
        grid.cellSize = new Vector3(cellSize, cellSize, cellSize);

        cells = new GridCell[width, height];
        Vector3 gridCenter = playerController.transform.position;
        gridCenter.y = 0;
        transform.position = gridCenter - new Vector3(width * cellSize / 2.0f, 0, height * cellSize / 2.0f);
        
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                Vector3Int cellPosition = new Vector3Int(x, 0, z); //셀 위치
                Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition); //그리드 함수를 통해서 월드 포지션 위치를 가져온다
                GameObject cellObject = Instantiate(cellPrefabs, worldPosition, cellPrefabs.transform.rotation);
                cellObject.transform.SetParent(transform);

                cells[x, z] = new GridCell(cellPosition);
            }
        }
    }
    //Update is called once per frame
    void Update()
    {
        Vector3 lookPosition = GetLookPosition();
        if(lookPosition != Vector3.zero)
        {
            Vector3Int gridPosition = grid.WorldToCell(lookPosition);
            if (isValidGridPosition(gridPosition))
            {
                HighlightCell(gridPosition);

                if (Input.GetMouseButton(0))
                {
                    PlaceBuilding(gridPosition);
                }
                if (Input.GetMouseButton(1))
                {
                    RemoveBuilding(gridPosition);
                }
            }
        }
    }

    //그리드 셀에 건물을  배치하는 메서드
    private void PlaceBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        if (!cell.IsOccupied)
        {
            Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
            GameObject building = Instantiate(builingPrefabs, worldPosition, Quaternion.identity);
            cell.IsOccupied = true;
            cell.Building = building;
        }
    }
    //그리드 셀에서 거물을 제거하는 메서드 
    private void RemoveBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];  //위치 기반으로 cell을 받아온다.
        if (cell.IsOccupied)                                    //해당 위치에 건물이 있는지 확인한다. 
        {
            Destroy(cell.Building);                                     //Cell 건물을 제거한다. 
            cell.IsOccupied = false;                                    //건물 확인 값
            cell.Building = null;                                       //Cell 에 놓인 빌딩 null 값으로 변경
        }
    }

    //선택된 셀을 하이라이트하는 메스
    private void HighlightCell(Vector3Int gridPosition)
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GameObject cellObject = cells[x, z].Building != null ? cells[x, z].Building : transform.GetChild(x * height + z).gameObject;
                cellObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        //특징 샐에 건물이 있으면 빨간색 아니면 초록색
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        GameObject highlightObject = cell.Building != null ? cell.Building : transform.GetChild(gridPosition.x * height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied ? Color.red : Color.green;
    }

    //그리드 포지션 유효한지 확인하는 메서드
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width &&
            gridPosition.z >= 0 && gridPosition.z < height;
    }
    //플레이어가 보고 있는 위치를 계산하는 메서드
    private Vector3 GetLookPosition()
    {
        if (playerController.isFirstPerson)
        {
            Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white);
            }
        }
        //3인칭 모드
        else
        {
            Vector3 characterPosition = playerController.transform.position;
            Vector3 characterFoward = playerController.transform.forward;
            Vector3 rayOrigin = characterPosition + Vector3.up * 1.5f + characterFoward * 0.5f;
            Vector3 rayDirection = (characterFoward - Vector3.up).normalized;

            Ray ray = new Ray(rayOrigin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.blue);
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white);
            }
        }
        return Vector3.zero;
    }
    
}
