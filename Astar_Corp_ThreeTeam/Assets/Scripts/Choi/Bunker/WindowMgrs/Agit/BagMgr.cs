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
    public Dictionary<int, GameObject> storageObjs = new Dictionary<int, GameObject>();
    public Dictionary<int, string> storageInfo = new Dictionary<int, string>();
    List<int> storageDiscardedIndex = new List<int>();

    public GameObject bagContents;
    public GameObject bagPrefab;
    public Dictionary<int, GameObject> bagObjs = new Dictionary<int, GameObject>();
    public Dictionary<int, string> bagInfo = new Dictionary<int, string>();
    List<int> bagDiscardedIndex = new List<int>();

    public int currentIndex;
    int currentStorageIndex;
    int currentBagIndex;

    public void Init()
    {
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
        if (storageInfo.Count != 0) storageInfo.Clear();

        if (bagObjs.Count != 0)
        {
            foreach (var element in bagObjs)
            {
                Destroy(element.Value);
            }
            bagObjs.Clear();
            bagContents.transform.DetachChildren();
        }
        if (bagInfo.Count != 0) bagInfo.Clear();

        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(storagePrefab, storageContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectStorageItem(num); });

            storageObjs.Add(num, go);
            storageInfo.Add(num, element.Key);
            i++;
        }
        //가방정보 불러오기.
        //i = 0;
        //foreach (var element in playerDataMgr.currentEquippables)
        //{
        //    var go = Instantiate(bagPrefab, bagContents.transform);

        //    int num = i;
        //    var button = go.AddComponent<Button>();
        //    button.onClick.AddListener(delegate { SelectBagItem(num); });
        //    bagObjs.Add(num, go);
        //    i++;
        //}

        currentStorageIndex = -1;
        currentBagIndex = -1;
    }

    public void SelectStorageItem(int index)
    {
        if (currentStorageIndex != -1)
        {
            storageObjs[currentStorageIndex].GetComponent<Image>().color = Color.white;
        }
       
        currentStorageIndex = index;
        storageObjs[currentStorageIndex].GetComponent<Image>().color = Color.red;
    }

    public void SelectBagItem(int index)
    {
        if (currentBagIndex != -1)
        {
            bagObjs[currentBagIndex].GetComponent<Image>().color = Color.white;
        }

        currentBagIndex = index;
        bagObjs[currentBagIndex].GetComponent<Image>().color = Color.red;
    }

    public void MoveToBag()
    {
        if (currentStorageIndex == -1) return;

        Destroy(storageObjs[currentStorageIndex]);
        storageObjs.Remove(currentStorageIndex);
        storageDiscardedIndex.Add(currentStorageIndex);

        var go = Instantiate(bagPrefab, bagContents.transform);

        int num;
        if (storageDiscardedIndex.Count != 0)
        {
            num = storageDiscardedIndex[0];
            storageDiscardedIndex.RemoveAt(0);
        }
        else num = bagObjs.Count;

        var button = go.AddComponent<Button>();
        button.onClick.AddListener(delegate { SelectBagItem(num); });
        bagObjs.Add(num, go);

        bagInfo.Add(num, storageInfo[currentStorageIndex]);
        storageInfo.Remove(currentStorageIndex);
        currentStorageIndex = -1;
    }

    public void MoveToStorage()
    {
        if (currentBagIndex == -1) return;

        Destroy(bagObjs[currentBagIndex]);
        bagObjs.Remove(currentBagIndex);
        bagDiscardedIndex.Add(currentBagIndex);

        var go = Instantiate(storagePrefab, storageContents.transform);
        var child = go.transform.GetChild(0).gameObject;

        var key = bagInfo[currentBagIndex];
        child.GetComponent<Text>().text = playerDataMgr.equippableList[key].name;

        int num;
        if (bagDiscardedIndex.Count != 0)
        {
            num = bagDiscardedIndex[0];
            bagDiscardedIndex.RemoveAt(0);
        }
        else num = storageObjs.Count;

        var button = go.AddComponent<Button>();
        button.onClick.AddListener(delegate { SelectStorageItem(num); });
        storageObjs.Add(num, go);

        storageInfo.Add(num, bagInfo[currentBagIndex]);
        bagInfo.Remove(currentBagIndex);
        currentBagIndex = -1;
    }
}
