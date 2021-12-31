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
    List<GameObject> characterGOs;

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

    string currentItem;
    //(string, int) currentMainWeapon;
    //(string, int) currentSubWeapon;
    InvenMode currentMode;

    EquipType equipType;

    PlayerDataMgr playerDataMgr;

    // Start is called before the first frame update
    void Start()
    {
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

        int i = 0;
        foreach (var element in playerDataMgr.characterInfos)
        {
            int num = i;
            characterData.Add(num, element.Value.name);
            var go = Instantiate(characterPrefab, characterContent.transform);
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            var child = go.transform.GetChild(1).gameObject;
            child.GetComponent<Text>().text = element.Value.name;
            i++;
        }

        //������ ������.
        itemData = new Dictionary<string, ItemType>();
        foreach (var element in playerDataMgr.equippableList)
        {
            itemData.Add($"{element.Value.id}", ItemType.Equippable);
        }
        foreach (var element in playerDataMgr.consumableList)
        {
            itemData.Add($"{element.Value.id}", ItemType.Consumable);
        }


        ////equippable
        //itemData.Add("������ �ôϸ� ������", ItemType.Equippable);
        //itemData.Add("���Ӵ��� ������", ItemType.Equippable);
        //itemData.Add("�������� ������", ItemType.Equippable);
        ////consumable
        //itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        //itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        //itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        //itemData.Add("���� ������ũ", ItemType.Consumable);
        ////equippable
        //itemData.Add("�˺��� ����� ������", ItemType.Equippable);
        //itemData.Add("�ñ������� �׶� ������", ItemType.Equippable);
        ////consumable
        //itemData.Add("���ܼ�", ItemType.Consumable);
        //itemData.Add("������ ����", ItemType.Consumable);
        //itemData.Add("�縮Ǯ", ItemType.Consumable);
        //itemData.Add("���� �̵� �ֹ���", ItemType.Consumable);
        ////equippable
        //itemData.Add("��ũ����Ʈ�� ���� ������", ItemType.Equippable);
        //itemData.Add("��ũ����Ʈ�� �߹� ������", ItemType.Equippable);
        //itemData.Add("��������Ʈ ������", ItemType.Equippable);

        //���� ���� ������.
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
        ////equippable
        //items.Add("������ �ôϸ� ������", 1);
        //items.Add("���Ӵ��� ������", 4);
        //items.Add("�������� ������", 3);
        ////consumable
        //items.Add("���� HP ȸ�� ����", 10);
        //items.Add("���� HP ȸ�� ����", 24);
        //items.Add("���� HP ȸ�� ����", 51);
        //items.Add("���� ������ũ", 21);
        ////equippable
        //items.Add("�˺��� ����� ������", 2);
        //items.Add("�ñ������� �׶� ������", 3);
        ////consumable
        //items.Add("���ܼ�", 15);
        //items.Add("������ ����", 37);
        //items.Add("�縮Ǯ", 26);
        //items.Add("���� �̵� �ֹ���", 38);
        ////equippable
        //items.Add("��ũ����Ʈ�� ���� ������", 4);
        //items.Add("��ũ����Ʈ�� �߹� ������", 1);
        //items.Add("��������Ʈ ������", 1);

        itemGOs = new List<GameObject>();
        currentList = new Dictionary<int, string>();

        currentMode = InvenMode.All;
        AllDisplay();
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
        Debug.Log("ĳ���ͺ���");
        if (detailWin.activeSelf == true) detailWin.SetActive(false);
        currentCharacter = characterData[i];
        currentCharacterTxt.text = currentCharacter;

        if (playerDataMgr.characterStats[currentCharacter].mainWeapon != null)
        {
            var child = mainWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = $"+{playerDataMgr.characterInfos[currentCharacter].mainWeaponNum}";
        }
        else
        {
            var child = mainWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = null;
        }

        if (playerDataMgr.characterStats[currentCharacter].subWeapon != null)
        {
            var child = subWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = $"+{playerDataMgr.characterInfos[currentCharacter].subWeaponNum}";
        }
        else
        {
            var child = subWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
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
            mainWeaponButton.GetComponent<Text>().text = "�ֹ���";

            subWeaponButtonObj.SetActive(true);
            var subWeaponButton = subWeaponButtonObj.transform.GetChild(0).gameObject;
            subWeaponButton.GetComponent<Text>().text = "��������";
        }
        else if (itemData[currentItem] == ItemType.Consumable)
        {
            detailInfoName.text = playerDataMgr.currentConsumables[currentList[i]].name;

            mainWeaponButtonObj.SetActive(false);
            subWeaponButtonObj.SetActive(false);

            singleButtonObj.SetActive(true);
            var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
            singleButton.GetComponent<Text>().text = "������ ���";
        }
    }

    public void UseItem()
    {
        if (currentCharacter == null) return;

        var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
        if (singleButton.GetComponent<Text>().text.Equals("�����ϱ�"))
        {
            Disarm();
            var child = mainWeaponQuick.transform.GetChild(0);
            var detailInfo = child.GetComponent<Text>();
            detailInfo.text = string.Empty;
        }
        else
        {
            if (itemData[currentItem] == ItemType.Equippable)
            {
                if (equipType == EquipType.MainWeapon)
                {
                    Debug.Log("�ֹ��� ����");
                    //�̹� �������� ������ �κ��丮�� ����.
                    Disarm();

                    playerDataMgr.characterInfos[currentCharacter].mainWeapon = currentItem;
                    playerDataMgr.characterInfos[currentCharacter].mainWeaponNum = items[currentItem];
                    var equip = playerDataMgr.currentEquippables[currentItem];
                    playerDataMgr.characterStats[currentCharacter].mainWeapon = equip;

                    var child = mainWeaponQuick.transform.GetChild(0);
                    var detailInfo = child.GetComponent<Text>();
                    detailInfo.text = $"+{playerDataMgr.characterInfos[currentCharacter].mainWeaponNum}";

                    playerDataMgr.currentEquippables.Remove(currentItem);
                    playerDataMgr.currentEquippablesNum.Remove(currentItem);

                    var index = playerDataMgr.characterInfos[currentCharacter].saveId;
                    playerDataMgr.saveData.mainWeapon[index] = currentItem;
                    playerDataMgr.saveData.mainWeaponNum[index] = items[currentItem];
                    index = playerDataMgr.saveData.equippableList.IndexOf(currentItem);
                    playerDataMgr.saveData.equippableList.Remove(currentItem);
                    playerDataMgr.saveData.equippableNumList.RemoveAt(index);
                    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                    items.Remove(currentItem);
                }
                else if (equipType == EquipType.SubWeapon)
                {
                    Debug.Log("�������� ����");
                    Disarm();

                    playerDataMgr.characterInfos[currentCharacter].subWeapon = currentItem;
                    playerDataMgr.characterInfos[currentCharacter].subWeaponNum = items[currentItem];
                    var equip = playerDataMgr.currentEquippables[currentItem];
                    playerDataMgr.characterStats[currentCharacter].subWeapon = equip;

                    var child = subWeaponQuick.transform.GetChild(0);
                    var detailInfo = child.GetComponent<Text>();
                    detailInfo.text = $"+{playerDataMgr.characterInfos[currentCharacter].subWeaponNum}";

                    playerDataMgr.currentEquippables.Remove(currentItem);
                    playerDataMgr.currentEquippablesNum.Remove(currentItem);

                    var index = playerDataMgr.characterInfos[currentCharacter].saveId;
                    playerDataMgr.saveData.subWeapon[index] = currentItem;
                    playerDataMgr.saveData.subWeaponNum[index] = items[currentItem];
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
        else if (playerDataMgr.characterStats[currentCharacter].mainWeapon == null && equipType == EquipType.MainWeapon) return;
        else if (playerDataMgr.characterStats[currentCharacter].subWeapon == null && equipType == EquipType.SubWeapon) return;

        string name;
        switch (equipType)
        {
            case EquipType.MainWeapon:
                name = playerDataMgr.characterStats[currentCharacter].mainWeapon.name;
                detailInfoName.text = name;
                break;
            case EquipType.SubWeapon:
                name = playerDataMgr.characterStats[currentCharacter].subWeapon.name;
                detailInfoName.text = name;
                break;
        }

        detailWin.SetActive(true);

        mainWeaponButtonObj.SetActive(false);
        subWeaponButtonObj.SetActive(false);
        singleButtonObj.SetActive(true);
        var singleButton = singleButtonObj.transform.GetChild(0).gameObject;
        singleButton.GetComponent<Text>().text = "�����ϱ�";
    }

    public void Disarm()
    {
        if (playerDataMgr.characterInfos[currentCharacter].mainWeapon != null && equipType == EquipType.MainWeapon)
        {
            var id = playerDataMgr.characterInfos[currentCharacter].mainWeapon;
            var num = playerDataMgr.characterInfos[currentCharacter].mainWeaponNum;

            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(num);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.characterInfos[currentCharacter].mainWeapon = null;
            playerDataMgr.characterInfos[currentCharacter].mainWeaponNum = 0;
            playerDataMgr.characterStats[currentCharacter].mainWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, num);

            items.Add(id, num);
        }
        else if (playerDataMgr.characterInfos[currentCharacter].subWeapon != null && equipType == EquipType.SubWeapon)
        {
            var id = playerDataMgr.characterInfos[currentCharacter].subWeapon;
            var num = playerDataMgr.characterInfos[currentCharacter].subWeaponNum;

            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(num);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.characterInfos[currentCharacter].subWeapon = null;
            playerDataMgr.characterInfos[currentCharacter].subWeaponNum = 0;
            playerDataMgr.characterStats[currentCharacter].subWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, num);

            items.Add(id, num);

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
