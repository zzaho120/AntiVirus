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

        //Ʈ��.
        int i = 0;
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(prefab, truckContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.truckEquippablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}��";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
            truckList.Add(element.Key, go);
            truckWeaponInfo.Add(element.Key, element.Value);
            truckWeaponNumInfo.Add(element.Key, itemNum);

            i++;
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(prefab, truckContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.truckConsumablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}��";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Truck); });
            truckList.Add(element.Key, go);
            truckConsumableInfo.Add(element.Key, element.Value);
            truckConsumableNumInfo.Add(element.Key, itemNum);
            i++;
        }

        //â��.
        i = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(prefab, storageContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.currentEquippablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}��";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
            storageList.Add(element.Key, go);
            storageWeaponInfo.Add(element.Key, element.Value);
            storageWeaponNumInfo.Add(element.Key, itemNum);

           i++;
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            var go = Instantiate(prefab, storageContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            child = go.transform.GetChild(1).gameObject;
            var itemNum = playerDataMgr.currentConsumablesNum[element.Key];
            child.GetComponent<Text>().text = $"{itemNum}��";

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, InvenKind.Storage); });
            storageList.Add(element.Key, go);
            storageConsumableInfo.Add(element.Key, element.Value);
            storageConsumableNumInfo.Add(element.Key, itemNum);
            i++;
        }

        originColor = new Color(255, 192, 0);
        currentKey = null;
        currentInvenKind = InvenKind.None;
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

        Debug.Log($"currentKey : {currentKey}");

        if (currentInvenKind == InvenKind.Storage)
        {
            var image = storageList[currentKey].GetComponent<Image>();
            image.color = Color.red;
        }
        else if (currentInvenKind == InvenKind.Truck)
        {
            if (truckList[currentKey] == null) Debug.Log("����ֽ��ϴ�.");
            var image = truckList[currentKey].GetComponent<Image>();
            image.color = Color.red;
        }
    }

    public void MoveToTrunk()
    {
        if (currentKey == null) return;
        
        if (storageWeaponInfo.ContainsKey(currentKey))
        {
            //json.
            int index=0;
            var id = storageWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckEquippableList.Contains(id))
            {
                playerDataMgr.saveData.truckEquippableList.Add(id);
                playerDataMgr.saveData.truckEquippableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
                playerDataMgr.saveData.truckEquippableNumList[index]++;
            }

            Debug.Log($"index : {index} num : {playerDataMgr.saveData.truckEquippableNumList[index]}");

            index = playerDataMgr.saveData.equippableList.IndexOf(id);
            if (playerDataMgr.saveData.equippableNumList[index] == 1)
            {
                playerDataMgr.saveData.equippableList.Remove(id);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.equippableNumList[index]--;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckEquippables.ContainsKey(id))
            {
                //���絥���� ����.
                truckWeaponInfo.Add(currentKey, storageWeaponInfo[currentKey]);
                truckWeaponNumInfo.Add(currentKey, 1);

                var go = Instantiate(prefab, truckContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = storageWeaponInfo[currentKey].name;
                
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"1��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Truck); });
                truckList.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, 1);
            }
            else
            {
                //���絥���� ����.
                truckWeaponNumInfo[currentKey]++;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippablesNum[id]++;
            }

            if (playerDataMgr.currentEquippablesNum[id] == 1)
            {
                //���� ������.
                storageWeaponInfo.Remove(currentKey);
                storageWeaponNumInfo.Remove(currentKey);
                Destroy(storageList[currentKey]);
                storageList.Remove(currentKey);
                
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //���� ������.
                storageWeaponNumInfo[currentKey]--;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippablesNum[id]--;
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
                playerDataMgr.saveData.truckConsumableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
                playerDataMgr.saveData.truckConsumableNumList[index]++;
            }

            index = playerDataMgr.saveData.consumableList.IndexOf(id);
            if (playerDataMgr.saveData.consumableNumList[index] == 1)
            {
                playerDataMgr.saveData.consumableList.Remove(id);
                playerDataMgr.saveData.consumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.consumableNumList[index]--;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckConsumables.ContainsKey(id))
            {
                //���絥���� ����.
                truckConsumableInfo.Add(currentKey, storageConsumableInfo[currentKey]);
                truckConsumableNumInfo.Add(currentKey, 1);

                var go = Instantiate(prefab, truckContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = storageConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"1��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Truck); });
                truckList.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, 1);
            }
            else
            {
                //���絥���� ����.
                truckConsumableNumInfo[currentKey]++;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumablesNum[id]++;
            }

            if (playerDataMgr.currentConsumablesNum[id] == 1)
            {
                //���� ������.
                storageConsumableInfo.Remove(currentKey);
                storageConsumableNumInfo.Remove(currentKey);
                Destroy(storageList[currentKey]);
                storageList.Remove(currentKey);
                
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentConsumables.Remove(id);
                playerDataMgr.currentConsumablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //���� ������.
                storageConsumableNumInfo[currentKey]--;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentConsumablesNum[id]--;
            }
        }
    }

    public void MoveToStorage()
    {
        if (currentKey == null) return;

        if (truckWeaponInfo.ContainsKey(currentKey))
        {
            //json.
            int index=0;
            var id = truckWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.equippableList.Contains(id))
            {
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(1);
            }
            else
            {
               
                index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index]++;
            }
            Debug.Log($"index : {index} storage num : {playerDataMgr.saveData.equippableNumList[index]}");

            index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
            if (playerDataMgr.saveData.truckEquippableNumList[index] == 1)
            {
                playerDataMgr.saveData.truckEquippableList.Remove(id);
                playerDataMgr.saveData.truckEquippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckEquippableNumList[index]--;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentEquippables.ContainsKey(id))
            {
                //���絥���� ����.
                storageWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
                storageWeaponNumInfo.Add(currentKey, 1);

                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = truckWeaponInfo[currentKey].name;
                
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"1��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
                storageList.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //���絥���� ����.
                storageWeaponNumInfo[currentKey]++;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippablesNum[id]++;
            }

            if (playerDataMgr.truckEquippablesNum[id] == 1)
            {
                //���� ������.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                Destroy(truckList[currentKey]);
                truckList.Remove(currentKey);
                
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //���� ������.
                truckWeaponNumInfo[currentKey]--;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippablesNum[id]--;
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
                playerDataMgr.saveData.consumableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.consumableList.IndexOf(id);
                playerDataMgr.saveData.consumableNumList[index]++;
            }

            index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
            if (playerDataMgr.saveData.truckConsumableNumList[index] == 1)
            {
                playerDataMgr.saveData.truckConsumableList.Remove(id);
                playerDataMgr.saveData.truckConsumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckConsumableNumList[index]--;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentConsumables.ContainsKey(id))
            {
                //���絥���� ����.
                storageConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
                storageConsumableNumInfo.Add(currentKey, 1);

                var go = Instantiate(prefab, storageContent.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = truckConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"1��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
                storageList.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.currentConsumablesNum.Add(id, 1);
            }
            else
            {
                //���絥���� ����.
                storageConsumableNumInfo[currentKey]++;
                var child = storageList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentConsumablesNum[id]++;
            }

            if (playerDataMgr.truckConsumablesNum[id] == 1)
            {
                //���� ������.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);
                Destroy(truckList[currentKey]);
                truckList.Remove(currentKey);
                
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumables.Remove(id);
                playerDataMgr.truckConsumablesNum.Remove(id);

                currentKey = null;
                currentInvenKind = InvenKind.None;
            }
            else
            {
                //���� ������.
                truckConsumableNumInfo[currentKey]--;
                var child = truckList[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumablesNum[id]--;
            }
        }
    }
}