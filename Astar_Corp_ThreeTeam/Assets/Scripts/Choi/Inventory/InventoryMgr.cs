using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum InvenMode
{
    All,
    Equippable,
    Consumable
}

public enum ItemType
{
    Equippable,
    Consumable
}

public enum EquipType
{
    MainWeapon,
    SubWeapon
}

public class InventoryMgr : MonoBehaviour
{
    public GameObject inventoryContent;
    public GameObject inventoryItemPrefab;

    public GameObject characterContent;
    public GameObject characterPrefab;
    public Text currentCharacterTxt;

    string currentCharacter;
    Dictionary<int, string> characterData = new Dictionary<int, string>();
    List<GameObject> characterGOs = new List<GameObject>();
    int totalSquadNum;

    //DetailInfo.
    public GameObject detailWin;
    public Text detailInfoName;
    public GameObject mainWeaponButtonObj;
    public GameObject subWeaponButtonObj;
    public GameObject singleButtonObj;
    public GameObject mainWeaponQuick;
    public GameObject subWeaponQuick;

    Dictionary<string, ItemType> itemData;
    Dictionary<string, int> items;
    List<GameObject> itemGOs;
    Dictionary<int, string> currentList;

    int currentIndex;
    string currentItem;
    InvenMode currentMode;

    EquipType equipType;

    public PlayerDataMgr playerDataMgr;

    public void Init()
    {
        //아이템 데이터.
        itemData = new Dictionary<string, ItemType>();
        foreach (var element in playerDataMgr.equippableList)
        {
            itemData.Add($"{element.Value.id}", ItemType.Equippable);
        }
        foreach (var element in playerDataMgr.consumableList)
        {
            itemData.Add($"{element.Value.id}", ItemType.Consumable);
        }

        //유저 소유 데이터.
        items = new Dictionary<string, int>();
        int k = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            items.Add($"{element.Value.id}", playerDataMgr.currentEquippablesNum[element.Key]);
            k++;
        }
        k = 0;
        foreach (var element in playerDataMgr.currentConsumables)
        {
            items.Add($"{element.Value.id}", playerDataMgr.currentConsumablesNum[element.Key]);
            k++;
        }

        itemGOs = new List<GameObject>();
        currentList = new Dictionary<int, string>();

