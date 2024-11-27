using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    public static CraftingUIManager Instance { get; private set; }  //싱글톤 인스턴스

    [Header("UI References")]
    public GameObject craftingPanel;                        //조합 UI 패널
    public TextMeshProUGUI buildingNameText;                //건물 이름 텍스트 
    public Transform recipeContainer;                       //레시피 버튼들이 들어갈 컨테이너
    public Button closeButton;                              //닫기 버튼
    public GameObject recipeButtonPefabs;                   //레시피 버튼 프리팹

    private BuildingCrafter currentCrafter;                 //현재 선택된 건물의 제작 시스템

    private void Awake()
    {
        if (Instance == null) Instance = this;              //싱글톤 설정
        else Destroy(gameObject);

        craftingPanel.SetActive(false); 
    }

    private void RefreshRecipeList()                                        //레시피 목록 새로 고침 
    {
        foreach (Transform child in recipeContainer)                        //기존 레시피 버튼들 제거 
        {
            Destroy(child.gameObject);
        }

        if (currentCrafter != null && currentCrafter.recipes != null)       //새 레시피 버튼들 생성
        {
            foreach (CraftingRecipe recipe in currentCrafter.recipes)
            {
                GameObject buttonObj = Instantiate(recipeButtonPefabs, recipeContainer);
                RecipeButton recipeButton = buttonObj.GetComponent<RecipeButton>();
                recipeButton.Setup(recipe, currentCrafter);
            }
        }
    }

    public void ShowUI(BuildingCrafter crafter)             //UI 표시 
    {
        currentCrafter = crafter;
        craftingPanel.SetActive(true);                      //패널을 켜준다.

        Cursor.visible = true;                              //마우스 커서 표시 잠금 해제
        Cursor.lockState = CursorLockMode.None;

        if (crafter != null)
        {
            buildingNameText.text = crafter.GetComponent<ConstructibleBuilding>().buildingName;
            RefreshRecipeList();
        }
    }

    public void HideUI()
    {
        craftingPanel.SetActive(false);                      //패널을 끈다.
        currentCrafter = null;

        Cursor.visible = false;                              //마우스 커서 표시 및 잠금 해제
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Start()
    {
        closeButton.onClick.AddListener(() => HideUI());
    }
}
