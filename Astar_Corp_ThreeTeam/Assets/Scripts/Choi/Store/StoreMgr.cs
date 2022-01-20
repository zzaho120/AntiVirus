using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum StoreKind
{ 
    None,
    Storage,
    Store
}

public class StoreMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject storeContents;
    public GameObject storePrefab;
    public GameObject storageContents;
    public GameObject storagePrefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text titleTxt;
    public Text itemNameTxt;
    public Text itemNumTxt;
    public Slider slider;
    public GameObject buyButton;
    public GameObject sellButton;

    public Dictionary<string, GameObject> storeObjs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> storageObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> storeWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, Consumable> storeConsumableInfo = new Dictionary<string, Consumable>();

    int storeLevel;
    int maxItemNum;
    StoreKind currentKind;
    string currentKey;

    public void Init()
    {
        storeLevel = playerDataMgr.saveData.storeLevel;
        Bunker storeLevelInfo = playerDataMgr.bunkerList["BUN_0006"];
        switch (storeLevel)
        {
            case 1:
                maxItemNum = storeLevelInfo.level1;
                break;
            case 2:
                maxItemNum = storeLevelInfo.level2;
                break;
            case 3:
                maxItemNum = storeLevelInfo.level3;
                break;
            case 4:
                maxItemNum = storeLevelInfo.level4;
                break;
            case 5:
                maxItemNum = storeLevelInfo.level5;
                break;
        }

        ClosePopup();

        //이전 정보 삭제.
        if (storageObjs.Count != 0)
        {
            foreach (var element in storageObjs)
            {
                Destroy(element.Value);
            }
            storageObjs.Clear();
            storageContents.transform.DetachChildren();
        }
        if (storageWeaponInfo.Count != 0) storageWeaponInfo.Clear();
        if (storageWeaponNumInfo.Count != 0) storageWeaponNumInfo.Clear();
        if (storageConsumableInfo.Count != 0) storageConsumableInfo.Clear();
        if (storageConsumableNumInfo.Count != 0) storageConsumableNumInfo.Clear();

        //생성.
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(storagePrefab, storageContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}개";

            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Storage); });

            storageObjs.Add(key, go);
            storageWeaponInfo.Add(element.Key, element.Value);
            storageWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            var go = Instantiate(storagePrefab, storageContents.transform);
            var child = go.transform.GetChild(0).gameObject;  
            var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}개";

            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Storage); });

            storageObjs.Add(key, go);
            storageConsumableInfo.Add(element.Key, element.Value);
            storageConsumableNumInfo.Add(element.Key, itemNum);
        }

        DisplayEquippables();

        currentKind = StoreKind.None;
        currentKey = null;
    }

    public void DisplayEquippables()
    {
        if (storeObjs.Count != 0)
        {
            foreach (var element in storeObjs)
            {
                Destroy(element.Value);
            }
            storeObjs.Clear();
            storeContents.transform.DetachChildren();
        }
        storeWeaponInfo.Clear();

        foreach (var element in playerDataMgr.equippableList)
        {
            var go = Instantiate(storePrefab, storeContents.transform);
            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Store); });

            storeObjs.Add(element.Key,go);
            storeWeaponInfo.Add(element.Key, element.Value);
        }

    }

    public void DisplayConsumables()
    { 
     if (storeObjs.Count != 0)
        {
            foreach (var element in storeObjs)
            {
                Destroy(element.Value);
            }
            storeObjs.Clear();
            storeContents.transform.DetachChildren();
        }
        storeConsumableInfo.Clear();

        foreach (var element in playerDataMgr.consumableList)
        {
            var go = Instantiate(storePrefab, storeContents.transform);
            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Store); });

            storeObjs.Add(element.Key, go);
            storeConsumableInfo.Add(element.Key, element.Value);
        }
    }

    public void SelectItem(string key, StoreKind kind)
    {
        if (currentKey != null)
        {
            if (kind == StoreKind.Storage)
            {
                var image = storageObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
            else if (kind == StoreKind.Store)
            {
                var image = storeObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }

        currentKey = key;
        currentKind = kind;

        if (currentKind == StoreKind.Storage)
        {
            var image = storageObjs[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (storageWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageWeaponInfo[currentKey].name;
                slider.maxValue = storageWeaponNumInfo[currentKey];
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                slider.maxValue = storageConsumableNumInfo[currentKey];
            }

            //팝업 관련.
            titleTxt.text = "판매";
            if (buyButton.activeSelf) buyButton.SetActive(false);
            if (!sellButton.activeSelf) sellButton.SetActive(true);
        }
        else if (currentKind == StoreKind.Store)
        {
            var image = storeObjs[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (storeWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storeWeaponInfo[currentKey].name;
                slider.maxValue =100;
            }
            else if (storeConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storeConsumableInfo[currentKey].name;
                slider.maxValue = 100;
            }

            //팝업 관련.
            titleTxt.text = "구매";
            if (sellButton.activeSelf) sellButton.SetActive(false);
            if (!buyButton.activeSelf) buyButton.SetActive(true);
        }

        slider.value = 0;
        itemNumTxt.text = $"0개";
        OpenPopup();
    }

    public void Operation()
    {
        if (currentKind == StoreKind.Storage)
            Sell(Mathf.FloorToInt(slider.value));
        else if (currentKind == StoreKind.Store)
            Buy(Mathf.FloorToInt(slider.value));

        ClosePopup();
    }

    public void Buy(int itemNum)
    {
        if (currentKey == null) return;

        if (storeWeaponInfo.ContainsKey(currentKey))
        {
            if (storageWeaponInfo.ContainsKey(currentKey))
            {
                //json.
                var id = storageWeaponInfo[currentKey].id;
                var index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] += itemNum;

                //playerDataMgr.
                playerDataMgr.currentEquippablesNum[currentKey] += itemNum;

                //현재데이터.
                storageWeaponNumInfo[currentKey] += itemNum;

                //GameObject.
                var child = storageObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";
            }
            else
            {
                //json.
                var id = playerDataMgr.equippableList[currentKey].id;
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(itemNum);

                //playerDataMgr.
                var weapon = playerDataMgr.equippableList[currentKey];
                playerDataMgr.currentEquippables.Add(currentKey, weapon);
                playerDataMgr.currentEquippablesNum.Add(currentKey, itemNum);

                //현재데이터.
                storageWeaponInfo.Add(currentKey, weapon);
                storageWeaponNumInfo.Add(currentKey, itemNum);

                //GameObject.
                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                string key = currentKey;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Storage); });

                storageObjs.Add(key, go);
            }
        }
        else if (storeConsumableInfo.ContainsKey(currentKey))
        {
            if (storageConsumableInfo.ContainsKey(currentKey))
            {
                //json.
                var id = storageConsumableInfo[currentKey].id;
                var index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableNumList[index] += itemNum;

                //playerDataMgr.
                playerDataMgr.currentConsumablesNum[currentKey] += itemNum;

                //현재데이터.
                storageConsumableNumInfo[currentKey] += itemNum;

                //GameObject.
                var child = storageObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";
            }
            else
            {
                //json.
                var id = storageConsumableInfo[currentKey].id;
                playerDataMgr.saveData.consumableList.Add(id);
                playerDataMgr.saveData.consumableNumList.Add(itemNum);

                //playerDataMgr.
                var consumable = playerDataMgr.consumableList[currentKey];
                playerDataMgr.currentConsumables.Add(currentKey, consumable);
                playerDataMgr.currentConsumablesNum.Add(currentKey, itemNum);

                //현재데이터.
                storageConsumableInfo.Add(currentKey, consumable);
                storageConsumableNumInfo.Add(currentKey, itemNum);

                //GameObject.
                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                string key = currentKey;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, StoreKind.Storage); });

                storageObjs.Add(key, go);
            }
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        storeObjs[currentKey].GetComponent<Image>().color = Color.white;
        currentKey = null;
        currentKind = StoreKind.None;
    }

    public void Sell(int itemNum)
    {
        if (currentKey == null) return;

        if (storageWeaponInfo.ContainsKey(currentKey))
        {
            if (storageWeaponNumInfo[currentKey] - itemNum != 0)
            {
                //json.
                var id = storageWeaponInfo[currentKey].id;
                var index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] -= itemNum;

                //playerDataMgr.
                playerDataMgr.currentEquippablesNum[currentKey] -= itemNum;

                //현재데이터.
                storageWeaponNumInfo[currentKey] -= itemNum;

                //GameObject.
                var child = storageObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";
            }
            else
            {
                //json.
                var id = playerDataMgr.equippableList[currentKey].id;
                var index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableList.Remove(currentKey);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);

                //playerDataMgr.
                playerDataMgr.currentEquippables.Remove(currentKey);
                playerDataMgr.currentEquippablesNum.Remove(currentKey);

                //현재데이터.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);

                //GameObject.
                Destroy(storageObjs[currentKey]);
                storageObjs.Remove(currentKey);
            }
        }
        else if (storageConsumableInfo.ContainsKey(currentKey))
        {
            if (storageConsumableNumInfo[currentKey] - itemNum != 0)
            {
                //json.
                var id = storageConsumableInfo[currentKey].id;
                var index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableNumList[index] -= itemNum;

                //playerDataMgr.
                playerDataMgr.currentConsumablesNum[currentKey] -= itemNum;

                //현재데이터.
                storageConsumableNumInfo[currentKey] -= itemNum;

                //GameObject.
                var child = storageObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{ storageConsumableNumInfo[currentKey]}개";
            }
            else
            {
                //json.
                var id = playerDataMgr.consumableList[currentKey].id;
                var index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableList.Remove(currentKey);
                playerDataMgr.saveData.consumableNumList.RemoveAt(index);

                //playerDataMgr.
                playerDataMgr.currentConsumables.Remove(currentKey);
                playerDataMgr.currentConsumablesNum.Remove(currentKey);

                //현재데이터.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);

                //GameObject.
                Destroy(storageObjs[currentKey]);
                storageObjs.Remove(currentKey);
            }
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        if(storageObjs.ContainsKey(currentKey)) storageObjs[currentKey].GetComponent<Image>().color = Color.white;
        currentKey = null;
        currentKind = StoreKind.None;
    }

    public void AddSliderValue(int plus)
    {
        if (slider.value + plus >= slider.maxValue)
        {
            slider.value = slider.maxValue;
            itemNumTxt.text = $"{slider.value}개";
        }
        else
        {
            slider.value += plus;
            itemNumTxt.text = $"{slider.value}개";
        }
    }

    //팝업 관련.
    public void NumAdjustment()
    {
        itemNumTxt.text = $"{slider.value}개";
    }

    public void Add10()
    {
        AddSliderValue(10);
    }

    public void Add100()
    {
        AddSliderValue(100);
    }

    public void AddMax()
    {
        AddSliderValue(Mathf.FloorToInt(slider.maxValue));
    }

    //창 관련.
    public void OpenPopup()
    {
        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);

        if (currentKind == StoreKind.Storage)
        {
            var image = storageObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }
        else if (currentKind == StoreKind.Store)
        {
            var image = storeObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentKind = StoreKind.None;
        currentKey = null;
    }
    //public GameObject storeContent;
    //public GameObject storeItemPrefab;

    //public GameObject itemNumWin;
    //public Text itemNum;
    //public Slider itemNumSlider;

    //public GameObject inventoryContent;
    //public GameObject inventoryItemPrefab;

    //Dictionary<int, string> storeItemData;
    //Dictionary<int, GameObject> storeList;

    //Dictionary<string, int> inventoryData;
    //Dictionary<string, GameObject> inventoryList;
    //int inventoryIndex;

    //string currentItem;

    //public void Init()
    //{
    //    storeItemData = new Dictionary<int, string>();
    //    storeItemData.Add(0, "빨간포션");
    //    storeItemData.Add(1, "주황포션");
    //    storeItemData.Add(2, "파란포션");
    //    storeItemData.Add(3, "하양포션");
    //    storeItemData.Add(4, "빨간알약");
    //    storeItemData.Add(5, "주황알약");
    //    storeItemData.Add(6, "파란알약");
    //    storeItemData.Add(7, "하양알약");

    //    storeList = new Dictionary<int, GameObject>();

    //    int i = 0;
    //    foreach (var element in storeItemData)
    //    {
    //        var go = Instantiate(storeItemPrefab, storeContent.transform);
    //        var child = go.transform.GetChild(0);
    //        var itemName = child.GetComponent<Text>();
    //        itemName.text = element.Value;

    //        var button = go.AddComponent<Button>();
    //        int num = i;
    //        button.onClick.AddListener(delegate { ShowItemNumWin(num); });
    //        storeList.Add(i, go);
    //        i++;
    //    }

    //    inventoryData = new Dictionary<string, int>();
    //    inventoryList = new Dictionary<string, GameObject>();
    //    inventoryIndex = 0;
    //}

    //public void ShowItemNumWin(int i)
    //{
    //    itemNumWin.SetActive(true);
    //    currentItem = storeItemData[i];
    //}

    //public void BuyItem()
    //{
    //    if (currentItem == null) return;
    //    if (Mathf.FloorToInt(itemNumSlider.value * 100) == 0) return;

    //    if (inventoryData.ContainsKey(currentItem))
    //    {
    //        int count = Mathf.FloorToInt(itemNumSlider.value * 100);
    //        inventoryData[currentItem] += count;

    //        var child = inventoryList[currentItem].transform.GetChild(1);
    //        child.GetComponent<Text>().text = inventoryData[currentItem].ToString();
    //    }

    //    else
    //    {
    //        int count = Mathf.FloorToInt(itemNumSlider.value * 100);
    //        inventoryData.Add(currentItem, count);

    //        var go = Instantiate(inventoryItemPrefab, inventoryContent.transform);
    //        var child = go.transform.GetChild(0);
    //        child.GetComponent<Text>().text = currentItem.Substring(0,1);

    //        child = go.transform.GetChild(1);
    //        child.GetComponent<Text>().text = inventoryData[currentItem].ToString();
    //        inventoryList.Add(currentItem, go);
    //    }
    //}

    //public void ChangeItemNum()
    //{
    //    itemNum.text = Mathf.FloorToInt(itemNumSlider.value * 100).ToString();
    //}
}