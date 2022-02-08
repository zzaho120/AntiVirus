using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public enum StoreMode
{
    All,
    Equippables,
    Consumables,
    OtherItems
}

public class StoreMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject storeWin;
    public GameObject upgradeWin;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    [Header("버튼 관련")]
    public List<GameObject> storageButtons;
    public List<GameObject> storeButtons;

    [Header("총알 관련")]
    public Text storageBullet5Txt;
    public Text storageBullet7Txt;
    public Text storageBullet9Txt;
    public Text storageBullet45Txt;
    public Text storageBullet12Txt;

    //public Text moneyTxt;
    public Text storageWeightTxt;
    public GameObject storeContents;
    public GameObject storePrefab;
    public GameObject storageContents;
    public GameObject storagePrefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text titleTxt;
    public Text itemNameTxt;
    public Image itemImg;
    public Text itemTypeTxt;
    public Text itemNumTxt;
    public Text itemCostTxt;
    public Text itemWeightTxt;
    public Slider slider;
    public GameObject buyButton;
    public GameObject sellButton;

    [Header("Upgrade Win")]
    public Text storeLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    public Dictionary<int, GameObject> storeObjs = new Dictionary<int, GameObject>();
    public Dictionary<string, GameObject> storageObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> storageOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> storageOtherItemNumInfo = new Dictionary<string, int>();

    Dictionary<int, string> storeWeaponInfo = new Dictionary<int, string>();
    Dictionary<int, int> storeWeaponNumInfo = new Dictionary<int, int>();
    Dictionary<int, string> storeConsumableInfo = new Dictionary<int, string>();
    Dictionary<int, int> storeConsumableNumInfo = new Dictionary<int, int>();
    Dictionary<int, string> storeOtherItemInfo = new Dictionary<int, string>();
    Dictionary<int, int> storeOtherItemNumInfo = new Dictionary<int, int>();

    int storeLevel;
    int maxItemNum;
    int nextItemNum;
    int upgradeCost;
    int storageLevel;
    int maxStorageCapacity;

    int currentStoreKey;
    string currentStorageKey;
    StoreMode storeMode;
    StorageMode storageMode;

    int storageCurrentWeight;
    
    int grade1TotalVal;
    int grade2TotalVal;
    int grade3TotalVal;

    public void Init()
    {
        storeLevel = playerDataMgr.saveData.storeLevel;
        Bunker storeLevelInfo = playerDataMgr.bunkerList["BUN_0006"];
        switch (storeLevel)
        {
            case 1:
                maxItemNum = storeLevelInfo.level1;
                nextItemNum = storeLevelInfo.level2;
                upgradeCost = storeLevelInfo.level2Cost;
                break;
            case 2:
                maxItemNum = storeLevelInfo.level2;
                nextItemNum = storeLevelInfo.level3;
                upgradeCost = storeLevelInfo.level3Cost;
                break;
            case 3:
                maxItemNum = storeLevelInfo.level3;
                nextItemNum = storeLevelInfo.level4;
                upgradeCost = storeLevelInfo.level4Cost;
                break;
            case 4:
                maxItemNum = storeLevelInfo.level4;
                nextItemNum = storeLevelInfo.level5;
                upgradeCost = storeLevelInfo.level5Cost;
                break;
            case 5:
                maxItemNum = storeLevelInfo.level5;
                break;
        }

        storageLevel = playerDataMgr.saveData.storageLevel;
        Bunker storageLevelInfo = playerDataMgr.bunkerList["BUN_0002"];
        switch (storageLevel)
        {
            case 1:
                maxStorageCapacity = storageLevelInfo.level1;
                break;
            case 2:
                maxStorageCapacity = storageLevelInfo.level2;
                break;
            case 3:
                maxStorageCapacity = storageLevelInfo.level3;
                break;
            case 4:
                maxStorageCapacity = storageLevelInfo.level4;
                break;
            case 5:
                maxStorageCapacity = storageLevelInfo.level5;
                break;
        }

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
        if (storageOtherItemInfo.Count != 0) storageOtherItemInfo.Clear();
        if (storageOtherItemNumInfo.Count != 0) storageOtherItemNumInfo.Clear();

        //생성.
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
        //moneyTxt.text = $"돈 : {playerDataMgr.saveData.money}";

        SelectStoreItem();
        DisplayStoreItem(0);
        DisplayStorageItem(0);

        currentStoreKey = -1;
        currentStorageKey = null;

        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        ClosePopup();
    }

    public void SelectStoreItem()
    {
        storeWeaponInfo.Clear();
        storeWeaponNumInfo.Clear();
        storeConsumableInfo.Clear();
        storeConsumableNumInfo.Clear();
        storeOtherItemInfo.Clear();
        storeOtherItemNumInfo.Clear();

        if (playerDataMgr.saveData.storeReset == true)
        {
            //벙커 알람.
            bunkerMgr.bunkerObjs[3].transform.GetChild(1).gameObject.SetActive(true);
            bunkerMgr.quickButtons[1].transform.GetChild(1).gameObject.SetActive(true);

            //랜덤 아이템 생성.
            playerDataMgr.saveData.storeReset = false;
            if (playerDataMgr.saveData.storeItem.Count != 0) playerDataMgr.saveData.storeItem.Clear();
            if (playerDataMgr.saveData.storeItemNum.Count != 0) playerDataMgr.saveData.storeItemNum.Clear();
       
            grade1TotalVal = 0;
            grade2TotalVal = 0;
            grade3TotalVal = 0;
            foreach (var element in playerDataMgr.equippableList)
            {
                switch (element.Value.grade)
                {
                    case 1:
                        grade1TotalVal += element.Value.value;
                        break;
                    case 2:
                        grade2TotalVal += element.Value.value;
                        break;
                    case 3:
                        grade3TotalVal += element.Value.value;
                        break;
                }
            }
            foreach (var element in playerDataMgr.consumableList)
            {
                switch (element.Value.grade)
                {
                    case 1:
                        grade1TotalVal += element.Value.value;
                        break;
                    case 2:
                        grade2TotalVal += element.Value.value;
                        break;
                    case 3:
                        grade3TotalVal += element.Value.value;
                        break;
                }
            }
            foreach (var element in playerDataMgr.otherItemList)
            {
                if (String.IsNullOrEmpty(element.Value.grade)) continue;
                var grade = int.Parse(element.Value.grade);
                switch (grade)
                {
                    case 1:
                        grade1TotalVal += 30;
                        break;
                    case 2:
                        grade2TotalVal += 30;
                        break;
                    case 3:
                        grade3TotalVal += 30;
                        break;
                }
            }

            for (int i = 0; i < maxItemNum; i++)
            {
                int sum = 0;
                int ranNum = 0;
                if (storeLevel == 1) ranNum = UnityEngine.Random.Range(1, grade1TotalVal + 1);
                else if (storeLevel == 2) ranNum = UnityEngine.Random.Range(1, grade1TotalVal + grade2TotalVal + 1);
                else ranNum = UnityEngine.Random.Range(1, grade1TotalVal +grade2TotalVal + grade3TotalVal + 1);

                foreach (var element in playerDataMgr.equippableList)
                {
                    if (storeLevel == 1 && element.Value.grade != 1) continue;
                    else if (storeLevel == 2 && element.Value.grade == 3) continue;
                    
                    var previous = sum;
                    sum += element.Value.value;
                    if (!(ranNum > previous && ranNum <= sum)) continue;

                    int num = i;
                    //json.
                    playerDataMgr.saveData.storeItem.Add(element.Key);
                    playerDataMgr.saveData.storeItemNum.Add(element.Value.itemQuantity);

                    //현재데이터.
                    storeWeaponInfo.Add(num, element.Key);
                    storeWeaponNumInfo.Add(num, element.Value.itemQuantity);
                }
                foreach (var element in playerDataMgr.consumableList)
                {
                    if (storeLevel == 1 && element.Value.grade != 1) continue;
                    else if (storeLevel == 2 && element.Value.grade == 3) continue;

                    var previous = sum;
                    sum += element.Value.value;
                    if (!(ranNum > previous && ranNum <= sum)) continue;

                    int num = i;
                    //json.
                    playerDataMgr.saveData.storeItem.Add(element.Key);
                    playerDataMgr.saveData.storeItemNum.Add(element.Value.itemQuantity);

                    //현재데이터.
                    storeConsumableInfo.Add(num, element.Key);
                    storeConsumableNumInfo.Add(num, element.Value.itemQuantity);
                }
                foreach (var element in playerDataMgr.otherItemList)
                {
                    if (String.IsNullOrEmpty(element.Value.grade)) continue;
                    var grade = int.Parse(element.Value.grade);
                    if (storeLevel == 1 && grade != 1) continue;
                    else if (storeLevel == 2 && grade == 3) continue;

                    var previous = sum;
                    sum += 30;
                    if (!(ranNum > previous && ranNum <= sum)) continue;

                    int num = i;
                    //json.
                    playerDataMgr.saveData.storeItem.Add(element.Key);
                    playerDataMgr.saveData.storeItemNum.Add(int.Parse(element.Value.itemQuantity));

                    //현재데이터.
                    storeOtherItemInfo.Add(num, element.Key);
                    storeOtherItemNumInfo.Add(num, int.Parse(element.Value.itemQuantity));
                }
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        }
        else
        {
            for (int i= 0; i < playerDataMgr.saveData.storeItem.Count; i++)
            {
                if (playerDataMgr.equippableList.ContainsKey(playerDataMgr.saveData.storeItem[i]))
                {
                    storeWeaponInfo.Add(i, playerDataMgr.saveData.storeItem[i]);
                    storeWeaponNumInfo.Add(i, playerDataMgr.saveData.storeItemNum[i]);
                }
                else if (playerDataMgr.consumableList.ContainsKey(playerDataMgr.saveData.storeItem[i]))
                {
                    storeConsumableInfo.Add(i, playerDataMgr.saveData.storeItem[i]);
                    storeConsumableNumInfo.Add(i, playerDataMgr.saveData.storeItemNum[i]);
                }
                else if (playerDataMgr.otherItemList.ContainsKey(playerDataMgr.saveData.storeItem[i]))
                {
                    storeOtherItemInfo.Add(i, playerDataMgr.saveData.storeItem[i]);
                    storeOtherItemNumInfo.Add(i, playerDataMgr.saveData.storeItemNum[i]);
                }
            }
        }
    }

    public void DisplayStoreItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        //버튼 초기화.
        foreach (var element in storeButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
        }

        switch (index)
        {
            case 0:
                storeMode = StoreMode.All;
                break;
            case 1:
                storeMode = StoreMode.Equippables;
                break;
            case 2:
                storeMode = StoreMode.Consumables;
                break;
            case 3:
                storeMode = StoreMode.OtherItems;
                break;
        }
        storeButtons[index].GetComponent<Image>().color = Color.red;

        if (storeObjs.Count != 0)
        {
            foreach (var element in storeObjs)
            {
                Destroy(element.Value);
            }
            storeObjs.Clear();
            storeContents.transform.DetachChildren();
        }

        if (storeMode != StoreMode.Consumables && storeMode != StoreMode.OtherItems)
        {
            foreach (var element in storeWeaponInfo)
            {
                var go = Instantiate(storePrefab, storeContents.transform);
                string key = element.Value;

                var name = playerDataMgr.equippableList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storeWeaponNumInfo[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.equippableList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{weight}";

                var price = playerDataMgr.equippableList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{price}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                int itemKey = element.Key;
                button.onClick.AddListener(delegate { SelectStoreItem(itemKey); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = playerDataMgr.equippableList[key].img;

                storeObjs.Add(element.Key, go);
            }
        }

        if (storeMode != StoreMode.Equippables && storeMode != StoreMode.OtherItems)
        {
            foreach (var element in storeConsumableInfo)
            {
                var go = Instantiate(storePrefab, storeContents.transform);
                string key = element.Value;

                var name = playerDataMgr.consumableList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storeConsumableNumInfo[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.consumableList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{weight}";

                var price = playerDataMgr.consumableList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{price}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                int itemKey = element.Key;
                button.onClick.AddListener(delegate { SelectStoreItem(itemKey); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = playerDataMgr.consumableList[key].img;

                storeObjs.Add(element.Key, go);
            }
        }

        if (storeMode != StoreMode.Equippables && storeMode != StoreMode.Consumables)
        {
            foreach (var element in storeOtherItemInfo)
            {
                var go = Instantiate(storePrefab, storeContents.transform);
                string key = element.Value;

                var name = playerDataMgr.otherItemList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storeOtherItemNumInfo[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.otherItemList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{weight}";

                var price = playerDataMgr.otherItemList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{price}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                int itemKey = element.Key;
                button.onClick.AddListener(delegate { SelectStoreItem(itemKey); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = playerDataMgr.otherItemList[key].img;

                storeObjs.Add(element.Key, go);
            }
        }
    }

    public void DisplayStorageItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        //버튼 초기화.
        foreach (var element in storageButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
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
        storageButtons[index].GetComponent<Image>().color = Color.red;

        if (storageObjs.Count != 0)
        {
            foreach (var element in storageObjs)
            {
                Destroy(element.Value);
            }
            storageObjs.Clear();
            storageContents.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        storageCurrentWeight = 0;
        foreach (var element in storageWeaponInfo)
        {
            if (storageMode != StorageMode.Consumables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                string key = element.Key;

                var name = playerDataMgr.equippableList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storageWeaponNumInfo[key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.equippableList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{weight * currentAmount}";

                var price = playerDataMgr.equippableList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt( price * 0.7f)*currentAmount }";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectStorageItem(key); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                storageObjs.Add(key, go);
            }
            storageCurrentWeight += (element.Value.weight * storageWeaponNumInfo[element.Key]);
        }

        foreach (var element in storageConsumableInfo)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                string key = element.Key;

                var name = playerDataMgr.consumableList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storageConsumableNumInfo[key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.consumableList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{weight * currentAmount}";

                var price = playerDataMgr.consumableList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt( price * 0.7f) * currentAmount}";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectStorageItem(key); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                storageObjs.Add(key, go);
            }
            storageCurrentWeight += (element.Value.weight * storageConsumableNumInfo[element.Key]);
        }

        foreach (var element in storageOtherItemInfo)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.Consumables)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                string key = element.Key;

                var name = playerDataMgr.otherItemList[key].name;
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{name}";

                var currentAmount = storageOtherItemNumInfo[key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{currentAmount}";

                var weight = playerDataMgr.otherItemList[key].weight;
                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{int.Parse( weight) * currentAmount}";

                var price = playerDataMgr.otherItemList[key].price;
                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(int.Parse( price)*0.7f)*currentAmount }";

                child = go.transform.GetChild(4).gameObject;
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectStorageItem(key); });

                child = go.transform.GetChild(5).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                storageObjs.Add(key, go);
            }
            storageCurrentWeight += (int.Parse(element.Value.weight) * storageOtherItemNumInfo[element.Key]);

            switch (element.Key)
            {
                case "BUL_0004":
                    bullet5Num += storageOtherItemNumInfo[element.Key];
                    break;
                case "BUL_0005":
                    bullet7Num += storageOtherItemNumInfo[element.Key];
                    break;
                case "BUL_0002":
                    bullet9Num += storageOtherItemNumInfo[element.Key];
                    break;
                case "BUL_0003":
                    bullet45Num += storageOtherItemNumInfo[element.Key];
                    break;
                case "BUL_0001":
                    bullet12Num += storageOtherItemNumInfo[element.Key];
                    break;
            }
        }
        storageWeightTxt.text = $"{storageCurrentWeight}/{maxStorageCapacity}";

        storageBullet5Txt.text = $"{bullet5Num}";
        storageBullet7Txt.text = $"{bullet7Num}";
        storageBullet9Txt.text = $"{bullet9Num}";
        storageBullet45Txt.text = $"{bullet45Num}";
        storageBullet12Txt.text = $"{bullet12Num}";

        bunkerMgr.bullet5Txt.text = $"{bullet5Num}";
        bunkerMgr.bullet7Txt.text = $"{bullet7Num}";
        bunkerMgr.bullet9Txt.text = $"{bullet9Num}";
        bunkerMgr.bullet45Txt.text = $"{bullet45Num}";
        bunkerMgr.bullet12Txt.text = $"{bullet12Num}";
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
    }

    public void SelectStoreItem(int key)
    {
        Image image;
        if (currentStoreKey != -1 && storeObjs.ContainsKey(currentStoreKey))
        {
            image = storeObjs[currentStoreKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentStoreKey = key;

        image = storeObjs[currentStoreKey].GetComponent<Image>();
        image.color = Color.red;

        if (storeWeaponInfo.ContainsKey(currentStoreKey))
        {
            var id = storeWeaponInfo[currentStoreKey];
            var name = playerDataMgr.equippableList[id].name;
            itemNameTxt.text = name;
            itemImg.sprite = playerDataMgr.equippableList[id].img;
            itemTypeTxt.text = "전투";
            slider.maxValue = storeWeaponNumInfo[currentStoreKey];
        }
        else if (storeConsumableInfo.ContainsKey(currentStoreKey))
        {
            var id = storeConsumableInfo[currentStoreKey];
            var name = playerDataMgr.consumableList[id].name;
            itemNameTxt.text = name;
            itemImg.sprite = playerDataMgr.consumableList[id].img;
            itemTypeTxt.text = "소모";
            slider.maxValue = storeConsumableNumInfo[currentStoreKey];
        }
        else if (storeOtherItemInfo.ContainsKey(currentStoreKey))
        {
            var id = storeOtherItemInfo[currentStoreKey];
            var name = playerDataMgr.otherItemList[id].name;
            itemNameTxt.text = name;
            itemImg.sprite = (playerDataMgr.otherItemList[id].img!=null)?
                playerDataMgr.otherItemList[id].img : null;
            itemTypeTxt.text = "기타";
            slider.maxValue = storeOtherItemNumInfo[currentStoreKey];
        }

        //팝업 관련.
        //titleTxt.text = "구매";
        if (sellButton.activeSelf) sellButton.SetActive(false);
        if (!buyButton.activeSelf) buyButton.SetActive(true);

        slider.value = 0;
        itemNumTxt.text = $"(선택 개수) 0개";
        itemCostTxt.text = "0";
        itemWeightTxt.text = "0";
        OpenPopup();
    }

    public void SelectStorageItem(string key)
    {
        Image image;
        if (currentStorageKey != null && storageObjs.ContainsKey(currentStorageKey))
        {
            image = storageObjs[currentStorageKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentStorageKey = key;
        
        image = storageObjs[currentStorageKey].GetComponent<Image>();
        image.color = Color.red;

        if (storageWeaponInfo.ContainsKey(currentStorageKey))
        {
            itemNameTxt.text = storageWeaponInfo[currentStorageKey].name;
            slider.maxValue = storageWeaponNumInfo[currentStorageKey];
        }
        else if (storageConsumableInfo.ContainsKey(currentStorageKey))
        {
            itemNameTxt.text = storageConsumableInfo[currentStorageKey].name;
            slider.maxValue = storageConsumableNumInfo[currentStorageKey];
        }
        else if (storageOtherItemInfo.ContainsKey(currentStorageKey))
        {
            itemNameTxt.text = storageOtherItemInfo[currentStorageKey].name;
            slider.maxValue = storageOtherItemNumInfo[currentStorageKey];
        }

        //팝업 관련.
        titleTxt.text = "판매";
        if (buyButton.activeSelf) buyButton.SetActive(false);
        if (!sellButton.activeSelf) sellButton.SetActive(true);

        slider.value = 0;
        itemNumTxt.text = $"(선택 개수) 0개";
        itemCostTxt.text = "0";
        OpenPopup();
    }

    public void Operation()
    {
        if (currentStorageKey != null)
            Sell(Mathf.FloorToInt(slider.value));
        else if (currentStoreKey != -1)
            Buy(Mathf.FloorToInt(slider.value));

        ClosePopup();
    }

    public void Buy(int itemNum)
    {
        if (currentStoreKey == -1) return;
        if (itemNum == 0) return;

        if (storeWeaponInfo.ContainsKey(currentStoreKey))
        {
            var currentKey = storeWeaponInfo[currentStoreKey];
            var cost = playerDataMgr.equippableList[currentKey].price;
            if (playerDataMgr.saveData.money - cost < 0) return;

            var weight = playerDataMgr.equippableList[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            if (storageWeaponInfo.ContainsKey(storeWeaponInfo[currentStoreKey]))
            {
                //json.
                var id = storageWeaponInfo[currentKey].id;
                var index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] += itemNum;

                //playerDataMgr.
                playerDataMgr.currentEquippablesNum[currentKey] += itemNum;

                //현재데이터.
                storageWeaponNumInfo[currentKey] += itemNum;
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
            }

            playerDataMgr.saveData.money -= cost;
        }
        else if (storeConsumableInfo.ContainsKey(currentStoreKey))
        {
            var currentKey = storeConsumableInfo[currentStoreKey];
            var cost = playerDataMgr.consumableList[currentKey].price;
            if (playerDataMgr.saveData.money - cost < 0) return;

            var weight = playerDataMgr.consumableList[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

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
            }
            else
            {
                //json.
                var id = playerDataMgr.consumableList[currentKey].id;
                playerDataMgr.saveData.consumableList.Add(id);
                playerDataMgr.saveData.consumableNumList.Add(itemNum);

                //playerDataMgr.
                var consumable = playerDataMgr.consumableList[currentKey];
                playerDataMgr.currentConsumables.Add(currentKey, consumable);
                playerDataMgr.currentConsumablesNum.Add(currentKey, itemNum);

                //현재데이터.
                storageConsumableInfo.Add(currentKey, consumable);
                storageConsumableNumInfo.Add(currentKey, itemNum);
            }

            playerDataMgr.saveData.money -= cost;
        }
        else if (storeOtherItemInfo.ContainsKey(currentStoreKey))
        {
            var currentKey = storeOtherItemInfo[currentStoreKey];
            var cost = int.Parse(playerDataMgr.otherItemList[currentKey].price);
            if (playerDataMgr.saveData.money - cost < 0) return;

            var weight = int.Parse(playerDataMgr.otherItemList[currentKey].weight);
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            if (storageOtherItemInfo.ContainsKey(currentKey))
            {
                //json.
                var id = storageOtherItemInfo[currentKey].id;
                var index = playerDataMgr.saveData.otherItemList.IndexOf(id);
                playerDataMgr.saveData.otherItemNumList[index] += itemNum;

                //playerDataMgr.
                playerDataMgr.currentOtherItemsNum[currentKey] += itemNum;

                //현재데이터.
                storageOtherItemNumInfo[currentKey] += itemNum;
            }
            else
            {
                //json.
                var id = playerDataMgr.otherItemList[currentKey].id;
                playerDataMgr.saveData.otherItemList.Add(id);
                playerDataMgr.saveData.otherItemNumList.Add(itemNum);

                //playerDataMgr.
                var otherItem = playerDataMgr.otherItemList[currentKey];
                playerDataMgr.currentOtherItems.Add(currentKey, otherItem);
                playerDataMgr.currentOtherItemsNum.Add(currentKey, itemNum);

                //현재데이터.
                storageOtherItemInfo.Add(currentKey, otherItem);
                storageOtherItemNumInfo.Add(currentKey, itemNum);
            }

            playerDataMgr.saveData.money -= cost;
        }

        if (playerDataMgr.saveData.storeItemNum[currentStoreKey] - itemNum == 0)
        {
            //json.
            playerDataMgr.saveData.storeItem.RemoveAt(currentStoreKey);
            playerDataMgr.saveData.storeItemNum.RemoveAt(currentStoreKey);

            //현재데이터.
            SelectStoreItem();
        }
        else
        {
            //json.
            playerDataMgr.saveData.storeItemNum[currentStoreKey] -= itemNum;

            //현재 데이터.
            if(storeWeaponNumInfo.ContainsKey(currentStoreKey)) storeWeaponNumInfo[currentStoreKey] -= itemNum;
            else if (storeConsumableNumInfo.ContainsKey(currentStoreKey)) storeConsumableNumInfo[currentStoreKey] -= itemNum;
            else if (storeOtherItemNumInfo.ContainsKey(currentStoreKey)) storeOtherItemNumInfo[currentStoreKey] -= itemNum;
        }

        var currentMode = (int)storageMode;
        DisplayStorageItem(currentMode);
        currentMode = (int)storeMode;
        DisplayStoreItem(currentMode);

        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        currentStoreKey = -1;
    }

    public void Sell(int itemNum)
    {
        if (currentStorageKey == null) return;
        if (itemNum == 0) return;

        int cost = 0;
        if (storageWeaponInfo.ContainsKey(currentStorageKey))
        {
            var currentKey = currentStorageKey;
            cost = Mathf.FloorToInt (playerDataMgr.equippableList[currentKey].price *0.7f);
            
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
            }
        }
        else if (storageConsumableInfo.ContainsKey(currentStorageKey))
        {
            var currentKey = currentStorageKey;
            cost = Mathf.FloorToInt(playerDataMgr.consumableList[currentKey].price * 0.7f);

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
            }
        }
        else if (storageOtherItemInfo.ContainsKey(currentStorageKey))
        {
            var currentKey = currentStorageKey;
            cost = Mathf.FloorToInt(int.Parse(playerDataMgr.otherItemList[currentKey].price) * 0.7f);

            if (storageOtherItemNumInfo[currentKey] - itemNum != 0)
            {
                //json.
                var id = storageOtherItemInfo[currentKey].id;
                var index = playerDataMgr.saveData.otherItemList.IndexOf(id);
                playerDataMgr.saveData.otherItemNumList[index] -= itemNum;

                //playerDataMgr.
                playerDataMgr.currentOtherItemsNum[currentKey] -= itemNum;

                //현재데이터.
                storageOtherItemNumInfo[currentKey] -= itemNum;
            }
            else
            {
                //json.
                var id = playerDataMgr.otherItemList[currentKey].id;
                var index = playerDataMgr.saveData.otherItemList.IndexOf(id);
                playerDataMgr.saveData.otherItemList.Remove(currentKey);
                playerDataMgr.saveData.otherItemNumList.RemoveAt(index);

                //playerDataMgr.
                playerDataMgr.currentOtherItems.Remove(currentKey);
                playerDataMgr.currentOtherItemsNum.Remove(currentKey);

                //현재데이터.
                storageOtherItemInfo.Remove(currentKey);
                storageOtherItemNumInfo.Remove(currentKey);
            }
        }
        var currentMode = (int)storageMode;
        DisplayStorageItem(currentMode);

        playerDataMgr.saveData.money += cost;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        currentStorageKey = null;
    }

    public void AddSliderValue(int plus)
    {
        if (slider.value + plus >= slider.maxValue)
        {
            slider.value = slider.maxValue;
            itemNumTxt.text = $"(선택 개수) {slider.value}개";
        }
        else if (slider.value + plus < slider.minValue)
        {
            slider.value = slider.minValue;
            itemNumTxt.text = $"(선택 개수) {slider.value}개";
        }
        else
        {
            slider.value += plus;
            itemNumTxt.text = $"(선택 개수) {slider.value}개";
        }
    }

    //팝업 관련.
    public void NumAdjustment()
    {
        itemNumTxt.text = $"(선택 개수) {slider.value}개";

        if (currentStoreKey != -1)
        {
            if (storeWeaponInfo.ContainsKey(currentStoreKey))
            {
                var key = storeWeaponInfo[currentStoreKey];
                var cost = playerDataMgr.equippableList[key].price;
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = playerDataMgr.equippableList[key].weight;
                itemWeightTxt.text = $"{weight * slider.value}";
            }
            else if (storeConsumableInfo.ContainsKey(currentStoreKey))
            {
                var key = storeConsumableInfo[currentStoreKey];
                var cost = playerDataMgr.consumableList[key].price;
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = playerDataMgr.consumableList[key].weight;
                itemWeightTxt.text = $"{weight * slider.value}";
            }
            else if (storeOtherItemInfo.ContainsKey(currentStoreKey))
            {
                var key = storeOtherItemInfo[currentStoreKey];
                var cost = int.Parse(playerDataMgr.otherItemList[key].price);
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = int.Parse(playerDataMgr.otherItemList[key].weight);
                itemWeightTxt.text = $"{weight * slider.value}";
            }
        }
        else if (currentStorageKey != null)
        {
            if (storageWeaponInfo.ContainsKey(currentStorageKey))
            {
                var cost = Mathf.FloorToInt(playerDataMgr.equippableList[currentStorageKey].price * 0.7f);
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = playerDataMgr.equippableList[currentStorageKey].weight;
                itemWeightTxt.text = $"{weight * slider.value}";
            }
            else if (storageConsumableInfo.ContainsKey(currentStorageKey))
            {
                var cost = Mathf.FloorToInt(playerDataMgr.consumableList[currentStorageKey].price * 0.7f);
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = playerDataMgr.consumableList[currentStorageKey].weight;
                itemWeightTxt.text = $"{weight * slider.value}";
            }
            else if (storageOtherItemInfo.ContainsKey(currentStorageKey))
            {
                var cost = Mathf.FloorToInt(int.Parse(playerDataMgr.otherItemList[currentStorageKey].price) * 0.7f);
                itemCostTxt.text = $"{cost * slider.value}";
                var weight = int.Parse(playerDataMgr.otherItemList[currentStorageKey].weight);
                itemWeightTxt.text = $"{weight * slider.value}";
            }
        }
    }
    public void Add1()
    {
        AddSliderValue(1);
    }

    public void Add10()
    {
        AddSliderValue(10);
    }

    public void Add50()
    {
        AddSliderValue(50);
    }

    public void Minus1()
    {
        AddSliderValue(-1);
    }

    public void Minus10()
    {
        AddSliderValue(-10);
    }

    public void Minus50()
    {
        AddSliderValue(-50);
    }

    public void AddMax()
    {
        AddSliderValue(Mathf.FloorToInt(slider.maxValue));
    }

    public void RefreshUpgradeWin()
    {
        if (storeLevel != 5)
        {
            storeLevelTxt.text = $"건물 레벨 {storeLevel}→{storeLevel + 1}";
            capacityTxt.text = $"등장 품목 수량 {maxItemNum}→{nextItemNum}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            storeLevelTxt.text = $"건물 레벨{storeLevel}→ -";
            capacityTxt.text = $"등장 품목 수량 {maxItemNum}→ -";
            materialTxt.text = $"-";
        }
    }

    //창 관련.
    public void OpenMainWin()
    {
        //벙커 알람.
        if (bunkerMgr.bunkerObjs[3].transform.GetChild(1).gameObject.activeSelf)
        {
            bunkerMgr.bunkerObjs[3].transform.GetChild(1).gameObject.SetActive(false);
            bunkerMgr.quickButtons[1].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (storeWin.activeSelf) storeWin.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
        if (!bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(true);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenStoreWin()
    {
        mainWin.SetActive(false);
        storeWin.SetActive(true);
    }

    public void CloseStoreWin()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);
        storeWin.SetActive(false);
        mainWin.SetActive(true);
    }


    public void OpenPopup()
    {
        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);

        if (currentStorageKey != null)
        {
            var image = storageObjs[currentStorageKey].GetComponent<Image>();
            image.color = Color.white;
        }
        else if (currentStoreKey != -1)
        {
            var image = storeObjs[currentStoreKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentStorageKey = null;
        currentStoreKey = -1;
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