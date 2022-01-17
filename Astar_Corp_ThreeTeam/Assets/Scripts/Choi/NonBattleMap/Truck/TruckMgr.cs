using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WarehouseKind
{ 
    None,
    Trunk,
    Bag
}
public class TruckMgr : MonoBehaviour
{
    [Header("Truck Squad")]
    public GameObject TruckWin;
    public GameObject Contents;
    public GameObject TruckUnitPrefab;
    public Text RemainingNum;
    [HideInInspector]
    public Dictionary<int, GameObject> TruckUnitGOs = new Dictionary<int, GameObject>();

    [Header("Selected Characters")]
    public GameObject selectedCharPrefab;
    public GameObject selectedChars;
    public GameObject truckEquips;
    public GameObject truckConsums;
    private TextMeshProUGUI charInfoTxt;

    [Header("Truck Inventory")]
    public GameObject equipTruckItem;
    public GameObject consumTruckItem;

    [Header("Character Inventory")]
    public GameObject SquadItemPrefab;
    public GameObject char1Inventory;
    public GameObject char2Inventory;
    public GameObject char3Inventory;
    public GameObject char4Inventory;

    //����.
    [Header("�˾�â ����")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemNumTxt;
    public Slider slider;
    public GameObject mainWeaponButton;
    public GameObject subWeaponButton;
    public GameObject bagButton;
    public GameObject truckButton;

    public GameObject truckContents;
    public GameObject truckPrefab;
    public GameObject bagContents;
    public GameObject bagPrefab;
    public Text mainWeaponTxt;
    public Text subWeaponTxt;

    Dictionary<string, GameObject> truckObjs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> bagWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> bagWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();

    int currentIndex;
    string currentKey;
    WarehouseKind currentKind;
  
    PlayerDataMgr playerDataMgr;
    
    void Start()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        Init();
    }

    public void Init()
    {
        if (playerDataMgr == null) playerDataMgr = PlayerDataMgr.Instance;
        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();

        if (truckWeaponInfo.Count != 0) truckWeaponInfo.Clear();
        if (truckWeaponNumInfo.Count != 0) truckWeaponNumInfo.Clear();
        if (truckConsumableInfo.Count != 0) truckConsumableInfo.Clear();
        if (truckConsumableNumInfo.Count != 0) truckConsumableNumInfo.Clear();

        if (bagWeaponInfo.Count != 0) bagWeaponInfo.Clear();
        if (bagWeaponNumInfo.Count != 0) bagWeaponNumInfo.Clear();
        if (bagConsumableInfo.Count != 0) bagConsumableInfo.Clear();
        if (bagConsumableNumInfo.Count != 0) bagConsumableNumInfo.Clear();

        if (TruckUnitGOs.Count != 0)
        {
            foreach (var element in TruckUnitGOs)
            {
                Destroy(element.Value);
            }
            TruckUnitGOs.Clear();

            Contents.transform.DetachChildren();
        }

        // CurrentSquad Setting
        foreach (var element in playerDataMgr.boardingSquad)
        {
            var go = Instantiate(TruckUnitPrefab, Contents.transform);
            var name = go.transform.GetChild(1).GetComponent<Text>();
            name.text = playerDataMgr.currentSquad[element.Value].character.name;

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectBattleCharacter(element.Value); });

