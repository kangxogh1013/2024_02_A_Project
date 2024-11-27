using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    public static CraftingUIManager Instance { get; private set; }  //�̱��� �ν��Ͻ�

    [Header("UI References")]
    public GameObject craftingPanel;                        //���� UI �г�
    public TextMeshProUGUI buildingNameText;                //�ǹ� �̸� �ؽ�Ʈ 
    public Transform recipeContainer;                       //������ ��ư���� �� �����̳�
    public Button closeButton;                              //�ݱ� ��ư
    public GameObject recipeButtonPefabs;                   //������ ��ư ������

    private BuildingCrafter currentCrafter;                 //���� ���õ� �ǹ��� ���� �ý���

    private void Awake()
    {
        if (Instance == null) Instance = this;              //�̱��� ����
        else Destroy(gameObject);

        craftingPanel.SetActive(false); 
    }

    private void RefreshRecipeList()                                        //������ ��� ���� ��ħ 
    {
        foreach (Transform child in recipeContainer)                        //���� ������ ��ư�� ���� 
        {
            Destroy(child.gameObject);
        }

        if (currentCrafter != null && currentCrafter.recipes != null)       //�� ������ ��ư�� ����
        {
            foreach (CraftingRecipe recipe in currentCrafter.recipes)
            {
                GameObject buttonObj = Instantiate(recipeButtonPefabs, recipeContainer);
                RecipeButton recipeButton = buttonObj.GetComponent<RecipeButton>();
                recipeButton.Setup(recipe, currentCrafter);
            }
        }
    }

    public void ShowUI(BuildingCrafter crafter)             //UI ǥ�� 
    {
        currentCrafter = crafter;
        craftingPanel.SetActive(true);                      //�г��� ���ش�.

        Cursor.visible = true;                              //���콺 Ŀ�� ǥ�� ��� ����
        Cursor.lockState = CursorLockMode.None;

        if (crafter != null)
        {
            buildingNameText.text = crafter.GetComponent<ConstructibleBuilding>().buildingName;
            RefreshRecipeList();
        }
    }

    public void HideUI()
    {
        craftingPanel.SetActive(false);                      //�г��� ����.
        currentCrafter = null;

        Cursor.visible = false;                              //���콺 Ŀ�� ǥ�� �� ��� ����
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Start()
    {
        closeButton.onClick.AddListener(() => HideUI());
    }
}
