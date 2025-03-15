using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем TextMeshPro
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class AllDate : MonoBehaviour
{

     // События для передачи нажатий на кнопки
    public delegate void ButtonClickedHandler(Item itemData);
    public event ButtonClickedHandler OnButtonClicked;

    public string jsonFilePath = "base.json";

    public GameObject itemPrefab; // Префаб для отображения элементов инвентаря
    public Transform parentTransform_item; // Место для размещения элементов инвентаря

    public GameObject itemPrefab2; // Префаб для отображения ресурсов
    public Transform parentTransform_resources; // Место для размещения элементов ресурсов

    public GameObject itemPrefab3; // Префаб для отображения объектов
    public Transform parentTransform_object; // Место для размещения элементов объектов

    public List<Item> itemdata = new List<Item>(); // Список для хранения данных из JSON
    public List<inv> itemdata_inv = new List<inv>(); // Список для инвентаря
    public List<res> itemdata_res = new List<res>(); // Список для ресурсов
    public List<obj> itemdata_obj = new List<obj>(); // Список для объектов

    private Save sv = new Save();

    private MainScript mainScript; // Ссылка на MainScript для работы с балансом

    public int totalSellValue = 0;
    public int resValue = 0;
    public int objValue = 0;

    public bool Creat;

     public int open = 0;



    void Start()
    {
        if (PlayerPrefs.HasKey("SV")){
            sv= JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            open = sv.open_save;

            for (int i =0; i<itemdata.Count; i++)
            {
                itemdata[i].lvl = sv.lvl_save[i];
                itemdata[i].count = sv.count_save[i];
                itemdata[i].isBuy = sv.isBuy_save[i];
                itemdata[i].price = sv.price_save[i];
            }
        }
        
        
        
        
        
        mainScript = FindObjectOfType<MainScript>();
        JSonLoadInventory(itemPrefab, parentTransform_item);
        JSonLoadResources(itemPrefab2, parentTransform_resources);
        JSonLoadObject(itemPrefab3, parentTransform_object);

        



        TextMeshProUGUI[] allTexts = GameObject.FindObjectsOfType<TextMeshProUGUI>();
        List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>(allTexts);
        textList.Reverse();

        int a = 0;

        for (int i = 0; i < textList.Count; i++)
        {
            TextMeshProUGUI text = textList[i];
            if (text != null && a < itemdata_inv.Count)
            {
                if (text.gameObject.name == "Count")
                {
                   if (itemdata_inv[a] != null)
                   {
                      itemdata_inv[a].countText = text; // Это теперь безопасно, так как оба типа TextMeshProUGUI
                      //Debug.Log("Текст для " + itemdata_inv[a].name + " присвоен: " + text.text);
                      a++;
                   }
                   else
                   {
                       //Debug.LogError("Элемент в itemdata_inv с индексом " + a + " равен null.");
                   }
                }
            }
            else
            {
                if (a >= itemdata_inv.Count)
                {
                    Debug.LogWarning("Переменная 'a' больше или равна количеству элементов в itemdata_inv.");
                    break;
                }
                 else
                {
                    Debug.LogError("Не удалось присвоить текст для элемента с индексом " + i);
                }
            }
        }      


        if(open == 1){
            for (int i = 0; i < 43; i++){
                if (i<=4){
                    itemdata_res[i].lvl = itemdata[i].lvl;
                    itemdata_res[i].count = itemdata[i].count;
                    itemdata_res[i].isBuy = itemdata[i].isBuy;
                    itemdata_res[i].price = itemdata[i].price;
                }else{
                    int c=i-5;
                    itemdata_obj[c].lvl = itemdata[i].lvl;
                    itemdata_obj[c].count = itemdata[i].count;
                    itemdata_obj[c].isBuy = itemdata[i].isBuy;
                    itemdata_obj[c].price = itemdata[i].price;
                }
            }
            Debug.Log("yes");
        }else{
            open = 1;
        }

        StartCoroutine(SaveData());  
        
        for (int i = 0; i < 43; i++){
            if (i<=4){
                itemdata_res[i].sellText.text = $"Buy: {itemdata_res[i].price}$";

            }else{
                int c=i-5;
                itemdata_obj[c].sellText.text = $"Buy: {itemdata_obj[c].price}$";

            }
        }

        // Оптимизация с корутинами
        foreach (var resource in itemdata_res)
        {
            StartCoroutine(UpdateResourceCountOverTime(resource));
        }

        foreach (var item in itemdata_obj)
        {
            StartCoroutine(UpdateObjectCountOverTime(item));
        }
    }

    void Update()
    {
        totalSellValue = resValue + objValue; // Обновляем общую сумму продажи
        
    }

    public IEnumerator SaveData(){
        while(true){
            yield return new WaitForSeconds(1f);
            OnApplicationQuit();
        }
    }

    public void JSonLoadInventory(GameObject prefab, Transform parent)
    {
         string filePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
        string json = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(filePath);
             while (!reader.isDone)
            {
            }
            json = reader.text;
         }
        else
        {
            json = File.ReadAllText(filePath);
        }

        ItemsList data = JsonConvert.DeserializeObject<ItemsList>(json);

        if (data != null && data.items != null){
           foreach (Item item in data.items)
            {
                inv inventoryItem = new inv
                {
                    name = item.name,
                    count = item.count
                };
                 itemdata_inv.Add(inventoryItem);
            }
           
            foreach (inv invItem in itemdata_inv)
            {
                GameObject item = Instantiate(prefab, parent);
                item.SetActive(true);
                
               TextMeshProUGUI[] nameTexts = item.GetComponentsInChildren<TextMeshProUGUI>();
                
                foreach (TextMeshProUGUI text in nameTexts)
                {
                   if (text.name == "Name")
                    {
                         text.text = invItem.name;
                    }
                    else if (text.name == "Count")
                    {
                       invItem.countText = text;
                        text.text = invItem.count.ToString();
                   }
                }
           }
        }
    }


    public void JSonLoadResources(GameObject prefab, Transform parent)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
        string json = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(filePath);
            while (!reader.isDone) { }
            json = reader.text;
        }
        else
        {
            json = File.ReadAllText(filePath);
        }

        ItemsList data = JsonConvert.DeserializeObject<ItemsList>(json);

        if (data != null && data.items != null)
         {
            int limit = 5;
            for (int i = 0; i < data.items.Count && i < limit; i++)
            {
                 Item item = data.items[i];
                res resourceItem = new res
               {
                    id = item.id,
                    name = item.name,
                    lvl = item.lvl,
                    time = item.time,
                    sell = item.sell,
                    price = item.price,
                    image = item.image,
                    images = item.images,
                    Background = item.Background,
                    button = item.button
               };
               itemdata_res.Add(resourceItem);
            }
           foreach (res resItem in itemdata_res)
            {
                GameObject item = Instantiate(prefab, parent);
                item.SetActive(true);

                TextMeshProUGUI[] resourceTexts = item.GetComponentsInChildren<TextMeshProUGUI>();

                // Get all Image components in children of the item
                Image[] resourceImages = item.GetComponentsInChildren<Image>();

                Button[] resourceBut = item.GetComponentsInChildren<Button>();

                // Loop through each Image component
                foreach (var image in resourceImages)
                {
                    // Check if the Image name matches "Image"
                    if (image.name == "Image")
                    {
                        // Load the texture from Resources
                        Texture2D texture = Resources.Load<Texture2D>(resItem.images);
                         // Create a new Sprite from the texture
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                        // Assign the Sprite to the Image component
                        image.sprite = sprite;

                        // Optionally store a reference to the Image in resItem
                        resItem.image = image;
                    }else if(image.name == "win"){
                        resItem.Background = image;
                    }
                }

                foreach (var but in resourceBut)
                {
                    // Check if the Image name matches "Image"
                    if (but.name == "da")
                    {
                        resItem.button = but;
                    }
                }


                foreach (TextMeshProUGUI text in resourceTexts)
                {
                    if (text.name == "Name")
                     {
                       resItem.nameText = text;
                        text.text = $"{resItem.name}: {resItem.lvl} lvl";
                    }
                    else if (text.name == "Description")
                     {
                       resItem.descriptionText = text;
                        text.text = $"Sell: {resItem.sell}, Time: {resItem.time}";
                   }
                     else if (text.name == "Sell")
                   {
                        resItem.sellText = text;
                         text.text = $"Buy: {resItem.price}$";
                    }
                }

                 Button button = item.GetComponentInChildren<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnResourceButtonClicked(resItem));
                }


                

            }
        }
    }

    private void OnResourceButtonClicked(res resourceItem)
    {
        MainScript mainScript = FindObjectOfType<MainScript>();
        if (mainScript != null)
        {
            mainScript.HandleResourceButtonClick(resourceItem);
           
        }
    }


    private IEnumerator UpdateResourceCountOverTime(res resourceItem)
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (resourceItem.isBuy == 1)
            {
                if (timer >= resourceItem.time)
                {
                    resourceItem.count += resourceItem.lvl;
                    resValue += resourceItem.sell * resourceItem.lvl;

                    inv invItem = itemdata_inv.Find(inv => inv.name == resourceItem.name);
                    if (invItem != null)
                    {
                        invItem.count = resourceItem.count;
                        invItem.countText.text = invItem.count.ToString();
                    }

                    timer = 0f;

                    resourceItem.Background.fillAmount = 1f; 
                }
                else
                {
                    resourceItem.Background.fillAmount = timer / resourceItem.time; // Увеличиваем fillAmount
                }
            }

            yield return null;
        }
    }


    public void JSonLoadObject(GameObject prefab, Transform parent)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
        string json = "";
         if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(filePath);
            while (!reader.isDone)
            {
            }
            json = reader.text;
        }
        else
        {
           json = File.ReadAllText(filePath);
        }
        ItemsList data = JsonConvert.DeserializeObject<ItemsList>(json);

        if (data != null && data.items != null)
        {
             int limit = 50;

            for (int i = 5; i < data.items.Count && i < limit; i++)
            {
                Item item = data.items[i];
                 obj objectItem = new obj
                {
                    id = item.id,
                    name = item.name,
                    lvl = item.lvl,
                    time = item.time,
                    sell = item.sell,
                    price = item.price,
                    craft = item.craft,
                    count = item.count,
                    image = item.image,
                    images = item.images,
                    Background = item.Background,
                    button = item.button
                 };

                itemdata_obj.Add(objectItem);
            }

             foreach (obj objItem in itemdata_obj)
            {
                GameObject item = Instantiate(prefab, parent);
                item.SetActive(true);

                TextMeshProUGUI[] objectTexts = item.GetComponentsInChildren<TextMeshProUGUI>();

                Image[] resourceImages = item.GetComponentsInChildren<Image>();

                Button[] resourceBut = item.GetComponentsInChildren<Button>();

                // Loop through each Image component
                foreach (var image in resourceImages)
                {
                    // Check if the Image name matches "Image"
                    if (image.name == "Image")
                    {
                        // Load the texture from Resources
                        Texture2D texture = Resources.Load<Texture2D>(objItem.images);
                         // Create a new Sprite from the texture
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                        // Assign the Sprite to the Image component
                        image.sprite = sprite;

                        // Optionally store a reference to the Image in resItem
                        objItem.image = image;
                    }else if(image.name == "win"){
                        objItem.Background = image;
                    }
                }

                foreach (var but in resourceBut)
                {
                    // Check if the Image name matches "Image"
                    if (but.name == "da")
                    {
                        objItem.button = but;
                    }
                }

               foreach (TextMeshProUGUI text in objectTexts)
                {
                    if (text.name == "Name")
                    {
                       objItem.nameText = text;
                         text.text = $"{objItem.name}: {objItem.lvl} lvl";
                    }
                    else if (text.name == "Description")
                    {
                         objItem.descriptionText = text;
                        text.text = $"Sell: {objItem.sell}, Time: {objItem.time}";
                    }
                    else if (text.name == "Sell")
                    {
                         objItem.sellText = text;
                         text.text = $"Buy: {objItem.price}$";
                    }
                    else if (text.name == "Craft")
                   {
                       foreach (CraftItem craftItem in objItem.craft)
                        {
                           text.text += $"{craftItem.name}: {craftItem.count}  ";
                        }
                     }
                }

               Button button = item.GetComponentInChildren<Button>();
               if (button != null)
                {
                   button.onClick.AddListener(() => OnObjectButtonClicked(objItem));
                }
             }
        }
    }


    private void OnObjectButtonClicked(obj objectItem)
    {
        MainScript mainScript = FindObjectOfType<MainScript>();
        if (mainScript != null)
        {
            
            mainScript.HandleObjectButtonClick(objectItem);
            
        }
    }


    private IEnumerator UpdateObjectCountOverTime(obj objectItem)
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            
            if (objectItem.isBuy == 1)
            {
                if (timer >= objectItem.time)
                {
                    objectItem.count += objectItem.lvl;
                    objValue += objectItem.sell * objectItem.lvl;

                    inv invItem = itemdata_inv.Find(inv => inv.name == objectItem.name);
                    if (invItem != null)
                    {
                        invItem.count = objectItem.count;
                        invItem.countText.text = invItem.count.ToString();
                    }
                    CraftObject(objectItem);
                        
                    timer = 0f; // Сбрасываем fillAmount для следующего цикла

                    objectItem.Background.fillAmount = 1f; 
                }
                else
                {
                    objectItem.Background.fillAmount = timer / objectItem.time; // Увеличиваем fillAmount
                }
                
                
            }

            yield return null;
        }
    }


        // Метод проверки ресурсов для крафта
    public bool CheckCraftRequirements(obj objectItem)
    {
        
        foreach (CraftItem craftItem in objectItem.craft)
         {
            inv inventoryItem = itemdata_inv.Find(inv => inv.name == craftItem.name);
            if (inventoryItem == null || inventoryItem.count > craftItem.count)
            {
                Creat=true;
                return true;

            }
        }
        Creat=false;
        return false;
    }


    public void CraftObject(obj objectItem)
    {
        if (objectItem.craft == null) return;
        foreach (CraftItem craftItem in objectItem.craft)
        {
            inv inventoryItem = itemdata_inv.Find(inv => inv.name == craftItem.name);
            res resourcesItem = itemdata_res.Find(res => res.name == craftItem.name);
            if (inventoryItem != null)
            {
                
                resourcesItem.count -= craftItem.count;
                resValue -= resourcesItem.sell * craftItem.count;
                //inventoryItem.countText.text = inventoryItem.count.ToString();
                Debug.Log("Создано " + objectItem.name + " with " + craftItem.name);
                Debug.Log($"{inventoryItem.count}");
            }
        }
    }


    //save данных
    private void OnApplicationQuit()
    {
        for (int i = 0; i < 43; i++){
            if (i<=4){
                itemdata[i].lvl = itemdata_res[i].lvl;
                itemdata[i].count = itemdata_res[i].count;
                itemdata[i].price = itemdata_res[i].price;
                itemdata[i].isBuy = itemdata_res[i].isBuy;
                
            }else{
                int c=i-5;
                itemdata[i].lvl = itemdata_obj[c].lvl;
                itemdata[i].count = itemdata_obj[c].count;
                itemdata[i].price = itemdata_obj[c].price;
                itemdata[i].isBuy = itemdata_obj[c].isBuy;
                
            }
        }
        sv.lvl_save = new int[itemdata.Count];
        sv.count_save = new int[itemdata.Count];
        sv.isBuy_save = new int[itemdata.Count];
        sv.price_save = new int[itemdata.Count];
        sv.open_save = open;
        for (int i =0; i<itemdata.Count; i++)
        {
            sv.lvl_save[i] = itemdata[i].lvl;
            sv.count_save[i] = itemdata[i].count;
            sv.isBuy_save[i] = itemdata[i].isBuy;
            sv.price_save[i] = itemdata[i].price;
        }
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(sv));
    }
    //
}


