using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;          //�̱���
    public GameObject textPrefab;                        //UI �ؽ�Ʈ ������

    private void Awake()
    {
        Instance = this;                                 //�̱��� ��� 
    }

    public void Show(string text, Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);            //���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ

        GameObject textObj = Instantiate(textPrefab, transform);                 //ui �ؽ�Ʈ ����
        textObj.transform.position = screenPos;

        TextMeshProUGUI temp = textObj.GetComponent<TextMeshProUGUI>();
        if (temp != null)
        {
            temp.text = text;

            StartCoroutine(AnimateText(textObj));                                 //���� �ִϸ��̼� ȿ�� ����
        }
    }
    private IEnumerator AnimateText(GameObject textObj)
    {
        float duration = 1f;                                                       //���� �ð�
        float timer = 0;                                                           //����� Ÿ�̸� 

        Vector3 startPos = textObj.transform.position;
        TextMeshProUGUI temp = textObj.GetComponent<TextMeshProUGUI>();            //�޾ƿ� ������Ʈ���� TMP ��Ʈ ����

        while (timer < duration)                                                   //Ÿ�̸Ӱ� 1�� ������ 
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            textObj.transform.position = startPos + Vector3.up * (progress * 50f);  //��Ʈ�� ���� �ö�� ȿ���� �ش�. 

            if (temp != null)
            {
                temp.alpha = 1 - progress;
            }

            yield return null;
        }
        Destroy(textObj);
    }
}