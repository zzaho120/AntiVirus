using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InvenKind
{ 
    None,
    Trunk,
    Storage
}

public class TrunkMgr : MonoBehaviour
{
    public GameObject trunkContent;
    public GameObject storageContent;
    public GameObject prefab;

    Dictionary<int, GameObject> trunkList = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> storageList = new Dictionary<int,  GameObject>();
    Dictionary<int, Weapon> storageWeaponInfo = new Dictionary<int, Weapon>();
    Dictionary<int, Consumable> storageConsumableInfo = new Dictionary<int, Consumable>();

    Dictionary<int, Weapon> trunkWeaponInfo = new Dictionary<int, Weapon>();
    Dictionary<int, Consumable> trunkConsumableInfo = new Dictionary<int, Consumable>();

    List<int> trunkDiscardedIndex = new List<int>();
    List<int> storageDiscardedIndex = new List<int>();
    
    public PlayerDataMgr playerDataMgr;
    Color originColor;
    int currentIndex;
    InvenKind currentInvenKind;

    public void Init()
    {
        int i = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(prefab, storageContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;
           
            var button = go.AddComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { SelectItem(num, InvenKind.Storage); });
            storageList.Add(num, go);
            storageWeaponInfo.Add(num, element.Value);

           i++;
        }

        foreach (var element in playerDataMgr.currentConsumables)
        {
            var go = Instantiate(prefab, storageContent.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;

            var button = go.AddComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { SelectItem(num, InvenKind.Storage); });
            storageList.Add(num, go);
            storageConsumableInfo.Add(num, element.Value);

            i++;
        }

        originColor = new Color(255, 192, 0);
        currentIndex = -1;
        currentInvenKind = InvenKind.None;
    }

    public void SelectItem(int i, InvenKind kind)
    {
        if (currentIndex != -1)
        {
            if (currentInvenKind == InvenKind.Storage)
            {
                var image = storageList[currentIndex].GetComponent<Image>();
                image.color = originColor;
            }
            else if(currentInvenKind == InvenKind.Trunk)
            {
                var image = trunkList[currentIndex].GetComponent<Image>();
                image.color = originColor;
            }
        }
        currentIndex = i;
        currentInvenKind = kind;

        if (currentInvenKind == InvenKind.Storage)
        {
            var image = storageList[currentIndex].GetComponent<Image>();
            image.color = Color.red;
        }
        else if (currentInvenKind == InvenKind.Trunk)
        {
            var image = trunkList[currentIndex].GetComponent<Image>();
            image.color = Color.red;
        }
    }

    public void MoveToTrunk()
    {
        if (currentIndex == -1) return;

        Destroy(storageList[currentIndex]);

        var go = Instantiate(prefab, trunkContent.transform);
        var child = go.transform.GetChild(0).gameObject;

        if (storageWeaponInfo.ContainsKey(currentIndex))
            child.GetComponent<Text>().text = storageWeaponInfo[currentIndex].name;
        else if(storageConsumableInfo.ContainsKey(currentIndex))
            child.GetComponent<Text>().text = storageConsumableInfo[currentIndex].name;
        
        int num;
        if (trunkDiscardedIndex.Count != 0)
        {
            num = trunkDiscardedIndex[0];
            trunkDiscardedIndex.RemoveAt(0);
        }
        else num = trunkList.Count;

        var button = go.AddComponent<Button>();
        button.onClick.AddListener(delegate { SelectItem(num, InvenKind.Trunk); });
        trunkList.Add(num, go);

        if (storageWeaponInfo.ContainsKey(currentIndex))
        {
            trunkWeaponInfo.Add(num, storageWeaponInfo[currentIndex]);
            storageWeaponInfo.Remove(currentIndex);
        }
        else if (storageConsumableInfo.ContainsKey(currentIndex))
        {
            trunkConsumableInfo.Add(num, storageConsumableInfo[currentIndex]);
            storageConsumableInfo.Remove(currentIndex);
        }
        storageList.Remove(currentIndex);
        storageDiscardedIndex.Add(currentIndex);

        currentIndex = -1;
        currentInvenKind = InvenKind.None;
    }

    public void MoveToStorage()
    {
        if (currentIndex == -1) return;

        Destroy(trunkList[currentIndex]);

        var go = Instantiate(prefab, storageContent.transform);
        var child = go.transform.GetChild(0).gameObject;

        if (trunkWeaponInfo.ContainsKey(currentIndex))
            child.GetComponent<Text>().text = trunkWeaponInfo[currentIndex].name;
        else if (trunkConsumableInfo.ContainsKey(currentIndex))
            child.GetComponent<Text>().text = trunkConsumableInfo[currentIndex].name;

        int num;
        if (storageDiscardedIndex.Count != 0)
        {
            num = storageDiscardedIndex[0];
            storageDiscardedIndex.RemoveAt(0);
        }
        else num = storageList.Count;

        var button = go.AddComponent<Button>();
        button.onClick.AddListener(delegate { SelectItem(num, InvenKind.Storage); });
        storageList.Add(num, go);

        if (trunkWeaponInfo.ContainsKey(currentIndex))
        {
            storageWeaponInfo.Add(num, trunkWeaponInfo[currentIndex]);
            trunkWeaponInfo.Remove(currentIndex);
        }
        else if (trunkConsumableInfo.ContainsKey(currentIndex))
        {
            storageConsumableInfo.Add(num, trunkConsumableInfo[currentIndex]);
            trunkConsumableInfo.Remove(currentIndex);
        }
        trunkList.Remove(currentIndex);
        trunkDiscardedIndex.Add(currentIndex);

        currentIndex = -1;
        currentInvenKind = InvenKind.None;
    }
}