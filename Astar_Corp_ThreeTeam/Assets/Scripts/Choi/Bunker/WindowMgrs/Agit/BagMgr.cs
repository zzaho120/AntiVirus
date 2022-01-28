using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BagMode
{ 
    All,
    Equippables,
    Consumables,
    OtherItems
}

public enum StorageMode
{
    All,
    Equippables,
    Consumables,
    OtherItems
}

public enum InventoryKind
{ 
    None,
    Bag,
    Storage
}

public class BagMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    [Header("버튼 관련")]
    public List<GameObject> bagButtons;
    public List<GameObject> storageButtons;
   
    [Header("총알 관련")]
    public Text bagBullet5Txt;
    public Text bagBullet7Txt;
    public Text bagBullet9Txt;
    public Text bagBullet45Txt;
    public Text bagBullet12Txt;

    public Text storageBullet5Txt;
    public Text storageBullet7Txt;
    public Text storageBullet9Txt;
    public Text storageBullet45Txt;
    public Text storageBullet12Txt;

    public GameObject bagWin;
    public Text bagWeightTxt;
    public Text storageWeightTxt;
    
    public GameObject storageContents;
    public GameObject storagePrefab;
    public GameObject bagContents;
    public GameObject bagPrefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text valueTxt;
    public Text itemNumTxt;
    public Slider slider;

    public Dictionary<string, GameObject> storageObjs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> storageOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> storageOtherItemNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> bagWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> bagWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> bagOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> bagOtherItemNumInfo = new Dictionary<string, int>();

    int bagTotalWeight;
    int bagCurrentWeight;
    int storageCurrentWeight;

    int storageLevel;
    int maxStorageCapacity;
    public int currentIndex;
    InventoryKind currentInvenKind;
    BagMode bagMode;
    StorageMode storageMode;
    string currentKey;

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

        //이전 정보 삭제.
        if (storageWeaponInfo.Count != 0) storageWeaponInfo.Clear();
        if (storageWeaponNumInfo.Count != 0) storageWeaponNumInfo.Clear();
        if (storageConsumableInfo.Count != 0) storageConsumableInfo.Clear();
        if (storageConsumableNumInfo.Count != 0) storageConsumableNumInfo.Clear();
        if (storageOtherItemInfo.Count != 0) storageOtherItemInfo.Clear();
        if (storageOtherItemNumInfo.Count != 0) storageOtherItemNumInfo.Clear();

        if (bagWeaponInfo.Count != 0) bagWeaponInfo.Clear();
        if (bagWeaponNumInfo.Count != 0) bagWeaponNumInfo.Clear();
        if (bagConsumableInfo.Count != 0) bagConsumableInfo.Clear();
        if (bagConsumableNumInfo.Count != 0) bagConsumableNumInfo.Clear();
        if (bagOtherItemInfo.Count != 0) bagOtherItemInfo.Clear();
        if (bagOtherItemNumInfo.Count != 0) bagOtherItemNumInfo.Clear();

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

        //가방정보 불러오기.
        if (currentIndex != -1)
        {
            foreach (var element in playerDataMgr.currentSquad[currentIndex].bag)
            {
                if (playerDataMgr.equippableList.ContainsKey(element.Key))
                {
                    bagWeaponInfo.Add(element.Key, playerDataMgr.equippableList[element.Key]);
                    bagWeaponNumInfo.Add(element.Key, element.Value);
                }
                else if (playerDataMgr.consumableList.ContainsKey(element.Key))
                {
                    bagConsumableInfo.Add(element.Key, playerDataMgr.consumableList[element.Key]);
                    bagConsumableNumInfo.Add(element.Key, element.Value);
                }
                else if (playerDataMgr.otherItemList.ContainsKey(element.Key))
                {
                    bagOtherItemInfo.Add(element.Key, playerDataMgr.otherItemList[element.Key]);
                    bagOtherItemNumInfo.Add(element.Key, element.Value);
                }
            }
        }

        currentKey = null;
        currentInvenKind = InventoryKind.None;
        if (currentIndex != -1) DisplayBag(0);
        DisplayStorage(0);
    }

    public void NumAdjustment()
    {
        itemNumTxt.text = $"(선택 개수) {slider.value}개";
    }

    public void SelectItem(string key, InventoryKind kind)
    {
        if (currentKey != null)
        {
            if (kind == InventoryKind.Storage)
            {
                var image = storageObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
            else if (kind == InventoryKind.Bag)
            {
                var image = bagObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }

        currentKey = key;
        currentInvenKind = kind;

        if (currentInvenKind == InventoryKind.Storage)
        {
            var image = storageObjs[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (storageWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageWeaponInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt( storageWeaponInfo[currentKey].price * 0.7f) * storageWeaponNumInfo[currentKey]).ToString();
                slider.maxValue = storageWeaponNumInfo[currentKey];
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt(storageConsumableInfo[currentKey].price * 0.7f) * storageConsumableNumInfo[currentKey]).ToString();
                slider.maxValue = storageConsumableNumInfo[currentKey];
            }
            else if (storageOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageOtherItemInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt(int.Parse(storageOtherItemInfo[currentKey].price) * 0.7f) * storageOtherItemNumInfo[currentKey]).ToString();
                slider.maxValue = storageOtherItemNumInfo[currentKey];
            }
        }
        else if (currentInvenKind == InventoryKind.Bag)
        {
            var image = bagObjs[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (bagWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagWeaponInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt(bagWeaponInfo[currentKey].price * 0.7f) * bagWeaponNumInfo[currentKey]).ToString();
                slider.maxValue = bagWeaponNumInfo[currentKey];
            }
            else if (bagConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagConsumableInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt(bagConsumableInfo[currentKey].price * 0.7f) * bagConsumableNumInfo[currentKey]).ToString();
                slider.maxValue = bagConsumableNumInfo[currentKey];
            }
            else if (bagOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagOtherItemInfo[currentKey].name;
                valueTxt.text = (Mathf.FloorToInt(int.Parse(bagOtherItemInfo[currentKey].price) * 0.7f) * bagOtherItemNumInfo[currentKey]).ToString();
                slider.maxValue = bagOtherItemNumInfo[currentKey];
            }
        }

        slider.value = 0;
        itemNumTxt.text = $"(선택 개수) 0개";
        OpenPopup();
    }

    public void DisplayBag(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        //버튼 초기화.
        foreach (var element in bagButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
        }

        switch (index)
        {
            case 0:
                bagMode = BagMode.All;
                break;
            case 1:
                bagMode = BagMode.Equippables;
                break;
            case 2:
                bagMode = BagMode.Consumables;
                break;
            case 3:
                bagMode = BagMode.OtherItems;
                break;
        }
        bagButtons[index].GetComponent<Image>().color = Color.red;

        if (bagObjs.Count != 0)
        {
            foreach (var element in bagObjs)
            {
                Destroy(element.Value);
            }
            bagObjs.Clear();
            bagContents.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        bagCurrentWeight = 0;
        bagTotalWeight = 0;
        foreach (var element in playerDataMgr.currentSquad[currentIndex].bag)
        {
            if (playerDataMgr.equippableList.ContainsKey(element.Key))
                bagCurrentWeight += (playerDataMgr.equippableList[element.Key].weight * element.Value);
            else if (playerDataMgr.consumableList.ContainsKey(element.Key))
                bagCurrentWeight += (playerDataMgr.consumableList[element.Key].weight * element.Value);
            else if (playerDataMgr.otherItemList.ContainsKey(element.Key))
            {
                bagCurrentWeight += (int.Parse(playerDataMgr.otherItemList[element.Key].weight) * element.Value);

                switch (element.Key)
                {
                    case "BUL_0004":
                        bullet5Num += element.Value;
                        break;
                    case "BUL_0005":
                        bullet7Num += element.Value;
                        break;
                    case "BUL_0002":
                        bullet9Num += element.Value;
                        break;
                    case "BUL_0003":
                        bullet45Num += element.Value;
                        break;
                    case "BUL_0001":
                        bullet12Num += element.Value;
                        break;
                }
            }

            if (index == 1 && !playerDataMgr.equippableList.ContainsKey(element.Key)) continue;
            else if (index == 2 && !playerDataMgr.consumableList.ContainsKey(element.Key)) continue;
            else if (index == 3 && !playerDataMgr.otherItemList.ContainsKey(element.Key)) continue;
            var go = Instantiate(bagPrefab, bagContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            string name = string.Empty;
            int value = 0;
            int weight = 0;
            if (playerDataMgr.equippableList.ContainsKey(element.Key))
            {
                name = playerDataMgr.equippableList[element.Key].name;
                value = Mathf.FloorToInt( playerDataMgr.equippableList[element.Key].price * 0.7f )* element.Value;
                weight = playerDataMgr.equippableList[element.Key].weight * element.Value;
            }
            else if (playerDataMgr.consumableList.ContainsKey(element.Key))
            {
                name = playerDataMgr.consumableList[element.Key].name;
                value = Mathf.FloorToInt(playerDataMgr.consumableList[element.Key].price *0.7f)* element.Value;
                weight = playerDataMgr.consumableList[element.Key].weight * element.Value;
            }
            else if (playerDataMgr.otherItemList.ContainsKey(element.Key))
            {
                name = playerDataMgr.otherItemList[element.Key].name;
                value = Mathf.FloorToInt(int.Parse(playerDataMgr.otherItemList[element.Key].price) *0.7f)* element.Value;
                weight = int.Parse(playerDataMgr.otherItemList[element.Key].weight) * element.Value;
            }
            child.GetComponent<Text>().text = $"{name}";

            child = go.transform.GetChild(1).gameObject;
            child.GetComponent<Text>().text = $"{element.Value}";

            child = go.transform.GetChild(2).gameObject;
            child.GetComponent<Text>().text = $"{weight}";

            child = go.transform.GetChild(3).gameObject;
            child.GetComponent<Text>().text = $"{value}";

            string key = element.Key;
            var button = go.transform.GetChild(4).gameObject.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Bag); });

            bagObjs.Add(key, go);
        }

        var level = playerDataMgr.currentSquad[currentIndex].bagLevel;
        switch (level)
        {
            case 1:
                bagTotalWeight = playerDataMgr.bagList["BAG_0001"].weight;
                break;
        }
        bagWeightTxt.text = $"무게 {bagCurrentWeight}/{bagTotalWeight}";
        
        bagBullet5Txt.text = $"{bullet5Num}";
        bagBullet7Txt.text = $"{bullet7Num}";
        bagBullet9Txt.text = $"{bullet9Num}";
        bagBullet45Txt.text = $"{bullet45Num}";
        bagBullet12Txt.text = $"{bullet12Num}";
    }

    public void DisplayStorage(int index)
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
        
        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (storageMode != StorageMode.Consumables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{ element.Value.weight * itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{ Mathf.FloorToInt(element.Value.price * 0.7f) * itemNum}";

                string key = element.Key;
                var button = go.transform.GetChild(4).gameObject.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Storage); });

                //이미지.

                storageObjs.Add(key, go);
            }
            storageCurrentWeight += (element.Value.weight * playerDataMgr.currentEquippablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.OtherItems)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{ element.Value.weight * itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt(element.Value.price *0.7f) * itemNum}";

                string key = element.Key;
                var button = go.transform.GetChild(4).gameObject.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Storage); });

                storageObjs.Add(key, go);
            }
                storageCurrentWeight += (element.Value.weight * playerDataMgr.currentConsumablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.currentOtherItems)
        {
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

            if (storageMode != StorageMode.Equippables && storageMode != StorageMode.Consumables)
            {
                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;

                var itemNum = playerDataMgr.currentOtherItemsNum[element.Key];
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}";

                child = go.transform.GetChild(2).gameObject;
                child.GetComponent<Text>().text = $"{ int.Parse(element.Value.weight) * itemNum}";

                child = go.transform.GetChild(3).gameObject;
                child.GetComponent<Text>().text = $"{Mathf.FloorToInt( int.Parse( element.Value.price) *0.7f) * itemNum}";

                string key = element.Key;
                var button = go.transform.GetChild(4).gameObject.GetComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Storage); });

                //이미지.

                storageObjs.Add(key, go);
            }
            storageCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.currentOtherItemsNum[element.Key]);
        }
       
        //레벨에 따라 구현해야 함.
        storageWeightTxt.text = $"무게 {storageCurrentWeight}/{maxStorageCapacity}";

        storageBullet5Txt.text = $"{bullet5Num}";
        storageBullet7Txt.text = $"{bullet7Num}";
        storageBullet9Txt.text = $"{bullet9Num}";
        storageBullet45Txt.text = $"{bullet45Num}";
        storageBullet12Txt.text = $"{bullet12Num}";
    }

    public void Move()
    {
        if (currentInvenKind == InventoryKind.Storage)
            MoveToBag(Mathf.FloorToInt(slider.value));
        else if (currentInvenKind == InventoryKind.Bag)
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
        else
        {
            slider.value += plus;
            itemNumTxt.text = $"(선택 개수) {slider.value}개";
        }
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

    public void MoveToBag(int itemNum)
    {
        if (currentKey == null) return;
        if (itemNum == 0) return;
        OpenPopup();

        if (storageWeaponInfo.ContainsKey(currentKey))
        {
            var weight = storageWeaponInfo[currentKey].weight;
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

            //json.
            int index = 0;
            var id = storageWeaponInfo[currentKey].id;

            index = currentIndex;

            int firstIndex = playerDataMgr.saveData.bagEquippableFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagEquippableLastIndex[index];

            bool contain = false;
            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagEquippableList[i] != id) continue;

                contain = true;
                containIndex = i;
            }

            if (!contain)
            {
                playerDataMgr.saveData.bagEquippableList.Insert(lastIndex, id);
                playerDataMgr.saveData.bagEquippableNumList.Insert(lastIndex, itemNum);
                playerDataMgr.saveData.bagEquippableLastIndex[index]++;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagEquippableFirstIndex[i]++;
                    playerDataMgr.saveData.bagEquippableLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagEquippableNumList[containIndex] += itemNum;
            }

            index = playerDataMgr.saveData.equippableList.IndexOf(id);
            if (playerDataMgr.saveData.equippableNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.equippableList.Remove(id);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.equippableNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //현재데이터 관련.
                bagWeaponInfo.Add(currentKey, storageWeaponInfo[currentKey]);
                bagWeaponNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagWeaponNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            int currentMode = (int)bagMode;
            DisplayBag(currentMode);

            if (playerDataMgr.currentEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);
                //Destroy(storageObjs[currentKey]);
                //storageObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                storageWeaponNumInfo[currentKey] -= itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorage(currentMode);
        }
        else if (storageConsumableInfo.ContainsKey(currentKey))
        {
            var weight = storageConsumableInfo[currentKey].weight;
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

            //json.
            int index = 0;
            var id = storageConsumableInfo[currentKey].id;

            index = playerDataMgr.currentSquad[currentIndex].saveId;
            int firstIndex = playerDataMgr.saveData.bagConsumableFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagConsumableLastIndex[index];

            bool contain = false;
            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagConsumableList[i] != id) continue;

                contain = true;
                containIndex = i;
            }

            if (!contain)
            {
                playerDataMgr.saveData.bagConsumableList.Insert(lastIndex, id);
                playerDataMgr.saveData.bagConsumableNumList.Insert(lastIndex, itemNum);
                playerDataMgr.saveData.bagConsumableLastIndex[index]++;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagConsumableFirstIndex[i]++;
                    playerDataMgr.saveData.bagConsumableLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagConsumableNumList[containIndex] += itemNum;
            }

            index = playerDataMgr.saveData.consumableList.IndexOf(id);
            if (playerDataMgr.saveData.consumableNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.consumableList.Remove(id);
                playerDataMgr.saveData.consumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.consumableNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //현재데이터 관련.
                bagConsumableInfo.Add(currentKey, storageConsumableInfo[currentKey]);
                bagConsumableNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagConsumableNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            int currentMode = (int)bagMode;
            DisplayBag(currentMode);

            if (playerDataMgr.currentConsumablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);
                //Destroy(storageObjs[currentKey]);
                //storageObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Remove(id);
                playerDataMgr.currentConsumablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                storageConsumableNumInfo[currentKey] -= itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id] -= itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorage(currentMode);
        }
        else if (storageOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(storageOtherItemInfo[currentKey].weight);
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

            //json.
            int index = 0;
            var id = storageOtherItemInfo[currentKey].id;

            index = playerDataMgr.currentSquad[currentIndex].saveId;
            int firstIndex = playerDataMgr.saveData.bagOtherItemFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagOtherItemLastIndex[index];

            bool contain = false;
            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagOtherItemList[i] != id) continue;

                contain = true;
                containIndex = i;
            }

            if (!contain)
            {
                playerDataMgr.saveData.bagOtherItemList.Insert(lastIndex, id);
                playerDataMgr.saveData.bagOtherItemNumList.Insert(lastIndex, itemNum);
                playerDataMgr.saveData.bagOtherItemLastIndex[index]++;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagOtherItemFirstIndex[i]++;
                    playerDataMgr.saveData.bagOtherItemLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagOtherItemNumList[containIndex] += itemNum;
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
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //현재데이터 관련.
                bagOtherItemInfo.Add(currentKey, storageOtherItemInfo[currentKey]);
                bagOtherItemNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagOtherItemNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            int currentMode = (int)bagMode;
            DisplayBag(currentMode);

            if (playerDataMgr.currentOtherItemsNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageOtherItemInfo.Remove(currentKey);
                storageOtherItemNumInfo.Remove(currentKey);
                //Destroy(storageObjs[currentKey]);
                //storageObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItems.Remove(id);
                playerDataMgr.currentOtherItemsNum.Remove(id);

                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                storageOtherItemNumInfo[currentKey] -= itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItemsNum[id] -= itemNum;
            }
            currentMode = (int)storageMode;
            DisplayStorage(currentMode);
        }
    }

    public void MoveToStorage(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (bagWeaponInfo.ContainsKey(currentKey))
        {
            var weight = bagWeaponInfo[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index = 0;
            var id = bagWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.equippableList.Contains(id))
            {
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] += itemNum;
            }

            index = playerDataMgr.currentSquad[currentIndex].saveId;
            int firstIndex = playerDataMgr.saveData.bagEquippableFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagEquippableLastIndex[index];

            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagEquippableList[i] != id) continue;

                containIndex = i;
            }

            if (playerDataMgr.saveData.bagEquippableNumList[containIndex] - itemNum == 0)
            {
                playerDataMgr.saveData.bagEquippableList.RemoveAt(containIndex);
                playerDataMgr.saveData.bagEquippableNumList.RemoveAt(containIndex);

                playerDataMgr.saveData.bagEquippableLastIndex[index]--;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagEquippableFirstIndex[i]--;
                    playerDataMgr.saveData.bagEquippableLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagEquippableNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                storageWeaponInfo.Add(currentKey, bagWeaponInfo[currentKey]);
                storageWeaponNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(storagePrefab, storageContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagWeaponInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Storage); });
                //storageObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageWeaponNumInfo[currentKey] += itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorage(currentMode);

            if (bagWeaponNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
                
                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                bagWeaponNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBag(currentMode);
        }
        else if (bagConsumableInfo.ContainsKey(currentKey))
        {
            var weight = bagConsumableInfo[currentKey].weight;
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index;
            var id = bagConsumableInfo[currentKey].id;
            if (!playerDataMgr.saveData.consumableList.Contains(id))
            {
                playerDataMgr.saveData.consumableList.Add(id);
                playerDataMgr.saveData.consumableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableNumList[index] += itemNum;
            }

            index = currentIndex;
            int firstIndex = playerDataMgr.saveData.bagConsumableFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagConsumableLastIndex[index];

            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagConsumableList[i] != id) continue;

                containIndex = i;
            }

            if (playerDataMgr.saveData.bagConsumableNumList[containIndex] - itemNum == 0)
            {
                playerDataMgr.saveData.bagConsumableList.RemoveAt(containIndex);
                playerDataMgr.saveData.bagConsumableNumList.RemoveAt(containIndex);

                playerDataMgr.saveData.bagConsumableLastIndex[index]--;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagConsumableFirstIndex[i]--;
                    playerDataMgr.saveData.bagConsumableLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagConsumableNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentConsumables.ContainsKey(id))
            {
                //현재데이터 관련.
                storageConsumableInfo.Add(currentKey, bagConsumableInfo[currentKey]);
                storageConsumableNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(storagePrefab, storageContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagConsumableInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Storage); });
                //storageObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.currentConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageConsumableNumInfo[currentKey] += itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id] += itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorage(currentMode);

            if (bagConsumableNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagConsumableInfo.Remove(currentKey);
                bagConsumableNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
                
                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                bagConsumableNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBag(currentMode);
        }
        else if (bagOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(bagOtherItemInfo[currentKey].weight);
            if (storageCurrentWeight + weight * itemNum > maxStorageCapacity) return;

            //json.
            int index = 0;
            var id = bagOtherItemInfo[currentKey].id;
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

            index = playerDataMgr.currentSquad[currentIndex].saveId;
            int firstIndex = playerDataMgr.saveData.bagOtherItemFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagOtherItemLastIndex[index];

            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagOtherItemList[i] != id) continue;

                containIndex = i;
            }

            if (playerDataMgr.saveData.bagOtherItemNumList[containIndex] - itemNum == 0)
            {
                playerDataMgr.saveData.bagOtherItemList.RemoveAt(containIndex);
                playerDataMgr.saveData.bagOtherItemNumList.RemoveAt(containIndex);

                playerDataMgr.saveData.bagOtherItemLastIndex[index]--;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagOtherItemFirstIndex[i]--;
                    playerDataMgr.saveData.bagOtherItemLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagOtherItemNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentOtherItems.ContainsKey(id))
            {
                //현재데이터 관련.
                storageOtherItemInfo.Add(currentKey, bagOtherItemInfo[currentKey]);
                storageOtherItemNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(storagePrefab, storageContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagWeaponInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Storage); });
                //storageObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItems.Add(id, playerDataMgr.otherItemList[id]);
                playerDataMgr.currentOtherItemsNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageOtherItemNumInfo[currentKey] += itemNum;
                //var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentOtherItemsNum[id] += itemNum;
            }
            var currentMode = (int)storageMode;
            DisplayStorage(currentMode);

            if (bagOtherItemNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagOtherItemInfo.Remove(currentKey);
                bagOtherItemNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                bagOtherItemNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBag(currentMode);
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

        if (currentInvenKind == InventoryKind.Storage)
        {
            var image = storageObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }
        else if (currentInvenKind == InventoryKind.Bag)
        {
            var image = bagObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentInvenKind = InventoryKind.None;
        currentKey = null;
    }
}
