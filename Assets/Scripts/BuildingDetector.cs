using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                //������ ���� ����
    public Vector3 lastPosition;                   //�÷��� ������ ����
    public float moveThreshold = 0.1f;             //�̵� ���� �Ӱ谪
    public ConstructibleBuilding currentNearbyBuilding;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;              //���� �� ���� ��ġ�� ������ ��ġ�� ����
        CheckForBuilding();                             //�ʱ� �ǹ� üũ ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) > moveThreshold)    //�÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ 
        {
            CheckForBuilding();                                                    //�̵��� ������ üũ 
            lastPosition = transform.position;                                     //���� ��ġ�� ������ ��ġ�� ������Ʈ
        }

        //����� �������� �ְ� E Ű�� ������ �� ������ ���� 
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            currentNearbyBuilding.StartConstruction(GetComponent<PlayerInventory>());         //PlayerInventroy�� �����Ͽ� �Ǽ� ���� �Լ� ȣ�� 
        }
    }
    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);  //���� ���� ���� ��� �ݶ��̴��� ã��

        float closestDistance = float.MaxValue;                     //���� ����� �Ÿ��� �ʱⰪ
        ConstructibleBuilding closestBuilding = null;               //���� ����� ������ �ʱⰪ

        foreach (Collider collider in hitColliders)                 //�� �ݶ��̴��� �˻��Ͽ� ���� ������ ������ ã��
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();            //�ǹ� ����
            if (building != null && building.canBuild && !building.isConstructed)        //�ǹ� �ְ� ���� �������� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);     //�Ÿ� ���
                if (distance < closestDistance)                                                     //�� ����� �ǹ� �߰� �� ������Ʈ 
                {
                    closestDistance = distance;
                    closestBuilding = building;
                }
            }
        }

        if (closestBuilding != currentNearbyBuilding)        //���� ����� �ǹ��� ����Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyBuilding = closestBuilding;        //���� ����� �ǹ� ������Ʈ
            if (currentNearbyBuilding != null)
            {
                if (FloatingTextManager.Instance != null)
                {
                    Vector3 textPostion = transform.position + Vector3.up * 0.5f;                   //������ ��ġ���� �ణ ���� �ؽ�Ʈ ����
                    FloatingTextManager.Instance.Show(
                        $"[F] Ű�� {currentNearbyBuilding.buildingName} �Ǽ� (���� {currentNearbyBuilding.requiredTree} �� �ʿ�)"
                        , currentNearbyBuilding.transform.position + Vector3.up
                        );
                }
            }
        }
    }

}