            TruckUnitGOs.Add(element.Value, go);
        }

        mainWeaponTxt.text = "�������";
        subWeaponTxt.text = "�������";
    }

    public void Open()
    {
        TruckWin.SetActive(true);
        Init();

        // MemberSelectPopup â���� ��� ĳ���� ���� �ؽ�Ʈ
        // �ش� �˾�â�� Open �ɶ� Find
        charInfoTxt = GameObject.Find("CharacterInfo").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Close()
    {
        TruckWin.SetActive(false);
    }

    // ���� ���� ������ ĳ���� ����
    void SelectBattleCharacter(int num)
    {
        //�ߺ��� ��.
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            playerDataMgr.battleSquad.Remove(num);
            var go = TruckUnitGOs[num];
            var child = go.transform.GetChild(0).GetComponent<Image>();
            child.color = Color.white;

            RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
            return;
        }

        //����á����.
        if (playerDataMgr.battleSquad.Count == 4) return;

        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        var selectedGo = TruckUnitGOs[num];
        var selectedChild = selectedGo.transform.GetChild(0).GetComponent<Image>();
        selectedChild.color = Color.red;

        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();

        //Ŭ�� �� ���� ĳ���� ���� ǥ��
        charInfoTxt.text = playerDataMgr.battleSquad[num].Name + "\n" +
            "Hp : " + playerDataMgr.battleSquad[num].currentHp;
    }

    private bool isItemInit;

    // Ʈ��UI ���� (�ʱ�ȭ)
    public void TruckUISetting()
    {
        // BattleSquad Setting
        foreach (var element in playerDataMgr.battleSquad)
        {
            var go = Instantiate(selectedCharPrefab, selectedChars.transform);
            var goName = go.GetComponentInChildren<TextMeshProUGUI>();
            goName.text = element.Value.character.name;

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharInventory(element.Key); });
        }

        if (!isItemInit)
        {
            foreach (var element in playerDataMgr.truckEquippables)
            {
                truckWeaponInfo.Add(element.Key, element.Value);
                truckWeaponNumInfo.Add(element.Key, playerDataMgr.truckEquippablesNum[element.Key]);
            }

            foreach (var element in playerDataMgr.truckConsumables)
            {
                truckConsumableInfo.Add(element.Key, element.Value);
                truckConsumableNumInfo.Add(element.Key, playerDataMgr.truckConsumablesNum[element.Key]);
            }

            DisplayEquippables();
            isItemInit = true;
        }
    }

    //// Ʈ�� ������ ī�װ� ���� (��� vs �Һ�)
    //public void SelectItemType(int value)
    //{
    //    equipTruckItem.SetActive(true);

    //    if (value == 0)
    //    {
    //        if (consumTruckItem.activeInHierarchy)
    //            consumTruckItem.SetActive(false);

    //        equipTruckItem.SetActive(true);
    //    }
    //    else if (value == 1)
    //    {
    //        if (equipTruckItem.activeInHierarchy)
    //            equipTruckItem.SetActive(false);

    //        consumTruckItem.SetActive(true);
    //    }
    //}

    public void DisplayEquippables()
    {
        if (truckObjs.Count != 0)
        {
            foreach (var element in truckObjs)
            {
                Destroy(element.Value);
            }
            truckObjs.Clear();
            truckContents.transform.DetachChildren();
        }

        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(truckPrefab, truckContents.transform);
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

            truckObjs.Add(element.Key, go);
        }
    }

    public void DisplayConsumables()
    {
        if (truckObjs.Count != 0)
        {
            foreach (var element in truckObjs)
            {
                Destroy(element.Value);
            }
            truckObjs.Clear();
            truckContents.transform.DetachChildren();
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(truckPrefab, truckContents.transform);
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

            truckObjs.Add(element.Key, go);
        }
    }

    public void NumAdjustment()
    {
        itemNumTxt.text = $"{slider.value}��";
    }

    public void SelectItem(string key, WarehouseKind kind)
    {
        if (currentKey != null)
        {
            if (kind == WarehouseKind.Trunk)
            {
                var image = truckObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
            else if (kind == WarehouseKind.Bag)
            {
                var image = bagObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }

        currentKey = key;
        currentKind = kind;

        if (currentKind == WarehouseKind.Trunk)
        {
            var image = truckObjs[currentKey].GetComponent<Image>();
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
        else if (currentKind == WarehouseKind.Bag)
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
        itemNumTxt.text = $"0��";
        OpenPopup();
    }

    public void Move()
    {
        if (currentKind == WarehouseKind.Trunk)
            MoveToBag(Mathf.FloorToInt(slider.value));
        else if (currentKind == WarehouseKind.Bag)
            MoveToTruck(Mathf.FloorToInt(slider.value));

        ClosePopup();
    }

    public void AddSliderValue(int plus)
    {
        if (slider.value + plus >= slider.maxValue)
        {
            slider.value = slider.maxValue;
            itemNumTxt.text = $"{slider.value}��";
        }
        else
        {
            slider.value += plus;
            itemNumTxt.text = $"{slider.value}��";
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

    private bool isFull;

    // ���� �κ��丮 ���
    private void SelectCharInventory(int key)
    {
        if (currentIndex == key) return;

        if (bagObjs.Count != 0)
        {
            foreach (var element in bagObjs)
            {
                Destroy(element.Value);
            }
            bagObjs.Clear();
            bagContents.transform.DetachChildren();
        }

        currentIndex = key;

        // �ӽ÷�
        var charName = GameObject.Find("Char Name").GetComponent<TextMeshProUGUI>();
        charName.text = playerDataMgr.battleSquad[key].character.name;

        foreach (var element in playerDataMgr.battleSquad[key].bag)
        {
            if (playerDataMgr.equippableList.ContainsKey(element.Key))
            {
                var go = Instantiate(bagPrefab, bagContents.transform);
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Bag); });

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{element.Value}��";

                bagObjs.Add(element.Key, go);
                bagWeaponInfo.Add(element.Key, playerDataMgr.equippableList[element.Key]);
                //bagWeaponNumInfo
            }
            if (playerDataMgr.consumableList.ContainsKey(element.Key))
            {
                var go = Instantiate(bagPrefab, bagContents.transform);
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Bag); });

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{element.Value}��";

                bagObjs.Add(element.Key, go);
            }

            isFull = true;
        }
    }

    public void MoveToBag(int itemNum)
    {
        if (currentKey == null) return;
        if (itemNum == 0) return;
        OpenPopup();

        if (truckWeaponInfo.ContainsKey(currentKey))
        {
            //json.
            int index = 0;
            var id = truckWeaponInfo[currentKey].id;

            index = currentIndex;

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
                    playerDataMgr.saveData.bagEquippableFirstIndex[i]++;
                    playerDataMgr.saveData.bagEquippableLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagEquippableNumList[containIndex] += itemNum;
            }

            index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
            if (playerDataMgr.saveData.truckEquippableNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.truckEquippableList.Remove(id);
                playerDataMgr.saveData.truckEquippableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckEquippableNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //���絥���� ����.
                bagWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
                bagWeaponNumInfo.Add(currentKey, itemNum);

                var go = Instantiate(bagPrefab, bagContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Bag); });
                bagObjs.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //���絥���� ����.
                bagWeaponNumInfo[currentKey] += itemNum;
                var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.truckEquippablesNum[id] - itemNum == 0)
            {
                //���� ������.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                Destroy(truckObjs[currentKey]);
                truckObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //���� ������.
                truckWeaponNumInfo[currentKey] -= itemNum;
                var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippablesNum[id] -= itemNum;
            }
        }
        else if (truckConsumableInfo.ContainsKey(currentKey))
        {
            //json.
            int index = 0;
            var id = truckConsumableInfo[currentKey].id;

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
                    playerDataMgr.saveData.bagConsumableFirstIndex[i]++;
                    playerDataMgr.saveData.bagConsumableLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagConsumableNumList[containIndex] += itemNum;
            }

            index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
            if (playerDataMgr.saveData.truckConsumableNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.truckConsumableList.Remove(id);
                playerDataMgr.saveData.truckConsumableNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckConsumableNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //���絥���� ����.
                bagConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
                bagConsumableNumInfo.Add(currentKey, itemNum);

                var go = Instantiate(bagPrefab, bagContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Bag); });
                bagObjs.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //���絥���� ����.
                bagConsumableNumInfo[currentKey] += itemNum;
                var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.truckConsumablesNum[id] - itemNum == 0)
            {
                //���� ������.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);
                Destroy(truckObjs[currentKey]);
                truckObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumables.Remove(id);
                playerDataMgr.truckConsumablesNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //���� ������.
                truckConsumableNumInfo[currentKey] -= itemNum;
                var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumablesNum[id] -= itemNum;
            }
        }
    }

    public void MoveToTruck(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (bagWeaponInfo.ContainsKey(currentKey))
        {
            //json.
            int index = 0;
            var id = bagWeaponInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckEquippableList.Contains(id))
            {
                playerDataMgr.saveData.truckEquippableList.Add(id);
                playerDataMgr.saveData.truckEquippableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
                playerDataMgr.saveData.truckEquippableNumList[index] += itemNum;
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
                    playerDataMgr.saveData.bagEquippableFirstIndex[i]--;
                    playerDataMgr.saveData.bagEquippableLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagEquippableNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckEquippables.ContainsKey(id))
            {
                //���絥���� ����.
                truckWeaponInfo.Add(currentKey, bagWeaponInfo[currentKey]);
                truckWeaponNumInfo.Add(currentKey, itemNum);

                var go = Instantiate(truckPrefab, truckContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = bagWeaponInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                truckObjs.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //���絥���� ����.
                truckWeaponNumInfo[currentKey] += itemNum;
                var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckEquippablesNum[id] += itemNum;
            }

            if (bagWeaponNumInfo[id] - itemNum == 0)
            {
                //���� ������.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);
                Destroy(bagObjs[currentKey]);
                bagObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //���� ������.
                bagWeaponNumInfo[currentKey] -= itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
        }
        else if (bagConsumableInfo.ContainsKey(currentKey))
        {
            //json.
            int index;
            var id = bagConsumableInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckConsumableList.Contains(id))
            {
                playerDataMgr.saveData.truckConsumableList.Add(id);
                playerDataMgr.saveData.truckConsumableNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
                playerDataMgr.saveData.truckConsumableNumList[index] += itemNum;
            }

            index = currentIndex;
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
                    playerDataMgr.saveData.bagConsumableFirstIndex[i]--;
                    playerDataMgr.saveData.bagConsumableLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagConsumableNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckConsumables.ContainsKey(id))
            {
                //���絥���� ����.
                truckConsumableInfo.Add(currentKey, bagConsumableInfo[currentKey]);
                truckConsumableNumInfo.Add(currentKey, itemNum);

                var go = Instantiate(truckPrefab, truckContents.transform);
                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = bagConsumableInfo[currentKey].name;

                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{itemNum}��";

                var button = go.AddComponent<Button>();
                string selectedKey = currentKey;
                button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                truckObjs.Add(selectedKey, go);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //���絥���� ����.
                truckConsumableNumInfo[currentKey] += itemNum;
                var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.truckConsumablesNum[id] += itemNum;
            }

            if (bagConsumableNumInfo[id] - itemNum == 0)
            {
                //���� ������.
                bagConsumableInfo.Remove(currentKey);
                bagConsumableNumInfo.Remove(currentKey);
                Destroy(bagObjs[currentKey]);
                bagObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //���� ������.
                bagConsumableNumInfo[currentKey] -= itemNum;
                var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}��";

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
        }
    }

    //â ����.
    public void OpenPopup()
    {
        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        if (popupWin.activeSelf) popupWin.SetActive(false);

        if (currentKind == WarehouseKind.Trunk)
        {
            var image = truckObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }
        else if (currentKind == WarehouseKind.Bag)
        {
            var image = bagObjs[currentKey].GetComponent<Image>();
            image.color = Color.white;
        }

        currentKind = WarehouseKind.None;
        currentKey = null;
    }
}
