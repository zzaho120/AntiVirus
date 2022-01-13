using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryKind
{ 
    None,
    Bag,
    Storage
}

public class BagMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    
    public GameObject bagWin;
    
    public GameObject storageContents;
    public GameObject storagePrefab;
    public GameObject bagContents;
    public GameObject bagPrefab;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemNumTxt;
    public Slider slider;

    public Dictionary<string, GameObject> storageObjs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> storageWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> storageWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> storageConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> storageConsumableNumInfo = new Dictionary<string, int>();
    
    Dictionary<string, Weapon> bagWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> bagWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();

    public int currentIndex;
    InventoryKind currentInvenKind;
    string currentKey;

    public void Init()
    {
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

        if (bagObjs.Count != 0)
        {
            foreach (var element in bagObjs)
            {
                Destroy(element.Value);
            }
            bagObjs.Clear();
            bagContents.transform.DetachChildren();
        }
        if (bagWeaponInfo.Count != 0) bagWeaponInfo.Clear();
        if (bagWeaponNumInfo.Count != 0) bagWeaponNumInfo.Clear();
        if (bagConsumableInfo.Count != 0) bagConsumableInfo.Clear();
        if (bagConsumableNumInfo.Count != 0) bagConsumableNumInfo.Clear();

        //생성.
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(storagePrefab, storageContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
            child = go.transform.GetChild(1).gameObject;
            child.GetComponent<Text>().text = $"{itemNum}개";

            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Storage); });

            storageObjs.Add(key, go);
            storageWeaponInfo.Add(element.Key, element.Value);
            storageWeaponNumInfo.Add(element.Key, itemNum);
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            var go = Instantiate(storagePrefab, storageContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
            child = go.transform.GetChild(1).gameObject;
            child.GetComponent<Text>().text = $"{itemNum}개";

            string key = element.Key;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Storage); });

            storageObjs.Add(key, go);
            storageConsumableInfo.Add(element.Key, element.Value);
            storageConsumableNumInfo.Add(element.Key, itemNum);
        }

        //가방정보 불러오기.
        if (currentIndex != -1)
        {
            foreach (var element in playerDataMgr.currentSquad[currentIndex].bag)
            {
                var go = Instantiate(bagPrefab, bagContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{element.Value}개";

                string key = element.Key;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(key, InventoryKind.Bag); });

                bagObjs.Add(key, go);
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
            }
        }

        currentKey = null;
        currentInvenKind = InventoryKind.None;
    }

    public void NumAdjustment()
    {
        itemNumTxt.text = $"{slider.value}개";
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
                slider.maxValue = storageWeaponNumInfo[currentKey];
            }
            else if (storageConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = storageConsumableInfo[currentKey].name;
                slider.maxValue = storageConsumableNumInfo[currentKey];
            }
        }
        else if (currentInvenKind == InventoryKind.Bag)
        {
            var image = bagObjs[currentKey].GetComponent<Image>();
            image.color = Color.red;

            if (bagWeaponInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagWeaponInfo[currentKey].name;
                slider.maxValue = bagWeaponNumInfo[currentKey];
            }
            else if (bagConsumableInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagConsumableInfo[currentKey].name;
                slider.maxValue = bagConsumableNumInfo[currentKey];
            }
        }

        slider.value = 0;
        itemNumTxt.text = $"0개";
        OpenPopup();
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

    public void MoveToBag(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (storageWeaponInfo.ContainsKey(currentKey))
        {
            //json.
            int index = 0;
            var id = storageWeaponInfo[currentKey].id;

            index = playerDataMgr.currentSquad[currentIndex].saveId;

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
                    playerDataMgr.saveData.bagEquippableFirstIndex[index]++;
                    playerDataMgr.saveData.bagEquippableLastIndex[index]++;
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

                var go = Instantiate(bagPrefab, bagContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Bag); });
                bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagWeaponNumInfo[currentKey] += itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.currentEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);
                Destroy(storageObjs[currentKey]);
                storageObjs.Remove(currentKey);

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
                var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= itemNum;
            }
        }
        else if (storageConsumableInfo.ContainsKey(currentKey))
        {
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
                    playerDataMgr.saveData.bagConsumableFirstIndex[index]++;
                    playerDataMgr.saveData.bagConsumableLastIndex[index]++;
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

                var go = Instantiate(bagPrefab, bagContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Bag); });
                bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagConsumableNumInfo[currentKey] += itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.currentConsumablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);
                Destroy(storageObjs[currentKey]);
                storageObjs.Remove(currentKey);

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
                var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id] -= itemNum;
            }
        }
    }

    public void MoveToStorage(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (bagWeaponInfo.ContainsKey(currentKey))
        {
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
                    playerDataMgr.saveData.bagEquippableFirstIndex[index]--;
                    playerDataMgr.saveData.bagEquippableLastIndex[index]--;
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

                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = bagWeaponInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Storage); });
                storageObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageWeaponNumInfo[currentKey] += itemNum;
                var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += itemNum;
            }

            if (bagWeaponNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);
                Destroy(bagObjs[currentKey]);
                bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
                
                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                bagWeaponNumInfo[currentKey] -= itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
        }
        else if (bagConsumableInfo.ContainsKey(currentKey))
        {
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

            index = playerDataMgr.currentSquad[currentIndex].saveId;
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
                    playerDataMgr.saveData.bagConsumableFirstIndex[index]--;
                    playerDataMgr.saveData.bagConsumableLastIndex[index]--;
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

                var go = Instantiate(storagePrefab, storageContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = bagConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}개";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InventoryKind.Storage); });
                storageObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.currentConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                storageConsumableNumInfo[currentKey] += itemNum;
                var child = storageObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentConsumablesNum[id] += itemNum;
            }

            if (bagConsumableNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagConsumableInfo.Remove(currentKey);
                bagConsumableNumInfo.Remove(currentKey);
                Destroy(bagObjs[currentKey]);
                bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
                
                currentKey = null;
                currentInvenKind = InventoryKind.None;
            }
            else
            {
                //현재 데이터.
                bagConsumableNumInfo[currentKey] -= itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
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
