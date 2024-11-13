using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;          //싱글톤
    public GameObject textPrefab;                        //UI 텍스트 프리팹

    private void Awake()
    {
        Instance = this;                                 //싱글톤 등록 
    }

    public void Show(string text, Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);            //월드 좌표를 스크린 좌표로 변환

        GameObject textObj = Instantiate(textPrefab, transform);                 //ui 텍스트 생성
        textObj.transform.position = screenPos;

        TextMeshProUGUI temp = textObj.GetComponent<TextMeshProUGUI>();
        if (temp != null)
        {
            temp.text = text;

            StartCoroutine(AnimateText(textObj));                                 //만든 애니메이션 효과 진행
        }
    }
    private IEnumerator AnimateText(GameObject textObj)
    {
        float duration = 1f;                                                       //동작 시간
        float timer = 0;                                                           //사용할 타이머 

        Vector3 startPos = textObj.transform.position;
        TextMeshProUGUI temp = textObj.GetComponent<TextMeshProUGUI>();            //받아온 오브젝트에서 TMP 폰트 참조

        while (timer < duration)                                                   //타이머가 1초 전까지 
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            textObj.transform.position = startPos + Vector3.up * (progress * 50f);  //폰트를 위로 올라는 효과를 준다. 

            if (temp != null)
            {
                temp.alpha = 1 - progress;
            }

            yield return null;
        }
        Destroy(textObj);
    }
}