// Класс для представления объекта Item
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public int lvl;
    public int time;
    public int price;
    public int sell;
    public int count;
    public int isBuy;
     public List<CraftItem> craft; // Добавляем поле craft

    public Image image;
    public string images;

    public Image Background;

    public Button button;
}

// Класс для представления списка Item
[System.Serializable]
public class ItemsList
{
    public List<Item> items;
}

// Класс для представления элемента инвентаря
[System.Serializable]
public class inv
{
    public string name;
    public int count;
    public TextMeshProUGUI countText;
}

[System.Serializable]
public class res
{
    public int id;
    public string name;  // Название ресурса
    public int lvl;
    public int time;    // 
    public int sell;     // Цена продажи ресурса
    public int price;
    public int count;
    public Button up;
    public int isBuy;

    // Добавляем ссылки на компоненты Text
    public TextMeshProUGUI nameText; // Используем TextMeshProUGUI
    public TextMeshProUGUI descriptionText; // Используем TextMeshProUGUI
    public TextMeshProUGUI sellText; // Используем TextMeshProUGUI

    public Image image;
    public string images;

    public Image Background;

    public Button button;
}



[System.Serializable]
public class obj
{
    public int id;
    public string name;  // Название объекта
    public int lvl;
    public int time;    // 
    public int sell;     // Цена продажи объекта
    public int price;         // Цена продажи
    public int isBuy;
    public int count;
    public List<CraftItem> craft;  // Список предметов для крафта

    // Добавляем ссылки на компоненты Text
    public TextMeshProUGUI nameText; // Используем TextMeshProUGUI
    public TextMeshProUGUI descriptionText; // Используем TextMeshProUGUI
    public TextMeshProUGUI sellText; // Используем TextMeshProUGUI

    public Image image;
    public string images;

    public Image Background;

    public Button button;
}
[System.Serializable]
public class CraftItem
{
    public string name; // Название предмета для крафта
    public int count;   // Количество предмета для крафта
}

[System.Serializable]
public class Save{
    public int open_save;
    public int[] lvl_save;
    public int[] count_save;
    public int[] isBuy_save;
    public int[] price_save;
}