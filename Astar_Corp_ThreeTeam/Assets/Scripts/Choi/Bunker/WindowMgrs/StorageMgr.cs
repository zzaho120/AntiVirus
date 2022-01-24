using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageMgr : MonoBehaviour
{
    public Text storageWeightTxt;

    [Header("총알 관련")]
    public Text storageBullet5Txt;
    public Text storageBullet7Txt;
    public Text storageBullet9Txt;
    public Text storageBullet45Txt;
    public Text storageBullet12Txt;

    public GameObject storageContent;
    public GameObject prefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemNumTxt;
    
    Dictionary<string, GameObject> storageList = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> storageOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> storageOtherItemNumInfo = new Dictionary<string, int>();

    public PlayerDataMgr playerDataMgr;
    Color originColor;
    string currentKey;
    InvenKind currentInvenKind;
    TrunkMode trunkMode;
    StorageMode storageMode;

    int storageCurrentWeight;
    int storageTotalWeight;

    public void Init()
    {
        ClosePopup();

        if (storageWeaponInfo.Count != 0) storageWeaponInfo.Clear();
        if (storageWeaponNumInfo.Count != 0) storageWeaponNumInfo.Clear();
        if (storageConsumableInfo.Count != 0) storageConsumableInfo.Clear();
        if (storageConsumableNumInfo.Count != 0) storageConsumableNumInfo.Clear();
        if (storageOtherItemInfo.Count != 0) storageOtherItemInfo.Clear();
        if (storageOtherItemNumInfo.Count != 0) storageOtherItemNumInfo.Clear();

        //창고.
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
            storageWeaponInfo.Add(element.Key, element.Value);
            storageWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
            storageConsumableInfo.Add(element.Key, element.Value);
            storageConsumableNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.currentOtherItems)
        {
            var itemNum = playerDataMgr.currentOtherItemsNum[element.Key];
            storageOtherItemInfo.Add(element.Key, element.Value);
            storageOtherItemNumInfo.Add(element.Key, itemNum);
        }

        DisplayStorageItem(0);
        originColor = prefab.GetComponent<Image>().color;
        currentKey = null;
        currentInvenKind = InvenKind.None;
    }

    public void DisplayStorageItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        switch (index)
        {
            case 0:
                storageMode = StorageMode.All;
                break;
            case 1:
                storageMode = StorageMode.Equippables;
                break;
            case 2:
                storageMode = StorageMode.Consumables;
                break;
            case 3:
                storageMode = StorageMode.OtherItems;
                break;
        }

        if (storageList.Count != 0)
        {
            foreach (var element in storageList)
            {
                Destroy(element.Value);
            }
            storageList.Clear();
            storageContent.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        storageCurrentWeight = 0;
        storageTotalWeight = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (storageMode != StorageMode.Consumables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(1).gameObject;
                var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                storageList.Add(element.Key, go);
            }
            storageCurrentWeight += (element.Value.weight * playerDataMgr.currentEquippablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(1).gameObject;
                var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                storageList.Add(element.Key, go);
            }
            storageCurrentWeight += (element.Value.weight * playerDataMgr.currentConsumablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentOtherItems)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.Consumables)
            {
                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(1).gameObject;
                var itemNum = playerDataMgr.currentOtherItemsNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                storageList.Add(element.Key, go);
            }
            storageCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.currentOtherItemsNum[element.Key]);

            switch (element.Key)
            {
                case "BUL_0004":
                    bullet5Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0005":
                    bullet7Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0002":
                    bullet9Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0003":
                    bullet45Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0001":
                    bullet12Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
            }
        }
        storageTotalWeight = 3000;
        //레벨에 따라 구현해야 함.
        storageWeightTxt.text = $"무게 {storageCurrentWeight}/{storageTotalWeight}";

        storageBullet5Txt.text = $"5탄x{bullet5Num.ToString("D3")}";
        storageBullet7Txt.text = $"7탄x{bullet7Num.ToString("D3")}";
        storageBullet9Txt.text = $"9탄x{bullet9Num.ToString("D3")}";
        storageBullet45Txt.text = $"45탄x{bullet45Num.ToString("D3")}";
        storageBullet12Txt.text = $"12게이지x{bullet12Num.ToString("D3")}";
    }

    public void SelectItem(string key, InvenKind kind)
    {
        if (currentKey != null)
        {
            if (currentInvenKind == InvenKind.Storage && storageList.ContainsKey(currentKey))
            {
                var image = storageList[currentKey].GetComponent<Image>();
                image.color = originColor;
            }
        }
        currentKey = key;
        currentInvenKind = kind;

        if (currentInvenKind == InvenKind.Storage)
        {
            var image = storageList[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (storageWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageWeaponInfo[currentKey].name;
                itemNumTxt.text = $"{ storageWeaponNumInfo[currentKey]}개";
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                itemNumTxt.text = $"{ storageConsumableNumInfo[currentKey]}개";
            }
            else if (storageOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageOtherItemInfo[currentKey].name;
                itemNumTxt.text = $"{ storageOtherItemNumInfo[currentKey]}개";
            }
        }
       
        OpenPopup();
    }

    //창 관련.
    public void OpenPopup()
    {
        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);

        if (currentInvenKind == InvenKind.Storage && storageList.ContainsKey(currentKey))
        {
            var image = storageList[currentKey].GetComponent<Image>();
            image.color = originColor;
        }

        currentInvenKind = InvenKind.None;
        currentKey = null;
    }
}