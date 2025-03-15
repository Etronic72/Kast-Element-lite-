using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для использования TextMeshPro
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainScript : MonoBehaviour
{
    public int balance;
    public TextMeshProUGUI balanceText;  // Заменили Text на TextMeshProUGUI

    public GameObject ObjectPanel;
    public GameObject ResourcesPanel;
    public GameObject InventoryPanel;
    public GameObject OPtionPanel;
    public Button ObjectButton;
    public Button ResourcesButton;
    public Button InventoryButton;
    public Button OPtionButton;
    public Button AllSell;
    public  TextMeshProUGUI AllSellText; // Заменили TextMeshProUGUI на Text
    public int CountElement;
    public Text CountElement_text;

    private Save2 bal = new Save2();

    public Color NotEnoughMoneyColor = Color.red;
    public Color EnoughMoneyColor = Color.white;

    private AllDate allDateScript;


    private void Awake()
    {
        
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("SV1")){
            bal= JsonUtility.FromJson<Save2>(PlayerPrefs.GetString("SV1"));
            balance = bal.balance_save;
        }
        ObjectButton.onClick.AddListener(() => TogglePanel(ObjectPanel));
        ResourcesButton.onClick.AddListener(() => TogglePanel(ResourcesPanel));
        InventoryButton.onClick.AddListener(() => TogglePanel(InventoryPanel));
        OPtionButton.onClick.AddListener(() => TogglePanel(OPtionPanel));

        allDateScript = FindObjectOfType<AllDate>(); // Находим скрипт AllDate
        
    }

    void Update()
    {

        balanceText.text = balance + "$";
       AllSellText.text = "+" + allDateScript.totalSellValue + "$";
        OnApplicationQuit();
        
    }

    // Метод для обновления UI для ресурсов
     private void UpdateResourceUI()
    {
        for (int i = 0; i < allDateScript.itemdata_res.Count; i++)
        {
            res resourceItem = allDateScript.itemdata_res[i];
            if (resourceItem.nameText != null)
                resourceItem.nameText.text = $"{resourceItem.name}: {resourceItem.lvl} lvl";

                 Button button = resourceItem.sellText.transform.parent.GetComponent<Button>();
                if (balance >= resourceItem.price)
                {
                   resourceItem.sellText.text = $"Buy {resourceItem.price}$";
                    SetButtonColor(button, EnoughMoneyColor);
                }
                else
                {
                    SetButtonColor(button, NotEnoughMoneyColor);
                }
            

        }
    }

    private void UpdateObjectUI()
    {
        for (int i = 0; i < allDateScript.itemdata_obj.Count; i++)
        {
            obj objectItem = allDateScript.itemdata_obj[i];
            if (objectItem.nameText != null)
                objectItem.nameText.text = $"{objectItem.name}: {objectItem.lvl} lvl";

                 Button button = objectItem.sellText.transform.parent.GetComponent<Button>();
                if (balance >= objectItem.price)
                {
                    objectItem.sellText.text = $"Buy: {objectItem.price}$";
                    SetButtonColor(button, EnoughMoneyColor);

                }
                else
                {
                    SetButtonColor(button, NotEnoughMoneyColor);
                }
            
        }
    }

     private void SetButtonColor(Button button, Color color)
    {
        if (button != null)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = color;
            cb.highlightedColor = color * 1.2f;
            cb.pressedColor = color * 0.8f;
            button.colors = cb;
        }
    }

    


    void TogglePanel(GameObject panel)
    {
        if (panel == ObjectPanel)
        {
            ObjectPanel.SetActive(true);
            ResourcesPanel.SetActive(false);
            InventoryPanel.SetActive(false);
            OPtionPanel.SetActive(false);
        }
        if (panel == ResourcesPanel)
        {
            ObjectPanel.SetActive(false);
            ResourcesPanel.SetActive(true);
            InventoryPanel.SetActive(false);
            OPtionPanel.SetActive(false);
        }
        if (panel == InventoryPanel)
        {
            ObjectPanel.SetActive(false);
            ResourcesPanel.SetActive(false);
            InventoryPanel.SetActive(true);
            OPtionPanel.SetActive(false);
        }
        if (panel == OPtionPanel)
        {
            ObjectPanel.SetActive(false);
            ResourcesPanel.SetActive(false);
            InventoryPanel.SetActive(false);
            OPtionPanel.SetActive(true);
        }
    }

    // Обработчик нажатия на кнопку ресурса
    public void HandleResourceButtonClick(res resourceItem)
    {
        UpdateResourceUI();
        Debug.Log($"Resource clicked! Name: {resourceItem.name}, Sell: {resourceItem.sell}, Price: {resourceItem.price}");
        if (balance >= resourceItem.price)
        {
            resourceItem.lvl++;
            resourceItem.isBuy = 1;
            balance = balance - resourceItem.price;
            resourceItem.price = (resourceItem.price + 1) * 2;
            resourceItem.sellText.text = $"Buy: {resourceItem.price}$";
            if (resourceItem.lvl == 1){
                CountElement++;
                CountElement_text.text = CountElement + "/50";
            }
        }
        else
        {
            Debug.Log("no");
        }
        UpdateResourceUI();
    }

    // Обработчик нажатия на кнопку объекта
    public void HandleObjectButtonClick(obj objectItem)
    {
        Debug.Log($"Object clicked! Name: {objectItem.name}, Sell: {objectItem.sell}, Price: {objectItem.price}");
        if (balance >= objectItem.price)
        {
            // Флаг, показывающий, достаточно ли ресурсов для крафта
            bool canCraft = true;

            if (objectItem.craft != null)
            {
                foreach (CraftItem craftItem in objectItem.craft)
                {
                    inv inventoryItem = allDateScript.itemdata_inv.Find(inv => inv.name == craftItem.name);
                    // Проверка, есть ли ресурс в инвентаре и достаточно ли его
                    if (inventoryItem == null || inventoryItem.count < craftItem.count)
                    {
                        canCraft = false; // Если хоть одного ресурса не хватает, крафт невозможен
                        break; // Прерываем цикл
                    }
                }
            }

            if (canCraft)
            {
                 objectItem.isBuy = 1;
                 objectItem.lvl++;
                 balance = balance - objectItem.price;
                 objectItem.price = (objectItem.price + 1) * 2;
                 objectItem.sellText.text = $"Buy: {objectItem.price}$";
                 if (objectItem.lvl == 1){
                      CountElement++;
                      CountElement_text.text = CountElement + "/50";
                 }
                 
                if (allDateScript.CheckCraftRequirements(objectItem))
                {
                      allDateScript.CraftObject(objectItem);
                 }
                else
                    Debug.Log("Недостаточно ресурсов для крафта");
            }
            else
            {
                Debug.Log("Недостаточно ресурсов для покупки " + objectItem.name);
            }

        }
        else
        {
            Debug.Log("no");
        }
        UpdateObjectUI();

    }

    public void SellAll()
    {
        balance += allDateScript.totalSellValue;
        allDateScript.resValue = 0;
        allDateScript.objValue = 0;
        AllSellText.text = "+" + allDateScript.totalSellValue + "$";

        foreach (var resource in allDateScript.itemdata_res)
        {
            resource.count = 0;
        }
        foreach (var item in allDateScript.itemdata_obj)
        {
            item.count = 0;
        }
        for ( int i =0; i<43; i++){
            allDateScript.itemdata_inv[i].countText.text = "0";
        }
    }

    //save данных
    private void OnApplicationQuit()
    {
        bal.balance_save = balance;
        PlayerPrefs.SetString("SV1", JsonUtility.ToJson(bal));
    }
//
}

[System.Serializable]
public class Save2{
    public int balance_save;
}