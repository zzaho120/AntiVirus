using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrunkWinMgr : ModalWindowManager
{
    // Managers
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    // Truck Items
    [Header("Truck Inventorys")]
    public GameObject truckItemPrefab;
    public GameObject truckItemList;

    // Truck Members
    [Header("Character Inventorys")]
    public GameObject truckMemberPrefab;
    public GameObject truckMemberList;

    public GameObject itemImg;
    string currentKey;
    int currentIndex;

    // Weight
    public TextMeshProUGUI totalWeight;
    private int trunkCurrentWeight;
    int bagCurrentWeight;

    private bool isInit;
    public bool isBattlePopup;

    public Dictionary<string, GameObject> truckItemObj = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> bagObjs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> truckMemberGo = new Dictionary<int, GameObject>();

    //// Truck item Data
    //Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    //Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    //Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    //Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    //Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    //Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();

    // Character item Data
    //Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    //Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();
    
    public void Init()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        if (!isInit)
        {
            foreach (var element in playerDataMgr.currentSquad)
            {
                if (playerDataMgr.currentSquad[element.Key].bag.Count > 0)
                    playerDataMgr.currentSquad[element.Key].bag.Clear();
            }
            isInit = true;
        }

        trunkCurrentWeight = 0;

        // Ʈ�� �κ��丮 ���� ���
        //if (!isInit)
        //{
        foreach (var element in playerDataMgr.truckEquippables)
        {
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
        }
        foreach (var element in playerDataMgr.truckConsumables)
        {
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
        }
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);
        }

        //// Update Trunk weight
        NonBattleMgr.Instance.worldUIMgr.printTruckUI.truckCurWeight = trunkCurrentWeight;
        NonBattleMgr.Instance.worldUIMgr.printTruckUI.Init();

        PrintTrunkItems();
    }

    public void PrintTrunkItems()
    {
        trunkCurrentWeight = 0;

        // ���� ���� ����
        if (truckItemObj.Count != 0)
        {
            foreach (var element in truckItemObj)
            {
                Destroy(element.Value);
            }
            truckItemObj.Clear();
        }

        // ����
        // 1. ����
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);

            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }
        // �Ҹ�ǰ
        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);

            if (isBattlePopup)
            {
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key); });
            }

            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckConsumablesNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }
        // 3. ��Ÿ��
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            
            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckOtherItemsNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }

        if (isBattlePopup)
        {
            // ���� ǥ��
            var trunkUI = transform.GetChild(1).transform.GetChild(1);
            trunkUI.GetComponentInChildren<Text>().text = $"{trunkCurrentWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
            
            // ĳ���� �κ��丮 ǥ��
            PrintCharInventory();
        }
        else
        {
            // ĳ���� �κ��丮 ǥ��
            var trunkUI = transform.GetChild(0).transform.GetChild(2);
            trunkUI.GetComponentInChildren<Text>().text = $"{trunkCurrentWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
        }
    }

    // ĳ����
    private void PrintCharInventory()
    {
        // ���� ���� ����
        if (truckMemberGo.Count != 0)
        {
            foreach (var element in truckMemberGo)
            {
                Destroy(element.Value);
            }
            truckMemberGo.Clear();
        }

        int i = 1;
        foreach (var element in playerDataMgr.boardingSquad)
        {
            var character = Instantiate(truckMemberPrefab, truckMemberList.transform);
            truckMemberGo.Add(element.Key, character);

            if (isBattlePopup)
            {
                // Ʈ�� -> ���� �κ��丮
                var moveButton = character.transform.GetChild(0).GetComponentInChildren<Button>();
                moveButton.onClick.AddListener(delegate { MoveItem(element.Key); });

                // ���� -> Ʈ�� �κ��丮
                var returnButton = character.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
                returnButton.onClick.AddListener(delegate { SelectCharacter(element.Key); });
            }

            // 2. ĳ���� ������
            var charImg = character.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = playerDataMgr.currentSquad[element.Value].character.halfImg;

            // 3. �а� ����
            var child = character.transform.GetChild(2).gameObject;
            child.GetComponentInChildren<Image>().sprite = playerDataMgr.currentSquad[element.Value].character.icon;
            child.GetComponentInChildren<Text>().text = $"Lv {playerDataMgr.currentSquad[element.Value].level}";

            // 4. ���� & źâ
            // 4-1. ����
            //var weight = character.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            //weight.text = $"{bagCurrentWeight} / {playerDataMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
            // 4-2. źâ
            var bullet = character.transform.GetChild(3).transform.GetChild(1).gameObject;
            var mainBullet = bullet.transform.GetChild(0);
            mainBullet.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                playerDataMgr.currentSquad[element.Value].weapon.MainWeaponBullet.ToString();
            var subBullet = bullet.transform.GetChild(1);
            subBullet.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                playerDataMgr.currentSquad[element.Value].weapon.SubWeaponBullet.ToString();

            foreach (var item in playerDataMgr.currentSquad[element.Key].bag)
            {
                // Create Object
                var itemList = character.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0);
                var itemGo = Instantiate(itemImg, itemList);
                var bagWeight = truckMemberGo[element.Key].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

                // Return Button
                var returnButton = itemGo.AddComponent<Button>();
                returnButton.onClick.AddListener(delegate { ReturnItem(item.Key); });

                // �Ҹ�ǰ
                if (playerDataMgr.consumableList.ContainsKey(item.Key))
                {
                    // ������ �̹���
                    itemGo.transform.GetChild(0).GetComponent<Image>().sprite = playerDataMgr.consumableList[item.Key].img;
                    // ����
                    itemGo.GetComponentInChildren<Text>().text = $"{item.Value}";
                    
                    //bagCurrentWeight += (playerDataMgr.consumableList[item.Key].weight * item.Value);
                    //bagWeight.text = $"{bagCurrentWeight} / {playerDataMgr.currentSquad[element.Key].MaxHp}";
                }

                i++;
            }
        }
    }

    public void SelectItem(string key)
    {
        truckItemObj[key].GetComponent<Image>().color = Color.red;
        currentKey = key;
    }

    int curSelectedChar = -1;

    public void SelectCharacter(int key)
    {
        curSelectedChar = key;
        //Debug.Log(playerDataMgr.currentSquad[key].Name);
        //playerDataMgr.currentSquad[key].bag
        //bagObjs[key].GetComponent<Image>().color = Color.red;
        //currentKey = key;
    }

    int itemNum = 1;
    public void MoveItem(int key)
    {
        if (currentKey == null) return;
        if (!truckItemObj.ContainsKey(currentKey)) return;
        truckItemObj[currentKey].GetComponent<Image>().color = Color.white;

        currentIndex = key;

        // characterStats.bag : ������id, ����
        // 2. Consume
        if (playerDataMgr.truckConsumables.ContainsKey(currentKey))
        {
            if (playerDataMgr.truckConsumablesNum[currentKey] <= 0)
            {
                Debug.Log("���� ����");
                return;
            }

            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(currentKey))
            {
                // �õ���
                playerDataMgr.currentSquad[currentIndex].bag.Add(currentKey, key);
            }
            else
            {
                // �õ���
                playerDataMgr.currentSquad[currentIndex].bag[currentKey] += 1;
                // ���� ������
                //bagConsumableNumInfo[currentKey] += 1;
            }

            Debug.Log($"{playerDataMgr.currentSquad[currentIndex].Name}�� ���濡 {playerDataMgr.currentSquad[currentIndex].bag.Count}���� ������ ����");
            Debug.Log($"�߰��� ������ �̸� : {playerDataMgr.truckConsumables[currentKey].name}");

            // ������ ������ �ű� ��
            if (playerDataMgr.truckConsumablesNum[currentKey] - 1 == 0)
            {
                playerDataMgr.truckConsumables.Remove(currentKey);
                playerDataMgr.truckConsumablesNum.Remove(currentKey);
            }
            // ���� �������� �� ���� ��
            else
            {
                playerDataMgr.truckConsumablesNum[currentKey] -= 1;
            }

            #region Json
            //var weight = truckConsumableInfo[currentKey].weight;
            ////if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;
            //
            ////json.
            //int index = 0;
            //var id = truckConsumableInfo[currentKey].id;
            //
            //index = playerDataMgr.currentSquad[currentIndex].saveId;        // Scout �� ���� ���õ� ĳ���� ���� ���
            ////Debug.Log(playerDataMgr.currentSquad[currentIndex].Name);     // Scout, Tanker ..
            //
            //int firstIndex = playerDataMgr.saveData.bagConsumableFirstIndex[index];
            //int lastIndex = playerDataMgr.saveData.bagConsumableLastIndex[index];
            //
            //bool contain = false;
            //int containIndex = -1;
            //for (int i = firstIndex; i < lastIndex; i++)
            //{
            //    if (playerDataMgr.saveData.bagConsumableList[i] != id) continue;
            //
            //    contain = true;
            //    containIndex = i;
            //}
            //
            //if (!contain)
            //{
            //    playerDataMgr.saveData.bagConsumableList.Insert(lastIndex, id);
            //    playerDataMgr.saveData.bagConsumableNumList.Insert(lastIndex, itemNum);
            //    playerDataMgr.saveData.bagConsumableLastIndex[index]++;
            //
            //    for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
            //    {
            //        playerDataMgr.saveData.bagConsumableFirstIndex[i]++;
            //        playerDataMgr.saveData.bagConsumableLastIndex[i]++;
            //    }
            //}
            //else
            //{
            //    playerDataMgr.saveData.bagConsumableNumList[containIndex] += itemNum;
            //}
            //
            //index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
            //if (playerDataMgr.saveData.truckConsumableNumList[index] - itemNum == 0)
            //{
            //    playerDataMgr.saveData.truckConsumableList.Remove(id);
            //    playerDataMgr.saveData.truckConsumableNumList.RemoveAt(index);
            //}
            //else playerDataMgr.saveData.truckConsumableNumList[index] -= itemNum;
            //
            //PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            //
            ////playerDataMgr.
            //if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            //{
            //    //���絥���� ����.
            //    bagConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
            //    bagConsumableNumInfo.Add(currentKey, itemNum);
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            //}
            //else
            //{
            //    //���絥���� ����.
            //    bagConsumableNumInfo[currentKey] += itemNum;
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            //}
            //
            //if (playerDataMgr.truckConsumablesNum[id] - itemNum == 0)
            //{
            //    //���� ������.
            //    truckConsumableInfo.Remove(currentKey);
            //    truckConsumableNumInfo.Remove(currentKey);
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.truckConsumables.Remove(id);
            //    playerDataMgr.truckConsumablesNum.Remove(id);
            //}
            //else
            //{
            //    //���� ������.
            //    truckConsumableNumInfo[currentKey] -= itemNum;
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.truckConsumablesNum[id] -= itemNum;
            //}
            #endregion
        }

        PrintTrunkItems();
        //Init();
        currentKey = null;
    }

    public void ReturnItem(string key)
    {
        if (trunkCurrentWeight >= playerDataMgr.truckList[playerDataMgr.saveData.currentCar].weight) return;
        if (curSelectedChar <= -1) return;

        if (playerDataMgr.currentSquad[curSelectedChar].bag.ContainsKey(key))
        {
            //Debug.Log("���õ� ������ : " + playerDataMgr.consumableList[key].name);
            
            // ������ �������� ��
            if(playerDataMgr.currentSquad[curSelectedChar].bag[key] - 1 <= 0)
            {
                playerDataMgr.currentSquad[curSelectedChar].bag.Remove(key);
            }
            // ���� �� ������ ��
            else
            {
                playerDataMgr.currentSquad[curSelectedChar].bag[key] -= 1;
                Debug.Log(playerDataMgr.currentSquad[curSelectedChar].bag[key]);
            }

            // Ʈ�� �κ��丮�� ���� ��
            if(!playerDataMgr.truckConsumables.ContainsKey(key))
            {
                playerDataMgr.truckConsumables.Add(key, playerDataMgr.consumableList[key]);
                playerDataMgr.truckConsumablesNum.Add(key, 1);
            }
            // Ʈ�� �κ��丮�� ���� ��
            else
            {
                playerDataMgr.truckConsumablesNum[key] += 1;
            }
        }

        Init();
        //PrintCharInventory();
        //PrintTrunkItems();
        //Init();
        /*
        truckItemObj[currentKey].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;

        if (playerDataMgr.consumableList.ContainsKey(currentKey))
        {
            // ������ ���� �������� ��
            if (playerDataMgr.currentSquad[currentIndex].bag[currentKey] - 1 == 0)
            {

            }
            //// ������ ���� �������� ��
            //if (bagConsumableNumInfo[currentKey] - 1 == 0)
            //{
            //    bagConsumableInfo.Remove(currentKey);
            //    bagConsumableNumInfo.Remove(currentKey);
            //    
            //    playerDataMgr.currentSquad[currentIndex].bag.Remove(currentKey);
            //}
            //// �������� �� �������� ��
            //else
            //{
            //    Debug.Log(playerDataMgr.currentSquad[currentIndex].Name);
            //
            //    bagConsumableNumInfo[currentKey] -= 1;
            //    playerDataMgr.currentSquad[currentIndex].bag[currentKey] -= 1;
            //}

            //var weight = bagConsumableInfo[currentKey].weight;
            ////if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;
            //
            ////json.
            //int index;
            //var id = bagConsumableInfo[currentKey].id;      // ���õ� �������� id
            ////Debug.Log(bagConsumableInfo[currentKey].name);  // Ȳ�� ������
            //if (!playerDataMgr.saveData.truckConsumableList.Contains(id))
            //{
            //    playerDataMgr.saveData.truckConsumableList.Add(id);
            //    playerDataMgr.saveData.truckConsumableNumList.Add(itemNum);
            //}
            //else
            //{
            //    index = playerDataMgr.saveData.truckConsumableList.IndexOf(id);
            //    playerDataMgr.saveData.truckConsumableNumList[index] += itemNum;
            //}
            //
            //index = currentIndex;
            //int firstIndex = playerDataMgr.saveData.bagConsumableFirstIndex[index];
            //int lastIndex = playerDataMgr.saveData.bagConsumableLastIndex[index];
            //
            //int containIndex = -1;
            //for (int i = firstIndex; i < lastIndex; i++)
            //{
            //    if (playerDataMgr.saveData.bagConsumableList[i] != id) continue;
            //
            //    containIndex = i;
            //}
            //
            ////
            //if (playerDataMgr.saveData.bagConsumableNumList[containIndex] - itemNum == 0)
            //{
            //    playerDataMgr.saveData.bagConsumableList.RemoveAt(containIndex);
            //    playerDataMgr.saveData.bagConsumableNumList.RemoveAt(containIndex);
            //
            //    playerDataMgr.saveData.bagConsumableLastIndex[index]--;
            //
            //    for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
            //    {
            //        playerDataMgr.saveData.bagConsumableFirstIndex[i]--;
            //        playerDataMgr.saveData.bagConsumableLastIndex[i]--;
            //    }
            //}
            //else playerDataMgr.saveData.bagConsumableNumList[containIndex] -= itemNum;
            //
            //PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            //
            ////playerDataMgr.
            //if (!playerDataMgr.truckConsumables.ContainsKey(id))
            //{
            //    //���絥���� ����.
            //    truckConsumableInfo.Add(currentKey, bagConsumableInfo[currentKey]);
            //    truckConsumableNumInfo.Add(currentKey, itemNum);
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
            //    playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            //}
            //else
            //{
            //    //���絥���� ����.
            //    truckConsumableNumInfo[currentKey] += itemNum;
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.truckConsumablesNum[id] += itemNum;
            //}
            //
            //PrintTrunkItems();
            //
            //if (bagConsumableNumInfo[id] - itemNum == 0)
            //{
            //    //���� ������.
            //    bagConsumableInfo.Remove(currentKey);
            //    bagConsumableNumInfo.Remove(currentKey);
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
            //}
            //else
            //{
            //    //���� ������.
            //    bagConsumableNumInfo[currentKey] -= itemNum;
            //
            //    //�÷��̾� ������ �Ŵ��� ����.
            //    playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            //}
            //PrintCharInventory();
        }
        */
    }

    public override void OpenWindow()
    {
        base.OpenWindow();
        Init();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }
}
