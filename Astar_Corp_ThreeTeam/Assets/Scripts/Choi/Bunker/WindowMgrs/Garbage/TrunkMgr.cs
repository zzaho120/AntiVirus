using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TrunkMode
{
    All,
    Equippables,
    Consumables,
    OtherItems
}

public enum InvenKind
{ 
    None,
    Truck,
    Storage
}

public class TrunkMgr : MonoBehaviour
{
    public Text trunkWeightTxt;
    public Text storageWeightTxt;

    [Header("버튼 관련")]
    public List<GameObject> trunkButtons;
    public List<GameObject> storageButtons;

    [Header("총알 관련")]
    public Text trunkBullet5Txt;
    public Text trunkBullet7Txt;
    public Text trunkBullet9Txt;
    public Text trunkBullet45Txt;
    public Text trunkBullet12Txt;

    public Text storageBullet5Txt;
    public Text storageBullet7Txt;
    public Text storageBullet9Txt;
    public Text storageBullet45Txt;
    public Text storageBullet12Txt;

    public GameObject trunkContent;
    public GameObject trunkPrefab;
    public GameObject storageContent;
    public GameObject storagePrefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemTypeTxt;
    public Image itemImg;
    public Text itemNumTxt;
    public Slider slider;

    public string trunkWeight;
    public int trunkBullet5;
    public int trunkBullet7;
    public int trunkBullet9;
    public int trunkBullet45;
    public int trunkBullet12;

    Dictionary<string, GameObject> truckList = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> storageList = new Dictionary<string,  GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> storageOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> storageOtherItemNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();

    public PlayerDataMgr playerDataMgr;
    public GarageMgr garageMgr;

    int storageLevel;
    int maxStorageCapacity;

    Color originColor;
    string currentKey;
    InvenKind currentInvenKind;
    TrunkMode trunkMode;
    StorageMode storageMode;

    int trunkCurrentWeight;
    int trunkTotalWeight;
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

        if (storageWeaponInfo.Count != 0) storageWeaponInfo.Clear();
        if (storageWeaponNumInfo.Count != 0) storageWeaponNumInfo.Clear();
        if (storageConsumableInfo.Count != 0) storageConsumableInfo.Clear();
        if (storageConsumableNumInfo.Count != 0) storageConsumableNumInfo.Clear();
        if (storageOtherItemInfo.Count != 0) storageOtherItemInfo.Clear();
        if (storageOtherItemNumInfo.Count != 0) storageOtherItemNumInfo.Clear();

        if (truckWeaponInfo.Count != 0) truckWeaponInfo.Clear();
        if (truckWeaponNumInfo.Count != 0) truckWeaponNumInfo.Clear();
        if (truckConsumableInfo.Count != 0) truckConsumableInfo.Clear();
        if (truckConsumableNumInfo.Count != 0) truckConsumableNumInfo.Clear();
        if (truckOtherItemInfo.Count != 0) truckOtherItemInfo.Clear();
        if (truckOtherItemNumInfo.Count != 0) truckOtherItemNumInfo.Clear();

