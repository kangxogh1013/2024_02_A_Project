using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


//������ ���� ����
public enum ItemType
{
    Crystal,                //ũ����Ż
    Plant,                  //�Ĺ�
    Bush,                   //��Ǯ
    Tree,                   //���� 
    VegetableStew,          //��ä ��Ʃ (��� ȸ����)
    FruitSalad,             //���� ������ ( ��� ȸ����)
    RepairKit               //���� ŰƮ (���ֺ� ������)
}
public class ItemDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                //������ ���� ����
    private Vector3 lastPosition;                   //�÷��̾��� ������ ��ġ ���� (�÷��̾� �̵��� ���� �� ��� �ֺ��� ã�� ���� ����)
    private float moveThreshold = 0.1f;             //�̵� ���� �Ӱ谪
    private Collectibleltem currentNearbyItem;      //���� ���� ������ �ִ� ���� ������ ������

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;           //���� �� ���� ��ġ�� ������ ��ġ�� ���� 
        CheckForItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) > moveThreshold)     //�÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ 
        {
            CheckForItems();                                                        //�̵��� ������ üũ 
            lastPosition = transform.position;                                      //���� ��ġ�� ������ ��ġ�� ������Ʈ 
        }
        //����� �������� �ְ� E Ű�� ������ �� ������ ���� 
        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());         //PlayerInventroy�� �����Ͽ� ������ ����
        }
    }

    //�ֺ��� ���� ������ �������� �����ϴ� �Լ� 
    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);  //���� ���� ���� ��� �ݶ��̴��� ã�ƿ�

        float closestDistance = float.MaxValue;       //���� ����� �Ÿ��� �ʱⰪ
        Collectibleltem closestItem = null;           //���� ����� ������ �ʱⰪ

        foreach (Collider collider in hitColliders)   //�� �ݶ��̴��� �˻��Ͽ� ���� ������ �������� ã��
        {
            Collectibleltem item = collider.GetComponent<Collectibleltem>();        //�������� ���� 
            if (item != null && item.canCollect)      //�������� �ְ� ���� �������� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);   //�Ÿ� ���
                if (distance < closestDistance)                                                   //�� ����� �������� �߰� �� ������Ʈ 
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }
        if (closestItem != currentNearbyItem)            //���� ����� �������� ����Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyItem = closestItem;            //���� ����� ������ ������Ʈ 
            if (currentNearbyItem != null)
            {
                Debug.Log($"[E] Ű�� ���� {currentNearbyItem.itemName} ���� ");           //���ο� ������ ���� �޼��� ǥ��
            }
        }
    }
    private void OnDrawGizmos()                //����Ƽ Sceneâ�� ���̴� Debug�׸�
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}