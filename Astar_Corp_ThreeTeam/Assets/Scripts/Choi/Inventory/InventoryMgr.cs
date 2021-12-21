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

public class InventoryMgr : MonoBehaviour
{
   
    public GameObject InventoryContent;
    public GameObject inventoryItemPrefab;

    //DetailInfo.
    public GameObject detailWin;
    public Text detailInfoName;
    public Text ButtonText;

    public GameObject equipQuick;

    Dictionary<string, ItemType> itemData;
    Dictionary<string, int> items;
    List<GameObject> itemGOs;
    Dictionary<int, string> currentList;

    string currentItem;
    (string,int) currentEquip;
    InvenMode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        //������ ������.
        itemData = new Dictionary<string, ItemType>();
        //equippable
        itemData.Add("������ �ôϸ� ������", ItemType.Equippable);
        itemData.Add("���Ӵ��� ������", ItemType.Equippable);
        itemData.Add("�������� ������", ItemType.Equippable);
        //consumable
        itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        itemData.Add("���� HP ȸ�� ����", ItemType.Consumable);
        itemData.Add("���� ������ũ", ItemType.Consumable);
        //equippable
        itemData.Add("�˺��� ����� ������", ItemType.Equippable);
        itemData.Add("�ñ������� �׶� ������", ItemType.Equippable);
        //consumable
        itemData.Add("���ܼ�", ItemType.Consumable);
        itemData.Add("������ ����", ItemType.Consumable);
        itemData.Add("�縮Ǯ", ItemType.Consumable);
        itemData.Add("���� �̵� �ֹ���", ItemType.Consumable);
        //equippable
        itemData.Add("��ũ����Ʈ�� ���� ������", ItemType.Equippable);
        itemData.Add("��ũ����Ʈ�� �߹� ������", ItemType.Equippable);
        itemData.Add("��������Ʈ ������", ItemType.Equippable);

        //���� ���� ������.
        items = new Dictionary<string, int>();
        //equippable
        items.Add("������ �ôϸ� ������", 1);
        items.Add("���Ӵ��� ������", 4);
        items.Add("�������� ������", 3);
        //consumable
        items.Add("���� HP ȸ�� ����", 10);
        items.Add("���� HP ȸ�� ����", 24);
        items.Add("���� HP ȸ�� ����", 51);
        items.Add("���� ������ũ", 21);
        //equippable
        items.Add("�˺��� ����� ������", 2);
        items.Add("�ñ������� �׶� ������", 3);
        //consumable
        items.Add("���ܼ�", 15);
        items.Add("������ ����", 37);
        items.Add("�縮Ǯ", 26);
        items.Add("���� �̵� �ֹ���", 38);
        //equippable
        items.Add("��ũ����Ʈ�� ���� ������", 4);
        items.Add("��ũ����Ʈ�� �߹� ������", 1);
        items.Add("��������Ʈ ������", 1);

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
            var go = Instantiate(inventoryItemPrefab, InventoryContent.transform);
            var child = go.transform.GetChild(0);
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

            var go = Instantiate(inventoryItemPrefab, InventoryContent.transform);
            var child = go.transform.GetChild(0);
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

            var go = Instantiate(inventoryItemPrefab, InventoryContent.transform);
            var child = go.transform.GetChild(0);
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

    public void DetailInfo(int i)
    {
        detailWin.SetActive(true);

        detailInfoName.text = currentList[i];
        currentItem = currentList[i];

        if (itemData[currentItem] == ItemType.Equippable)
            ButtonText.text = "�����ϱ�";
        else if (itemData[currentItem] == ItemType.Consumable)
            ButtonText.text = "������ ���";
    }

    public void UseItem()
    {
        if (ButtonText.text.Equals("�����ϱ�"))
        {
            Disarm();
        }
        else
        {
            if (itemData[currentItem] == ItemType.Equippable)
            {
                if (currentEquip.Item1 != null)
                {
                    items.Add(currentEquip.Item1, currentEquip.Item2);
                }

                var child = equipQuick.transform.GetChild(0);
                var detailInfo = child.GetComponent<Text>();
                detailInfo.text = $"+{items[currentItem]}";

                currentEquip.Item1 = currentItem;
                currentEquip.Item2 = items[currentItem];

                items.Remove(currentItem);
            }
            else if (itemData[currentItem] == ItemType.Consumable)
            {
                items[currentItem]--;
                if (items[currentItem] == 0)
                {
                    items.Remove(currentItem);
                    currentItem = null;
                }
            }
        }

        ListUpdate();
        detailWin.SetActive(false);
    }

    public void EquipQuickDisplay()
    {
        if (currentEquip.Item1 == null) return;
        detailInfoName.text = currentEquip.Item1;
       
        detailWin.SetActive(true);
        ButtonText.text = "�����ϱ�";
    }

    public void Disarm()
    {
        items.Add(currentEquip.Item1, currentEquip.Item2);
        currentEquip = (null, -1);

        var child = equipQuick.transform.GetChild(0);
        var detailInfo = child.GetComponent<Text>();
        detailInfo.text = string.Empty;
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
