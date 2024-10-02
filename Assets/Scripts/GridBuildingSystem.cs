using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GridCell Ŭ������ �� �׸��� ���� ���¿� �����͸� ���� �մϴ�.
public class GridCell
{
    public Vector3Int Position;             //���� �׸��� �� ��ġ
    public bool IsOccupied;                 //���� �ǹ��� ������ ����
    public GameObject Building;             //���� ��ġ�� �ǹ� ��ü

    public GridCell(Vector3Int position)      //Ŭ���� �̸��� ������ �Լ�
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }
}

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private int width = 10;    //�׸����� ���� ũ��
    [SerializeField] private int height = 10;   //�׸����� ���� ũ��
    [SerializeField] private float cellSize = 10;       //�� ���� ũ��
    [SerializeField] private GameObject cellPrefabs;    //�� ������
    [SerializeField] private GameObject builingPrefabs; //���� ������

    [SerializeField] private PlayerController playerController;  //�÷��̾� ��Ʈ�ѷ� ����

    [SerializeField] private Grid grid;
    private GridCell[,] cells;
    private Camera firstPersonCamera;

    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera; //�÷��̾��� ī�޶� ��ü�� �����´�
        CreateGrid();
    }

    //�׸��� ���� Glzmo ǥ���ϴ� �޼���
    private void OnDrawGizmos()               //����Ƽ Sceneâ�� ���̴� Debug �׸�
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


    //�׸��带 �����ϰ� ���� �ʱ�ȭ�ϴ� �޼���
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
                Vector3Int cellPosition = new Vector3Int(x, 0, z); //�� ��ġ
                Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition); //�׸��� �Լ��� ���ؼ� ���� ������ ��ġ�� �����´�
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

    //�׸��� ���� �ǹ���  ��ġ�ϴ� �޼���
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
    //�׸��� ������ �Ź��� �����ϴ� �޼��� 
    private void RemoveBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];  //��ġ ������� cell�� �޾ƿ´�.
        if (cell.IsOccupied)                                    //�ش� ��ġ�� �ǹ��� �ִ��� Ȯ���Ѵ�. 
        {
            Destroy(cell.Building);                                     //Cell �ǹ��� �����Ѵ�. 
            cell.IsOccupied = false;                                    //�ǹ� Ȯ�� ��
            cell.Building = null;                                       //Cell �� ���� ���� null ������ ����
        }
    }

    //���õ� ���� ���̶���Ʈ�ϴ� �޽�
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

        //Ư¡ ���� �ǹ��� ������ ������ �ƴϸ� �ʷϻ�
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        GameObject highlightObject = cell.Building != null ? cell.Building : transform.GetChild(gridPosition.x * height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied ? Color.red : Color.green;
    }

    //�׸��� ������ ��ȿ���� Ȯ���ϴ� �޼���
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width &&
            gridPosition.z >= 0 && gridPosition.z < height;
    }
    //�÷��̾ ���� �ִ� ��ġ�� ����ϴ� �޼���
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
        //3��Ī ���
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
