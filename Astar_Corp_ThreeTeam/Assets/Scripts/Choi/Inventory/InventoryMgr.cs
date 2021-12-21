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
        //아이템 데이터.
        itemData = new Dictionary<string, ItemType>();
        //equippable
        itemData.Add("오딘의 궁니르 마법구", ItemType.Equippable);
        itemData.Add("헤임달의 마법구", ItemType.Equippable);
        itemData.Add("레바테인 마법구", ItemType.Equippable);
        //consumable
        itemData.Add("대형 HP 회복 물약", ItemType.Consumable);
        itemData.Add("중형 HP 회복 물약", ItemType.Consumable);
        itemData.Add("소형 HP 회복 물약", ItemType.Consumable);
        itemData.Add("순록 스테이크", ItemType.Consumable);
        //equippable
        itemData.Add("알브의 세계수 마법구", ItemType.Equippable);
        itemData.Add("시구르드의 그람 마법구", ItemType.Equippable);
        //consumable
        itemData.Add("벌꿀술", ItemType.Consumable);
        itemData.Add("프리낭 연어", ItemType.Consumable);
        itemData.Add("사리풀", ItemType.Consumable);
        itemData.Add("순간 이동 주문서", ItemType.Consumable);
        //equippable
        itemData.Add("지크프리트의 노퉁 마법구", ItemType.Equippable);
        itemData.Add("지크프리트의 발뭉 마법구", ItemType.Equippable);
        itemData.Add("울프베르트 마법구", ItemType.Equippable);

        //유저 소유 데이터.
        items = new Dictionary<string, int>();
        //equippable
        items.Add("오딘의 궁니르 마법구", 1);
        items.Add("헤임달의 마법구", 4);
        items.Add("레바테인 마법구", 3);
        //consumable
        items.Add("대형 HP 회복 물약", 10);
        items.Add("중형 HP 회복 물약", 24);
        items.Add("소형 HP 회복 물약", 51);
        items.Add("순록 스테이크", 21);
        //equippable
        items.Add("알브의 세계수 마법구", 2);
        items.Add("시구르드의 그람 마법구", 3);
        //consumable
        items.Add("벌꿀술", 15);
        items.Add("프리낭 연어", 37);
        items.Add("사리풀", 26);
        items.Add("순간 이동 주문서", 38);
        //equippable
        items.Add("지크프리트의 노퉁 마법구", 4);
        items.Add("지크프리트의 발뭉 마법구", 1);
        items.Add("울프베르트 마법구", 1);

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
            ButtonText.text = "장착하기";
        else if (itemData[currentItem] == ItemType.Consumable)
            ButtonText.text = "아이템 사용";
    }

    public void UseItem()
    {
        if (ButtonText.text.Equals("해제하기"))
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
        ButtonText.text = "해제하기";
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
