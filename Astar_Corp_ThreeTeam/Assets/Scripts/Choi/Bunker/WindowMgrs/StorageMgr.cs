using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageMgr : MonoBehaviour
{
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject storageWin;
    public GameObject upgradeWin;
    public Text storageWeightTxt;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

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
    public Image itemImg;
    public Text itemTypeTxt;
    public Text detailTxt;

    [Header("Upgrade Win")]
    public Text storageLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    public List<GameObject> buttons;

    Dictionary<string, GameObject> storageList = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> storageOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> storageOtherItemNumInfo = new Dictionary<string, int>();

    public PlayerDataMgr playerDataMgr;
    int storageLevel;
    int maxStorageCapacity;
    int nextStorageCapacity;
    int upgradeCost;

    Color originColor;
    string currentKey;
    InvenKind currentInvenKind;
    TrunkMode trunkMode;
    StorageMode storageMode;

    int storageCurrentWeight;
   
    public void Init()
    {
        ClosePopup();

        storageLevel = playerDataMgr.saveData.storageLevel;
        Bunker storageLevelInfo = playerDataMgr.bunkerList["BUN_0002"];
        switch (storageLevel)
        {
            case 1:
                maxStorageCapacity = storageLevelInfo.level1;
                nextStorageCapacity = storageLevelInfo.level2;
                upgradeCost = storageLevelInfo.level2Cost;
                break;
            case 2:
                maxStorageCapacity = storageLevelInfo.level2;
                nextStorageCapacity = storageLevelInfo.level3;
                upgradeCost = storageLevelInfo.level3Cost;
                break;
            case 3:
                maxStorageCapacity = storageLevelInfo.level3;
                nextStorageCapacity = storageLevelInfo.level4;
                upgradeCost = storageLevelInfo.level4Cost;
                break;
            case 4:
                maxStorageCapacity = storageLevelInfo.level4;
                nextStorageCapacity = storageLevelInfo.level5;
                upgradeCost = storageLevelInfo.level5Cost;
                break;
            case 5:
                maxStorageCapacity = storageLevelInfo.level5;
                break;
        }

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
        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void DisplayStorageItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        bunkerMgr.moneyTxt.text = $"{playerDataMgr.saveData.money}";

        //버튼 초기화.
        foreach (var element in buttons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = new Color(118f/255, 153f/255, 184f/255);
        }

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
        buttons[index].GetComponent<Image>().color = Color.red;

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
        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (storageMode != StorageMode.Consumables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(1).gameObject;
                var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{element.Value.weight * itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(element.Value.price *0.7f) * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

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
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{element.Value.weight * itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(element.Value.price * 0.7f) * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                
                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

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
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{int.Parse( element.Value.weight )* itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(int.Parse(element.Value.price) * 0.7f) * itemNum}";
               
                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = (element.Value.img == null)? null : element.Value.img;

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
        storageWeightTxt.text = $"{storageCurrentWeight}/{maxStorageCapacity}";

        storageBullet5Txt.text = $"{bullet5Num}";
        storageBullet7Txt.text = $"{bullet7Num}";
        storageBullet9Txt.text = $"{bullet9Num}";
        storageBullet45Txt.text = $"{bullet45Num}";
        storageBullet12Txt.text = $"{bullet12Num}";
    }

    public void SelectItem(string key, InvenKind kind)
    {
        bunkerMgr.PlayClickSound();

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
                itemNameTxt.text = storageWeaponInfo[currentKey].storeName;
                itemTypeTxt.text = $"{ GetTypeStr( storageWeaponInfo[currentKey].kind)}";
                itemImg.sprite = storageWeaponInfo[currentKey].img;
                detailTxt.text = $"{ storageWeaponInfo[currentKey].des}";
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].storeName;
                itemImg.sprite = storageConsumableInfo[currentKey].img;
                detailTxt.text = $"{storageConsumableInfo[currentKey].des}";
            }
            else if (storageOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageOtherItemInfo[currentKey].storeName;
                itemImg.sprite = (storageOtherItemInfo[currentKey].img == null)? null : storageOtherItemInfo[currentKey].img;
                //itemTypeTxt.text = $"{ storageOtherItemNumInfo[currentKey]}개";
                detailTxt.text = $"{storageOtherItemInfo[currentKey].des}";
            }
        }
       
        OpenPopup();
    }

    string GetTypeStr(string kind)
    {
        string type = string.Empty;
        switch (kind)
        {
            case "1":
                type = "Handgun";
                break;
            case "2":
                type = "SG";
                break;
            case "3":
                type = "SMG";
                break;
            case "4":
                type = "AR";
                break;
            case "5":
                type = "LMG";
                break;
            case "6":
                type = "SR";
                break;
            case "7":
                type = "근접무기";
                break;
        }
        return type;
    }

    public void RefreshUpgradeWin()
    {
        if (storageLevel != 5)
        {
            storageLevelTxt.text = $"건물 레벨 {storageLevel}→{storageLevel + 1}";
            capacityTxt.text = $"창고 최대 무게 {maxStorageCapacity}→{nextStorageCapacity}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            storageLevelTxt.text = $"건물 레벨{storageLevel}→ -";
            capacityTxt.text = $"창고 최대 무게 {maxStorageCapacity}→ -";
            materialTxt.text = $"-";
        }
    }

    //창 관련.
    public void OpenMainWin()
    {
        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (storageWin.activeSelf) storageWin.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenStorageWin()
    {
        mainWin.SetActive(false);
        storageWin.SetActive(true);
    }

    public void CloseStorageWin()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);
        storageWin.SetActive(false);
        mainWin.SetActive(true);
    }

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

    public void OpenUpgradeWin()
    {
        RefreshUpgradeWin();

        mainWin.SetActive(false);
        upgradeWin.SetActive(true);
    }

    public void CloseUpgradeWin()
    {
        upgradeWin.SetActive(false);
        mainWin.SetActive(true);
    }
}