        //트렁크.
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var itemNum = playerDataMgr.truckEquippablesNum[element.Key];
            truckWeaponInfo.Add(element.Key, element.Value);
            truckWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            var itemNum = playerDataMgr.truckConsumablesNum[element.Key];
            truckConsumableInfo.Add(element.Key, element.Value);
            truckConsumableNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.truckOtherItems)
        {
            var itemNum = playerDataMgr.truckOtherItemsNum[element.Key];
            truckOtherItemInfo.Add(element.Key, element.Value);
            truckOtherItemNumInfo.Add(element.Key, itemNum);
        }

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

        if(playerDataMgr.saveData.currentCar == null) playerDataMgr.saveData.currentCar = "TRU_0004";
        DisplayTruckItem(0);
        DisplayStorageItem(0);
        originColor = trunkPrefab.GetComponent<Image>().color;
        currentKey = null;
        currentInvenKind = InvenKind.None;
    }

    public void DisplayTruckItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        //버튼 초기화.
        foreach (var element in trunkButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = new Color(118f/255, 153f/255, 184f/255);
        }

        switch (index)
        {
            case 0:
                trunkMode = TrunkMode.All;
                break;
            case 1:
                trunkMode = TrunkMode.Equippables;
                break;
            case 2:
                trunkMode = TrunkMode.Consumables;
                break;
            case 3:
                trunkMode = TrunkMode.OtherItems;
                break;
        }
        trunkButtons[index].GetComponent<Image>().color = Color.red;

        if (truckList.Count != 0)
        {
            foreach (var element in truckList)
            {
                Destroy(element.Value);
            }
            truckList.Clear();
            trunkContent.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        trunkCurrentWeight = 0;
        trunkTotalWeight = 0;
        foreach (var element in playerDataMgr.truckEquippables)
        {
            if (trunkMode != TrunkMode.Consumables && trunkMode != TrunkMode.OtherItems)
            {
                var go = Instantiate(trunkPrefab, trunkContent.transform);
                
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.truckEquippablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(3).gameObject;
                var weight = element.Value.weight;
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = element.Value.price;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
                truckList.Add(element.Key, go);
            }
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            if (trunkMode != TrunkMode.Equippables && trunkMode != TrunkMode.OtherItems)
            {
                var go = Instantiate(trunkPrefab, trunkContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.truckConsumablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(3).gameObject;
                var weight = element.Value.weight;
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = element.Value.price;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
                truckList.Add(element.Key, go);
            }
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.truckOtherItems)
        {
            if (trunkMode != TrunkMode.Equippables && trunkMode != TrunkMode.Consumables)
            {
                var go = Instantiate(trunkPrefab, trunkContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = (element.Value.img == null)? null : element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.truckOtherItemsNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(3).gameObject;
                var weight = int.Parse( element.Value.weight );
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = int.Parse(element.Value.price);
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
                truckList.Add(element.Key, go);
            }
            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);

            switch (element.Key)
            {
                case "BUL_0004":
                    bullet5Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0005":
                    bullet7Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0002":
                    bullet9Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0003":
                    bullet45Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0001":
                    bullet12Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
            }
        }

        var key = playerDataMgr.saveData.currentCar;
        Debug.Log($"{key}");
        trunkTotalWeight = playerDataMgr.truckList[key].weight;
        trunkWeightTxt.text = $"{trunkCurrentWeight}/{trunkTotalWeight}";
        trunkWeight = trunkWeightTxt.text;

        trunkBullet5Txt.text = $"{bullet5Num}";
        trunkBullet7Txt.text = $"{bullet7Num}";
        trunkBullet9Txt.text = $"{bullet9Num}";
        trunkBullet45Txt.text = $"{bullet45Num}";
        trunkBullet12Txt.text = $"{bullet12Num}";
        trunkBullet5 = bullet5Num;
        trunkBullet7 = bullet7Num;
        trunkBullet9 = bullet9Num;
        trunkBullet45 = bullet45Num;
        trunkBullet12 = bullet12Num;
        garageMgr.UpdateInfo();

        if (trunkCurrentWeight > trunkTotalWeight)
        {
            trunkWeightTxt.color = Color.red;
            playerDataMgr.ableToExit = false;
        }
        else
        {
            if(trunkWeightTxt.color == Color.red) trunkWeightTxt.color = Color.black;
            playerDataMgr.ableToExit = true;
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
                element.GetComponent<Image>().color = new Color(118f / 255, 153f / 255, 184f / 255); ;
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
                var go = Instantiate(storagePrefab, storageContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(3).gameObject;
                var weight = element.Value.weight;
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = element.Value.price;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                storageList.Add(element.Key, go);
            }
            storageCurrentWeight += (element.Value.weight * playerDataMgr.currentEquippablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(storagePrefab, storageContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(3).gameObject;
                var weight = element.Value.weight;
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = element.Value.price;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
                storageList.Add(element.Key, go);
            }
            storageCurrentWeight += (element.Value.weight * playerDataMgr.currentConsumablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentOtherItems)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.Consumables)
            {
                var go = Instantiate(storagePrefab, storageContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = (element.Value.img == null) ? null : element.Value.img;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                child = go.transform.GetChild(2).gameObject;
                var itemNum = playerDataMgr.currentOtherItemsNum[element.Key];
                child.GetComponent<Text>().text = $"{itemNum}";


                child = go.transform.GetChild(3).gameObject;
                var weight = int.Parse(element.Value.weight);
                child.GetComponent<Text>().text = $"{weight * itemNum}";

                child = go.transform.GetChild(4).gameObject;
                var price = int.Parse( element.Value.price );
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(price * 0.7f) * itemNum}";

                child = go.transform.GetChild(5).gameObject;
                var button = child.AddComponent<Button>();
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
        storageWeightTxt.text = $"{storageCurrentWeight}/{maxStorageCapacity}";

        storageBullet5Txt.text = $"{bullet5Num}";
        storageBullet7Txt.text = $"{bullet7Num}";
        storageBullet9Txt.text = $"{bullet9Num}";
        storageBullet45Txt.text = $"{bullet45Num}";
        storageBullet12Txt.text = $"{bullet12Num}";

        garageMgr.bunkerMgr.bullet5Txt.text = $"{bullet5Num}";
        garageMgr.bunkerMgr.bullet7Txt.text = $"{bullet7Num}";
        garageMgr.bunkerMgr.bullet9Txt.text = $"{bullet9Num}";
        garageMgr.bunkerMgr.bullet45Txt.text = $"{bullet45Num}";
        garageMgr.bunkerMgr.bullet12Txt.text = $"{bullet12Num}";
    }

    public void NumAdjustment()
    {
       itemNumTxt.text = $"(선택 개수) {slider.value}개";
    }

    public void SelectItem(string key, InvenKind kind)
    {
        garageMgr.bunkerMgr.PlayClickSound();

        if (currentKey != null)
        {
            if (currentInvenKind == InvenKind.Storage && storageList.ContainsKey(currentKey))
            {
                var image = storageList[currentKey].GetComponent<Image>();
                image.color = originColor;
            }
            else if(currentInvenKind == InvenKind.Truck && truckList.ContainsKey(currentKey))
            {
                var image = truckList[currentKey].GetComponent<Image>();
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
                itemTypeTxt.text = "전투";
                itemImg.sprite = storageWeaponInfo[currentKey].img;
                slider.maxValue = storageWeaponNumInfo[currentKey];
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                itemTypeTxt.text = "소모";
                itemImg.sprite = storageConsumableInfo[currentKey].img;
                slider.maxValue = storageConsumableNumInfo[currentKey];
            }
            else if (storageOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageOtherItemInfo[currentKey].name;
                itemTypeTxt.text = "기타";
                itemImg.sprite = (storageOtherItemInfo[currentKey].img == null)? null : storageOtherItemInfo[currentKey].img;
                slider.maxValue = storageOtherItemNumInfo[currentKey];
            }
        }
        else if (currentInvenKind == InvenKind.Truck)
        {
            var image = truckList[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (truckWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckWeaponInfo[currentKey].name;
                itemTypeTxt.text = "전투";
                itemImg.sprite = truckWeaponInfo[currentKey].img;
                slider.maxValue = truckWeaponNumInfo[currentKey];
            }
            else if (truckConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckConsumableInfo[currentKey].name;
                itemTypeTxt.text = "소모";
                itemImg.sprite = truckConsumableInfo[currentKey].img;
                slider.maxValue = truckConsumableNumInfo[currentKey];
            }
            else if (truckOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckOtherItemInfo[currentKey].name;
                itemTypeTxt.text = "기타";
                itemImg.sprite = (truckOtherItemInfo[currentKey].img == null)? null : truckOtherItemInfo[currentKey].img;
                slider.maxValue = truckOtherItemNumInfo[currentKey];
            }
        }

        slider.value = 0;
        itemNumTxt.text = $"(선택 개수) 0개";
        OpenPopup();
    }

    public void Move()
    {
        if (slider.value == 0) return;
        if (currentInvenKind == InvenKind.Storage)
            MoveToTrunk(Mathf.FloorToInt(slider.value));
        else if(currentInvenKind == InvenKind.Truck)
            MoveToStorage(Mathf.FloorToInt(slider.value));

        ClosePopup();
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

    public void Add1()
    {
        AddSliderValue(1);
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

    public void Minus1()
    {
        AddSliderValue(-1);
    }

    public void MoveToTrunk(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (storageWeaponInfo.ContainsKey(currentKey))
        {
            var weight = playerDataMgr.equippableList[currentKey].weight;
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

            //json.
            int index=0;
            var id = storageWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckEquippableList.Contains(id))
            {
                playerDataMgr.saveData.truckEquippableList.Add(id);
                playerDataMgr.saveData.truckEquippableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
                playerDataMgr.saveData.truckEquippableNumList[index]+= itemNum;
            }

            index = playerDataMgr.saveData.equippableList.IndexOf(id);
            if (playerDataMgr.saveData.equippableNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.equippableList.Remove(id);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.equippableNumList[index]-=itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                truckWeaponInfo.Add(currentKey, storageWeaponInfo[currentKey]);
                truckWeaponNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[currentKey]+=itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id]+=itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (playerDataMgr.currentEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                storageWeaponNumInfo[currentKey]-= itemNum;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id]-=itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);
        }
        else if (storageConsumableInfo.ContainsKey(currentKey))
        {
            var weight = playerDataMgr.consumableList[currentKey].weight;
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

            //json.
            int index;
            var id = storageConsumableInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckConsumableList.Contains(id))
            {
                playerDataMgr.saveData.truckConsumableList.Add(id);
                playerDataMgr.saveData.truckConsumableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
                playerDataMgr.saveData.truckConsumableNumList[index]+=itemNum;
            }

            index = playerDataMgr.saveData.consumableList.IndexOf(id);
            if (playerDataMgr.saveData.consumableNumList[index] -itemNum== 0)
            {
                playerDataMgr.saveData.consumableList.Remove(id);
                playerDataMgr.saveData.consumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.consumableNumList[index]-=itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckConsumables.ContainsKey(id))
            {
                //현재데이터 관련.
                truckConsumableInfo.Add(currentKey, storageConsumableInfo[currentKey]);
                truckConsumableNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckConsumableNumInfo[currentKey]+=itemNum;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id]+=itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (playerDataMgr.currentConsumablesNum[id] -itemNum== 0)
            {
                //현재 데이터.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Remove(id);
                playerDataMgr.currentConsumablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                storageConsumableNumInfo[currentKey]-=itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id]-=itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);
        }
        else if (storageOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(playerDataMgr.otherItemList[currentKey].weight);
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

            //json.
            int index;
            var id = storageOtherItemInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckOtherItemList.Contains(id))
            {
                playerDataMgr.saveData.truckOtherItemList.Add(id);
                playerDataMgr.saveData.truckOtherItemNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckOtherItemList.IndexOf(id);
                playerDataMgr.saveData.truckOtherItemNumList[index] += itemNum;
            }

            index = playerDataMgr.saveData.otherItemList.IndexOf(id);
            if (playerDataMgr.saveData.otherItemNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.otherItemList.Remove(id);
                playerDataMgr.saveData.otherItemNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.otherItemNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckOtherItems.ContainsKey(id))
            {
                //현재데이터 관련.
                truckOtherItemInfo.Add(currentKey, storageOtherItemInfo[currentKey]);
                truckOtherItemNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItems.Add(id, playerDataMgr.otherItemList[id]);
                playerDataMgr.truckOtherItemsNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckOtherItemNumInfo[currentKey] += itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItemsNum[id] += itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (playerDataMgr.currentOtherItemsNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageOtherItemInfo.Remove(currentKey);
                storageOtherItemNumInfo.Remove(currentKey);
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItems.Remove(id);
                playerDataMgr.currentOtherItemsNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                storageOtherItemNumInfo[currentKey] -= itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItemsNum[id] -= itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);
        }
    }

    public void MoveToStorage(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (truckWeaponInfo.ContainsKey(currentKey))
        {
            var weight = playerDataMgr.equippableList[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index=0;
            var id = truckWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.equippableList.Contains(id))
            {
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index]+=itemNum;
            }
            
            index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
            if (playerDataMgr.saveData.truckEquippableNumList[index] -itemNum== 0)
            {
                playerDataMgr.saveData.truckEquippableList.Remove(id);
                playerDataMgr.saveData.truckEquippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckEquippableNumList[index]-=itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                storageWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
                storageWeaponNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageWeaponNumInfo[currentKey]+=itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id]+=itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);

            if (playerDataMgr.truckEquippablesNum[id] -itemNum== 0)
            {
                //현재 데이터.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                truckWeaponNumInfo[currentKey]-=itemNum;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id]-=itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
        else if (truckConsumableInfo.ContainsKey(currentKey))
        {
            var weight = playerDataMgr.consumableList[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index;
            var id = truckConsumableInfo[currentKey].id;
            if (!playerDataMgr.saveData.consumableList.Contains(id))
            {
                playerDataMgr.saveData.consumableList.Add(id);
                playerDataMgr.saveData.consumableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableNumList[index]+=itemNum;
            }

            index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
            if (playerDataMgr.saveData.truckConsumableNumList[index] - itemNum== 0)
            {
                playerDataMgr.saveData.truckConsumableList.Remove(id);
                playerDataMgr.saveData.truckConsumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckConsumableNumList[index]-=itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentConsumables.ContainsKey(id))
            {
                //현재데이터 관련.
                storageConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
                storageConsumableNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.currentConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageConsumableNumInfo[currentKey]+=itemNum;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id]+=itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);

            if (playerDataMgr.truckConsumablesNum[id] -itemNum ==  0)
            {
                //현재 데이터.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Remove(id);
                playerDataMgr.truckConsumablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                truckConsumableNumInfo[currentKey]-=itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id]-=itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
        else if (truckOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(playerDataMgr.otherItemList[currentKey].weight);
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index;
            var id = truckOtherItemInfo[currentKey].id;
            if (!playerDataMgr.saveData.otherItemList.Contains(id))
            {
                playerDataMgr.saveData.otherItemList.Add(id);
                playerDataMgr.saveData.otherItemNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.otherItemList.IndexOf(id);
                playerDataMgr.saveData.otherItemNumList[index] += itemNum;
            }

            index = playerDataMgr.saveData.truckOtherItemList.IndexOf(id);
            if (playerDataMgr.saveData.truckOtherItemNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.truckOtherItemList.Remove(id);
                playerDataMgr.saveData.truckOtherItemNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckOtherItemNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentOtherItems.ContainsKey(id))
            {
                //현재데이터 관련.
                storageOtherItemInfo.Add(currentKey, truckOtherItemInfo[currentKey]);
                storageOtherItemNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItems.Add(id, playerDataMgr.otherItemList[id]);
                playerDataMgr.currentOtherItemsNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageOtherItemNumInfo[currentKey] += itemNum;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItemsNum[id] += itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorageItem(currentMode);

            if (playerDataMgr.truckOtherItemsNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckOtherItemInfo.Remove(currentKey);
                truckOtherItemNumInfo.Remove(currentKey);
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItems.Remove(id);
                playerDataMgr.truckOtherItemsNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //현재 데이터.
                truckOtherItemNumInfo[currentKey] -= itemNum;
               
                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItemsNum[id] -= itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
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
        else if (currentInvenKind == InvenKind.Truck && truckList.ContainsKey(currentKey))
        {
            var image = truckList[currentKey].GetComponent<Image>();
            image.color = originColor;
        }

        currentInvenKind = InvenKind.None;
        currentKey = null;
    }
}