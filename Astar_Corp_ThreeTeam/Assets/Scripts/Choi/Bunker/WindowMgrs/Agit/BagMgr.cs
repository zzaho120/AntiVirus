using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    
    public GameObject bagWin;
    
    public GameObject storageContents;
    public GameObject storagePrefab;
    public Dictionary<string, GameObject> storageObjs = new Dictionary<string, GameObject>();
    public Dictionary<string, int> storageInfo = new Dictionary<string, int>();

    public GameObject bagContents;
    public GameObject bagPrefab;
    public Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();
    public Dictionary<string, int> bagInfo = new Dictionary<string, int>();
    
    public int currentIndex;
    string currentStorageKey;
    string currentBagKey;

    public void Init()
    {
        ////이전 정보 삭제.
        //if (storageObjs.Count != 0)
        //{
        //    foreach (var element in storageObjs)
        //    {
        //        Destroy(element.Value);
        //    }
        //    storageObjs.Clear();
        //    storageContents.transform.DetachChildren();
        //}
        //if (storageInfo.Count != 0) storageInfo.Clear();

        //if (bagObjs.Count != 0)
        //{
        //    foreach (var element in bagObjs)
        //    {
        //        Destroy(element.Value);
        //    }
        //    bagObjs.Clear();
        //    bagContents.transform.DetachChildren();
        //}
        //if (bagInfo.Count != 0) bagInfo.Clear();

        ////생성.
        //int i = 0;
        //foreach (var element in playerDataMgr.currentEquippables)
        //{
        //    var go = Instantiate(storagePrefab, storageContents.transform);
        //    var child = go.transform.GetChild(0).gameObject;
        //    child.GetComponent<Text>().text = element.Value.name;

        //    string key = element.Key;
        //    var button = go.AddComponent<Button>();
        //    button.onClick.AddListener(delegate { SelectStorageItem(key); });

        //    storageObjs.Add(key, go);
        //    storageInfo.Add(key, playerDataMgr.currentEquippablesNum[key]);
        //    i++;
        //}
        ////가방정보 불러오기.
        //i = 0;
        //foreach (var element in playerDataMgr.currentSquad[currentIndex].bag)
        //{
        //    var go = Instantiate(bagPrefab, bagContents.transform);

        //    string key = element.Key;
        //    var button = go.AddComponent<Button>();
        //    button.onClick.AddListener(delegate { SelectBagItem(key); });
        //    bagObjs.Add(key, go);
        //    i++;
        //}

        //currentStorageKey = null;
        //currentBagKey = null;
    }

    public void SelectStorageItem(string key)
    {
        if (currentStorageKey != null)
        {
            storageObjs[currentStorageKey].GetComponent<Image>().color = Color.white;
        }
       
        currentStorageKey = key;
        storageObjs[key].GetComponent<Image>().color = Color.red;
    }

    public void SelectBagItem(string key)
    {
        if (currentBagKey != null)
        {
            bagObjs[currentBagKey].GetComponent<Image>().color = Color.white;
        }

        currentBagKey = key;
        bagObjs[currentBagKey].GetComponent<Image>().color = Color.red;
    }

    public void MoveToBag()
    {
        if (currentStorageKey == null) return;

        Destroy(storageObjs[currentStorageKey]);
        storageObjs.Remove(currentStorageKey);
        
        var go = Instantiate(bagPrefab, bagContents.transform);
        var button = go.AddComponent<Button>();
        string key = currentStorageKey;
        button.onClick.AddListener(delegate { SelectBagItem(key); });
        bagObjs.Add(key, go);

        bagInfo.Add(currentStorageKey, storageInfo[currentStorageKey]);
        storageInfo.Remove(currentStorageKey);
        currentStorageKey = null;

        //if (truckWeaponInfo.ContainsKey(currentKey))
        //{
        //    //json.
        //    int index = 0;
        //    var id = truckWeaponInfo[currentKey].id;
        //    if (!playerDataMgr.saveData.equippableList.Contains(id))
        //    {
        //        playerDataMgr.saveData.equippableList.Add(id);
        //        playerDataMgr.saveData.equippableNumList.Add(1);
        //    }
        //    else
        //    {

        //        index = playerDataMgr.saveData.equippableList.IndexOf(id);
        //        playerDataMgr.saveData.equippableNumList[index]++;
        //    }
        //    Debug.Log($"index : {index} storage num : {playerDataMgr.saveData.equippableNumList[index]}");

        //    index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
        //    if (playerDataMgr.saveData.truckEquippableNumList[index] == 1)
        //    {
        //        playerDataMgr.saveData.truckEquippableList.Remove(id);
        //        playerDataMgr.saveData.truckEquippableNumList.RemoveAt(index);
        //    }
        //    else playerDataMgr.saveData.truckEquippableNumList[index]--;

        //    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //    //playerDataMgr.
        //    if (!playerDataMgr.currentEquippables.ContainsKey(id))
        //    {
        //        //현재데이터 관련.
        //        storageWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
        //        storageWeaponNumInfo.Add(currentKey, 1);

        //        var go = Instantiate(prefab, storageContent.transform);
        //        var child = go.transform.GetChild(0).gameObject;
        //        child.GetComponent<Text>().text = truckWeaponInfo[currentKey].name;

        //        child = go.transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"1개";

        //        var button = go.AddComponent<Button>();
        //        string selectedKey = currentKey;
        //        button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
        //        storageList.Add(selectedKey, go);

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
        //        playerDataMgr.currentEquippablesNum.Add(id, 1);
        //    }
        //    else
        //    {
        //        //현재데이터 관련.
        //        storageWeaponNumInfo[currentKey]++;
        //        var child = storageList[currentKey].transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"{storageWeaponNumInfo[currentKey]}개";

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.currentEquippablesNum[id]++;
        //    }

        //    if (playerDataMgr.truckEquippablesNum[id] == 1)
        //    {
        //        //현재 데이터.
        //        truckWeaponInfo.Remove(currentKey);
        //        truckWeaponNumInfo.Remove(currentKey);
        //        Destroy(truckList[currentKey]);
        //        truckList.Remove(currentKey);

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.truckEquippables.Remove(id);
        //        playerDataMgr.truckEquippablesNum.Remove(id);

        //        currentKey = null;
        //        currentInvenKind = InvenKind.None;
        //    }
        //    else
        //    {
        //        //현재 데이터.
        //        truckWeaponNumInfo[currentKey]--;
        //        var child = truckList[currentKey].transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.truckEquippablesNum[id]--;
        //    }
        //}
        //else if (truckConsumableInfo.ContainsKey(currentKey))
        //{
        //    //json.
        //    int index;
        //    var id = truckConsumableInfo[currentKey].id;
        //    if (!playerDataMgr.saveData.consumableList.Contains(id))
        //    {
        //        playerDataMgr.saveData.consumableList.Add(id);
        //        playerDataMgr.saveData.consumableNumList.Add(1);
        //    }
        //    else
        //    {
        //        index = playerDataMgr.saveData.consumableList.IndexOf(id);
        //        playerDataMgr.saveData.consumableNumList[index]++;
        //    }

        //    index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
        //    if (playerDataMgr.saveData.truckConsumableNumList[index] == 1)
        //    {
        //        playerDataMgr.saveData.truckConsumableList.Remove(id);
        //        playerDataMgr.saveData.truckConsumableNumList.RemoveAt(index);
        //    }
        //    else playerDataMgr.saveData.truckConsumableNumList[index]--;

        //    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //    //playerDataMgr.
        //    if (!playerDataMgr.currentConsumables.ContainsKey(id))
        //    {
        //        //현재데이터 관련.
        //        storageConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
        //        storageConsumableNumInfo.Add(currentKey, 1);

        //        var go = Instantiate(prefab, storageContent.transform);
        //        var child = go.transform.GetChild(0).gameObject;
        //        child.GetComponent<Text>().text = truckConsumableInfo[currentKey].name;

        //        child = go.transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"1개";

        //        var button = go.AddComponent<Button>();
        //        string selectedKey = currentKey;
        //        button.onClick.AddListener(delegate { SelectItem(selectedKey, InvenKind.Storage); });
        //        storageList.Add(selectedKey, go);

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.currentConsumables.Add(id, playerDataMgr.consumableList[id]);
        //        playerDataMgr.currentConsumablesNum.Add(id, 1);
        //    }
        //    else
        //    {
        //        //현재데이터 관련.
        //        storageConsumableNumInfo[currentKey]++;
        //        var child = storageList[currentKey].transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"{storageConsumableNumInfo[currentKey]}개";

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.currentConsumablesNum[id]++;
        //    }

        //    if (playerDataMgr.truckConsumablesNum[id] == 1)
        //    {
        //        //현재 데이터.
        //        truckConsumableInfo.Remove(currentKey);
        //        truckConsumableNumInfo.Remove(currentKey);
        //        Destroy(truckList[currentKey]);
        //        truckList.Remove(currentKey);

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.truckConsumables.Remove(id);
        //        playerDataMgr.truckConsumablesNum.Remove(id);

        //        currentKey = null;
        //        currentInvenKind = InvenKind.None;
        //    }
        //    else
        //    {
        //        //현재 데이터.
        //        truckConsumableNumInfo[currentKey]--;
        //        var child = truckList[currentKey].transform.GetChild(1).gameObject;
        //        child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

        //        //플레이어 데이터 매니저 관련.
        //        playerDataMgr.truckConsumablesNum[id]--;
        //    }
        //}
    }

    public void MoveToStorage()
    {
        if (currentBagKey == null) return;

        Destroy(bagObjs[currentBagKey]);
        bagObjs.Remove(currentBagKey);
        
        var go = Instantiate(storagePrefab, storageContents.transform);
        var child = go.transform.GetChild(0).gameObject;

        child.GetComponent<Text>().text = playerDataMgr.equippableList[currentBagKey].name;

        var button = go.AddComponent<Button>();
        string key = currentBagKey;
        button.onClick.AddListener(delegate { SelectStorageItem(key); });
        storageObjs.Add(key, go);

        storageInfo.Add(currentBagKey, bagInfo[currentBagKey]);
        bagInfo.Remove(currentBagKey);
        currentBagKey = null;
    }
}