        currentMode = InvenMode.All;
        AllDisplay();
    }

    public void Refresh()
    {
        if (playerDataMgr == null)
        {
            var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
            playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        }

        var currentSquad = playerDataMgr.currentSquad;

        if (characterData.Count != 0) characterData.Clear();
        if (characterGOs.Count != 0)
        {
            foreach (var element in characterGOs)
            {
                Destroy(element);
            }

            characterGOs.Clear();
        }
        characterContent.transform.DetachChildren();

        int j = 0;
        foreach (var element in currentSquad)
        {
            var go = Instantiate(characterPrefab, characterContent.transform);
            var button = go.AddComponent<Button>();
            int num = j;
            button.onClick.AddListener(delegate { SelectCharacter(element.Key); });
            characterGOs.Add(go);

            if (element.Value.character.name == string.Empty) characterData.Add(element.Key, null);
            else characterData.Add(element.Key, element.Value.character.name);

            j++;
        }

        Debug.Log($"characterContent.count : {characterContent.transform.childCount}");
        for (int i = 0 ; i < currentSquad.Count; i++)
        {
            var child = characterContent.transform.GetChild(i).gameObject;
            var squadName = child.transform.GetChild(1).gameObject.GetComponent<Text>();
            Debug.Log($"i : {i}");
            if (currentSquad[i].character.name == string.Empty) squadName.text = string.Empty;
            else squadName.text = currentSquad[i].character.name;
        }
    }

    public void AllDisplay()
    {
        Clear();
        detailWin.SetActive(false);

        int i = 0;
        foreach (var element in items)
        {
            var go = Instantiate(inventoryItemPrefab, inventoryContent.transform);
            var child = go.transform.GetChild(1);
            var detailInfo = child.GetComponent<Text>();
            if (itemData[element.Key] == ItemType.Equippable)
                detailInfo.text = $"+{element.Value}";
            else if (itemData[element.Key] == ItemType.Consumable)
                detailInfo.text = $"{element.Value}";

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { DetailInfo(num); });

            itemGOs.Add(go);

            currentList.Add(num, element.Key);
            i++;
        }
        currentMode = InvenMode.All;
    }

    public void EquippableDisplay()
    {
        Clear();
        detailWin.SetActive(false);

        int i = 0;
        foreach (var element in items)
        {
            if (itemData[element.Key] == ItemType.Consumable) continue;

            var go = Instantiate(inventoryItemPrefab, inventoryContent.transform);
            var child = go.transform.GetChild(1);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = $"+{element.Value}";

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { DetailInfo(num); });

            itemGOs.Add(go);

            currentList.Add(num, element.Key);
            i++;
        }
        currentMode = InvenMode.Equippable;
    }

    public void ConsumableDisplay()
    {
        Clear();
        detailWin.SetActive(false);

        int i = 0;
        foreach (var element in items)
        {
            if (itemData[element.Key] == ItemType.Equippable) continue;

            var go = Instantiate(inventoryItemPrefab, inventoryContent.transform);
            var child = go.transform.GetChild(1);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = $"{element.Value}";

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { DetailInfo(num); });

            itemGOs.Add(go);
            currentList.Add(num, element.Key);
            i++;
        }
        currentMode = InvenMode.Consumable;
    }

    public void Clear()
    {
        foreach (var element in itemGOs)
        {
            Destroy(element);
        }
        itemGOs.Clear();

        currentList.Clear();
    }

    public void SelectCharacter(int i)
    {
        if (detailWin.activeSelf == true) detailWin.SetActive(false);
        currentCharacter = characterData[i];
        currentIndex = i;

        if (currentCharacter != null)
        {
            currentCharacterTxt.text = currentCharacter;

            if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
            {
                var child = mainWeaponQuick.transform.GetChild(0);
                var detailInfo = child.GetComponent<Text>();
                detailInfo.text = $"+1";
            }
            else
            {
                var child = mainWeaponQuick.transform.GetChild(0);
                var detailInfo = child.GetComponent<Text>();
                detailInfo.text = null;
            }

            if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
            {
                var child = subWeaponQuick.transform.GetChild(0);
                var detailInfo = child.GetComponent<Text>();
                detailInfo.text = $"+1";
            }
            else
            {
                var child = subWeaponQuick.transform.GetChild(0);
                var detailInfo = child.GetComponent<Text>();
                detailInfo.text = null;
            }
        }
        else
        {
            currentCharacterTxt.text = "비어있음";

            var child = mainWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = null;

            child = subWeaponQuick.transform.GetChild(0);
            detailInfo = child.GetComponent<Text>();
            detailInfo.text = null;
        }
    }

    public void DetailInfo(int i)
    {
        detailWin.SetActive(true);

        currentItem = currentList[i];
       
        if (itemData[currentItem] == ItemType.Equippable)
        {
            detailInfoName.text = playerDataMgr.currentEquippables[currentList[i]].name;

            singleButtonObj.SetActive(false);

            mainWeaponButtonObj.SetActive(true);
            var mainWeaponButton = mainWeaponButtonObj.transform.GetChild(0).gameObject;
            mainWeaponButton.GetComponent<Text>().text = "주무기";

            subWeaponButtonObj.SetActive(true);
            var subWeaponButton = subWeaponButtonObj.transform.GetChild(0).gameObject;
            subWeaponButton.GetComponent<Text>().text = "보조무기";
        }
        else if (itemData[currentItem] == ItemType.Consumable)
        {
            detailInfoName.text = playerDataMgr.currentConsumables[currentList[i]].name;

            mainWeaponButtonObj.SetActive(false);
            subWeaponButtonObj.SetActive(false);

            singleButtonObj.SetActive(true);
            var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
            singleButton.GetComponent<Text>().text = "아이템 사용";
        }
    }

    public void UseItem()
    {
        Debug.Log("1");
        if (currentCharacter == null) return;
        Debug.Log("2");
        var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
        if (singleButton.GetComponent<Text>().text.Equals("해제하기"))
        {
            Debug.Log("해제하기");
            Disarm();
            var child = mainWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = string.Empty;

            singleButton.GetComponent<Text>().text = string.Empty;
        }
        else
        {
            Debug.Log("장착하기");
            Debug.Log($"currentItem : {currentItem}");

            if (itemData[currentItem] == ItemType.Equippable)
            {
                if (equipType == EquipType.MainWeapon)
                {
                    Debug.Log("주무기 장착");
                    //이미 아이템이 있으면 인벤토리에 넣음.
                    Disarm();

                    playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = playerDataMgr.equippableList[currentItem];
                    //playerDataMgr.characterStats[currentIndex].mainWeaponNum = items[currentItem];
                    var equip = playerDataMgr.currentEquippables[currentItem];
                    playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = equip;

                    var child = mainWeaponQuick.transform.GetChild(0);
                    var detailInfo = child.GetComponent<Text>();
                    detailInfo.text = $"+1";

                    playerDataMgr.currentEquippables.Remove(currentItem);
                    playerDataMgr.currentEquippablesNum.Remove(currentItem);

                    var index =currentIndex;
                    playerDataMgr.saveData.mainWeapon[index] = currentItem;
                    index = playerDataMgr.saveData.equippableList.IndexOf(currentItem);
                    playerDataMgr.saveData.equippableList.Remove(currentItem);
                    playerDataMgr.saveData.equippableNumList.RemoveAt(index);
                    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                    items.Remove(currentItem);
                }
                else if (equipType == EquipType.SubWeapon)
                {
                    Debug.Log("보조무기 장착");
                    Disarm();

                    playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = playerDataMgr.equippableList[currentItem];
                    //playerDataMgr.characterStats[currentIndex].subWeaponNum = items[currentItem];
                    var equip = playerDataMgr.currentEquippables[currentItem];
                    playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = equip;

                    var child = subWeaponQuick.transform.GetChild(0);
                    var detailInfo = child.GetComponent<Text>();
                    detailInfo.text = $"+1";

                    playerDataMgr.currentEquippables.Remove(currentItem);
                    playerDataMgr.currentEquippablesNum.Remove(currentItem);

                    var index = currentIndex;
                    playerDataMgr.saveData.subWeapon[index] = currentItem;
                    index = playerDataMgr.saveData.equippableList.IndexOf(currentItem);
                    playerDataMgr.saveData.equippableList.Remove(currentItem);
                    playerDataMgr.saveData.equippableNumList.RemoveAt(index);
                    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                    items.Remove(currentItem);
                }
            }
            else if (itemData[currentItem] == ItemType.Consumable)
            {
                //items[currentItem]--;
                //if (items[currentItem] == 0)
                //{
                //    items.Remove(currentItem);
                //    currentItem = null;
                //}
            }
        }

        ListUpdate();
        detailWin.SetActive(false);
    }

    public void EquipMainWeapon()
    {
        equipType = EquipType.MainWeapon;
    }

    public void EquipSubWeapon()
    {
        equipType = EquipType.SubWeapon;
    }

    public void EquipQuickDisplay()
    {
        if (currentCharacter == null) return;
        else if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon == null && equipType == EquipType.MainWeapon) return;
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon == null && equipType == EquipType.SubWeapon) return;

        string name;
        switch (equipType)
        {
            case EquipType.MainWeapon:
                name = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name;
                detailInfoName.text = name;
                break;
            case EquipType.SubWeapon:
                name = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name;
                detailInfoName.text = name;
                break;
        }

        detailWin.SetActive(true);

        mainWeaponButtonObj.SetActive(false);
        subWeaponButtonObj.SetActive(false);
        singleButtonObj.SetActive(true);
        var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
        singleButton.GetComponent<Text>().text = "해제하기";
    }

    public void Disarm()
    {
        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null && equipType == EquipType.MainWeapon)
        {
            var id = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.id;
            //var num = playerDataMgr.characterStats[currentIndex].weapon.mainWeaponNum;

            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(1);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, 1);

            items.Add(id, 1);
        }
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null && equipType == EquipType.SubWeapon)
        {
            var id = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.id;
            //var num = playerDataMgr.characterStats[currentIndex].subWeaponNum;

            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(1);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, 1);

            items.Add(id, 1);
        }
    }

    public void Sort()
    {
        var sortDic = items.OrderBy(x => itemData[x.Key]).ThenBy(x => x.Key);
        Dictionary<string, int> sortedDic = new Dictionary<string, int>();
        foreach (var element in sortDic)
        {
            sortedDic.Add(element.Key, element.Value);
        }
        items.Clear();
        items = sortedDic;

        ListUpdate();
    }

    void ListUpdate()
    {
        switch (currentMode)
        {
            case InvenMode.All:
                AllDisplay();
                break;
            case InvenMode.Equippable:
                EquippableDisplay();
                break;
            case InvenMode.Consumable:
                ConsumableDisplay();
                break;
        }
    }
}
