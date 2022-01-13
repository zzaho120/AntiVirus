using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InvenKind
{ 
    None,
    Truck,
    Storage
}

public class TrunkMgr : MonoBehaviour
{
    public GameObject truckContent;
    public GameObject storageContent;
    public GameObject prefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemNumTxt;
    public Slider slider;

    Dictionary<string, GameObject> truckList = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> storageList = new Dictionary<string,  GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();

    public PlayerDataMgr playerDataMgr;
    Color originColor;
    string currentKey;
    InvenKind currentInvenKind;

    public void Init()
    {
        ClosePopup();

        if (storageList.Count != 0)
        {
            foreach (var element in storageList)
            {
                Destroy(element.Value);
            }
            storageList.Clear();
            storageContent.transform.DetachChildren();
        }
        if (storageWeaponInfo.Count != 0) storageWeaponInfo.Clear();
        if (storageWeaponNumInfo.Count != 0) storageWeaponNumInfo.Clear();
        if (storageConsumableInfo.Count != 0) storageConsumableInfo.Clear();
        if (storageConsumableNumInfo.Count != 0) storageConsumableNumInfo.Clear();

        if (truckList.Count != 0)
        {
            foreach (var element in truckList)
            {
                Destroy(element.Value);
            }
            truckList.Clear();
            truckContent.transform.DetachChildren();
        }
        if (truckWeaponInfo.Count != 0) truckWeaponInfo.Clear();
        if (truckWeaponNumInfo.Count != 0) truckWeaponNumInfo.Clear();
        if (truckConsumableInfo.Count != 0) truckConsumableInfo.Clear();
        if (truckConsumableNumInfo.Count != 0) truckConsumableNumInfo.Clear();

        //트럭.
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(prefab, truckContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.truckEquippablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}개";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
            truckList.Add(element.Key, go);
            truckWeaponInfo.Add(element.Key, element.Value);
            truckWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(prefab, truckContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.truckConsumablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}개";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
            truckList.Add(element.Key, go);
            truckConsumableInfo.Add(element.Key, element.Value);
            truckConsumableNumInfo.Add(element.Key, itemNum);
        }

        //창고.
        foreach (var element in playerDataMgr.currentEquippables)
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
            storageWeaponInfo.Add(element.Key, element.Value);
            storageWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.currentConsumables)
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
            storageConsumableInfo.Add(element.Key, element.Value);
            storageConsumableNumInfo.Add(element.Key, itemNum);
        }

        originColor = prefab.GetComponent<Image>().color;
        currentKey = null;
        currentInvenKind = InvenKind.None;
    }

    public void NumAdjustment()
    {
       itemNumTxt.text = $"{slider.value}개";
    }

    public void SelectItem(string key, InvenKind kind)
    {
        if (currentKey != null)
        {
            if (currentInvenKind == InvenKind.Storage)
            {
                var image = storageList[currentKey].GetComponent<Image>();
                image.color = originColor;
            }
            else if(currentInvenKind == InvenKind.Truck)
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
                slider.maxValue = storageWeaponNumInfo[currentKey];
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                slider.maxValue = storageConsumableNumInfo[currentKey];
            }
        }
        else if (currentInvenKind == InvenKind.Truck)
        {
            var image = truckList[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (truckWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckWeaponInfo[currentKey].name;
                slider.maxValue = truckWeaponNumInfo[currentKey];
            }
            else if (truckConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckConsumableInfo[currentKey].name;
                slider.maxValue = truckConsumableNumInfo[currentKey];
            }
        }

        slider.value = 0;
        itemNumTxt.text = $"0개";
        OpenPopup();
    }

    public void Move()
    {
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
            itemNumTxt.text = $"{slider.value}개";
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

    public void MoveToTrunk(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (storageWeaponInfo.ContainsKey(currentKey))
        {
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

                var go = Instantiate(prefab, truckContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = storageWeaponInfo[currentKey].name;
                
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Truck); });
                truckList.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[currentKey]+=itemNum;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id]+=itemNum;
            }

            if (playerDataMgr.currentEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);
                Destroy(storageList[currentKey]);
                storageList.Remove(currentKey);
                
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
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id]-=itemNum;
            }
        }
        else if (storageConsumableInfo.ContainsKey(currentKey))
        {
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

                var go = Instantiate(prefab, truckContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = storageConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Truck); });
                truckList.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckConsumableNumInfo[currentKey]+=itemNum;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id]+=itemNum;
            }

            if (playerDataMgr.currentConsumablesNum[id] -itemNum== 0)
            {
                //현재 데이터.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);
                Destroy(storageList[currentKey]);
                storageList.Remove(currentKey);
                
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
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id]-=itemNum;
            }
        }
    }

    public void MoveToStorage(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (truckWeaponInfo.ContainsKey(currentKey))
        {
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

                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = truckWeaponInfo[currentKey].name;
                
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
                storageList.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageWeaponNumInfo[currentKey]+=itemNum;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id]+=itemNum;
            }

            if (playerDataMgr.truckEquippablesNum[id] -itemNum== 0)
            {
                //현재 데이터.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                Destroy(truckList[currentKey]);
                truckList.Remove(currentKey);
                
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
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id]-=itemNum;
            }
        }
        else if (truckConsumableInfo.ContainsKey(currentKey))
        {
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

                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = truckConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
                storageList.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.currentConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageConsumableNumInfo[currentKey]+=itemNum;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id]+=itemNum;
            }

            if (playerDataMgr.truckConsumablesNum[id] -itemNum ==  0)
            {
                //현재 데이터.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);
                Destroy(truckList[currentKey]);
                truckList.Remove(currentKey);
                
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
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id]-=itemNum;
            }
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

        if (currentInvenKind == InvenKind.Storage)
        {
            var image = storageList[currentKey].GetComponent<Image>();
            image.color = originColor;
        }
        else if (currentInvenKind == InvenKind.Truck)
        {
            var image = truckList[currentKey].GetComponent<Image>();
            image.color = originColor;
        }

        currentInvenKind = InvenKind.None;
        currentKey = null;
    }
}