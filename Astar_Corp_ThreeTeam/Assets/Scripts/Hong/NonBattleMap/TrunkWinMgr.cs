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
    int bulletNum;

    // Weight
    public TextMeshProUGUI totalWeight;
    private int trunkCurrentWeight;
    int bagCurrentWeight;

    private bool isInit;
    public bool isBattlePopup;

    public Dictionary<string, GameObject> truckItemObj = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> TruckMemberGo = new Dictionary<int, GameObject>();

    // Truck item Data
    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();

    // Character item Data
    Dictionary<string, Weapon> bagWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> bagWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> bagOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> bagOtherItemNumInfo = new Dictionary<string, int>();

    Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();


    private void Update()
    {
        // [수정] 트럭 무게 띄우기
        //if (playerDataMgr.saveData.currentCar == null)
        //{
        //    playerDataMgr.saveData.currentCar = "TRU_0001";//soMgr.GetTruck("TRU_0000");
        //}
        //
        //if (trunkCurrentWeight > 0)
        //    totalWeight.text = $" {trunkCurrentWeight}/ {soMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
        //else
        //    totalWeight.text = $" 0 / {soMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
    }

    public void Init()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        //// 임시 데이터 넣기
        //Weapon weapon1 = new Weapon();
        //weapon1 = soMgr.GetEquippable("WEP_0003");
        //Weapon weapon2 = new Weapon();
        //weapon2 = soMgr.GetEquippable("WEP_0010");
        //
        //if (!playerDataMgr.truckEquippables.ContainsKey("0"))
        //    playerDataMgr.truckEquippables.Add("0", weapon1);
        //
        //if (!playerDataMgr.truckEquippables.ContainsKey("1"))
        //   playerDataMgr.truckEquippables.Add("1", weapon2);

        if (!isInit)
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

            foreach (var element in playerDataMgr.truckOtherItems)
            {
                truckOtherItemInfo.Add(element.Key, element.Value);
                truckOtherItemNumInfo.Add(element.Key, playerDataMgr.truckOtherItemsNum[element.Key]);
            }

            isInit = true;
        }

        PrintTrunkItems();

        if (isBattlePopup)
        {
            PrintCharInventory();
        }
    }

    private void PrintCharInventory()
    {
        // 이전 정보 삭제
        if (TruckMemberGo.Count != 0)
        {
            foreach (var element in TruckMemberGo)
            {
                Destroy(element.Value);
            }
            TruckMemberGo.Clear();
        }

        int i = 1;
        foreach (var element in playerDataMgr.boardingSquad)
        {
            var character = Instantiate(truckMemberPrefab, truckMemberList.transform);
            if (isBattlePopup)
            {
                var button = character.GetComponentInChildren<Button>();
                button.onClick.AddListener(delegate { MoveItem(element.Key); });
            }

            // 1. 선택 토글키
            // 

            // 2. 캐릭터 아이콘
            var charImg = character.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = playerDataMgr.currentSquad[element.Value].character.halfImg;

            // 3. 분과 정보
            var child = character.transform.GetChild(2).gameObject;
            child.GetComponentInChildren<Image>().sprite = playerDataMgr.currentSquad[element.Value].character.icon;
            child.GetComponentInChildren<Text>().text = $"Lv {playerDataMgr.currentSquad[element.Value].level}";

            // 4. 무게 & 탄창
            // 4-1. 무게
            var weight = character.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            //weight.text = $"{bagCurrentWeight} / {playerDataMgr.currentSquad[element.Key].MaxHp}";
            // 4-2. 탄창
            var bullet = character.transform.GetChild(3).transform.GetChild(1).gameObject;
            var mainBullet = bullet.transform.GetChild(0);
            mainBullet.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerDataMgr.currentSquad[element.Value].weapon.MainWeaponBullet.ToString();

            //if (playerDataMgr.currentSquad[element.Value].weapon != null)
            //{
            //    var mainBulletImg = bullet.transform.GetChild(0).GetComponent<Image>();
            //    mainBulletImg.sprite = playerDataMgr.currentSquad[element.Value].weapon.mainWeapon.img;
            //}
            //if (playerDataMgr.currentSquad[element.Value].weapon != null)
            //{
            //    var subBulletImg = bullet.transform.GetChild(1).GetComponent<Image>();
            //    subBulletImg.sprite = playerDataMgr.currentSquad[element.Value].weapon.subWeapon.img;
            //}

            foreach (var item in playerDataMgr.currentSquad[element.Key].bag)
            {
                // // 아이템 오브젝트
                var itemList = character.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0);
                var itemGo = Instantiate(itemImg, itemList);
                var button = itemGo.AddComponent<Button>();
                //Debug.Log(item.Key);
                button.onClick.AddListener(delegate { ReturnItem(item.Key); });
                //var weight = TruckMemberGo[key].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

                // 장비(무기)
                if (playerDataMgr.equippableList.ContainsKey(item.Key))
                {
                    //Debug.Log($"{playerDataMgr.equippableList[item.Key].name} {item.Value}개");
                    itemGo.transform.GetChild(0).GetComponent<Image>().sprite = playerDataMgr.equippableList[item.Key].img;
                    itemGo.GetComponentInChildren<Text>().text = $"{item.Value}";
                    bagCurrentWeight += (playerDataMgr.equippableList[item.Key].weight * item.Value);
                    //bagObjs.Add(item.Key, itemGo);
                }
                // 소모품
                if (playerDataMgr.consumableList.ContainsKey(item.Key))
                {
                    //Debug.Log($"{playerDataMgr.consumableList[item.Key].name} {item.Value}개");
                    itemGo.transform.GetChild(0).GetComponent<Image>().sprite = playerDataMgr.consumableList[item.Key].img;
                    itemGo.GetComponentInChildren<Text>().text = $"{item.Value}";
                    bagCurrentWeight += (playerDataMgr.consumableList[item.Key].weight * item.Value);
                    //bagObjs.Add(item.Key, itemGo);
                }
                // 기타
                if (playerDataMgr.otherItemList.ContainsKey(item.Key))
                {
                    //Debug.Log($"{playerDataMgr.otherItemList[item.Key].name} {item.Value}개");
                    itemGo.transform.GetChild(0).GetComponent<Image>().sprite = playerDataMgr.otherItemList[item.Key].img;
                    itemGo.GetComponentInChildren<Text>().text = $"{item.Value}";
                    bagCurrentWeight += (int.Parse(playerDataMgr.otherItemList[item.Key].weight) * item.Value);
                    //bagObjs.Add(item.Key, itemGo);
                }
                //weight.text = $"{bagCurrentWeight} / {playerDataMgr.currentSquad[element.Key].MaxHp}";
            }

            TruckMemberGo.Add(element.Key, character);
            i++;
        }
    }

    public void PrintTrunkItems()
    {
        Debug.Log("Print Truck Items");

        // 이전 정보 삭제
        if (truckItemObj.Count != 0)
        {
            foreach (var element in truckItemObj)
            {
                Destroy(element.Value);
            }
            truckItemObj.Clear();
        }

        // 생성
        // 1. 무기
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);

            //if (isBattlePopup)
            //{
            //    var button = go.AddComponent<Button>();
            //    button.onClick.AddListener(delegate { SelectWeapon(element.Key); });
            //}

            // 이미지 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // 이름 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // 수량 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}개";
            // 무게 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // 가격 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            truckItemObj.Add(element.Key, go);
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
        }
        // 소모품
        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            //var button = go.AddComponent<Button>();

            if (isBattlePopup)
            {
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectConsume(element.Key); });
            }

            // 이미지 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // 이름 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // 수량 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckConsumablesNum[element.Key]}개";
            // 무게 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // 가격 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            truckItemObj.Add(element.Key, go);
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
        }
        // 3. 기타템
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            //var mainBullet = 
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            
            if (isBattlePopup)
            {
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectOtherItem(element.Key); });
            }

            // 이미지 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // 이름 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // 수량 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckOtherItemsNum[element.Key]}개";
            // 무게 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // 가격 4
            go.transform.GetChild(4).GetComponent<Text>().text = element.Value.price.ToString();

            truckItemObj.Add(element.Key, go);
            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);
        }
    }

    public void SelectWeapon(string key)
    {
        truckItemObj[key].GetComponent<Image>().color = Color.red;
        currentKey = key;
        Debug.Log(truckWeaponInfo[key].name);
        Debug.Log(truckWeaponNumInfo[key]); //수량
    }

    public void SelectConsume(string key)
    {
        truckItemObj[key].GetComponent<Image>().color = Color.red;
        currentKey = key;
        Debug.Log(truckConsumableInfo[key].name);
        Debug.Log(truckConsumableNumInfo[key]);
    }

    public void SelectOtherItem(string key)
    {
        truckItemObj[key].GetComponent<Image>().color = Color.red;
        currentKey = key;
        Debug.Log(truckOtherItemInfo[key].name);
    }

    int itemNum = 1;
    public void MoveItem(int key)
    {
        if (currentKey == null) return;
        //if (bagCurrentWeight > playerDataMgr.currentSquad[value].MaxHp) return;
        truckItemObj[currentKey].GetComponent<Image>().color = Color.white;

        if (playerDataMgr.currentSquad[key].bag.Count > 3) return;
        currentIndex = key;
        // 1. Weapon
        if (truckWeaponInfo.ContainsKey(currentKey))
        {
            var weight = truckWeaponInfo[currentKey].weight;

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
                //현재데이터 관련.
                bagWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
                bagWeaponNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagWeaponNumInfo[currentKey] += itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.truckEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                //currentKey = null;
            }
            else
            {
                //현재 데이터.
                truckWeaponNumInfo[currentKey] -= itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] -= itemNum;
            }
        }
        // 2. Consume
        if (truckConsumableInfo.ContainsKey(currentKey))
        {
            var weight = truckConsumableInfo[currentKey].weight;
            //if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

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
                //현재데이터 관련.
                bagConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
                bagConsumableNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagConsumableNumInfo[currentKey] += itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }

            if (playerDataMgr.truckConsumablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Remove(id);
                playerDataMgr.truckConsumablesNum.Remove(id);
            }
            else
            {
                //현재 데이터.
                truckConsumableNumInfo[currentKey] -= itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id] -= itemNum;
            }
        }
        // 3. Other
        if (truckOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(truckOtherItemInfo[currentKey].weight);
            //if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

            //string[] bulletType = new string[5];
            //bulletType[1] = "Ammo_12G";
            //bulletType[0] = "Ammo_9mm_01";
            //bulletType[2] = "Ammo_45_01";
            //bulletType[3] = "Ammo_556_01";
            //bulletType[4] = "Ammo_762_01";

            int bulletType = 0;
            switch (truckOtherItemInfo[currentKey].name)
            {
                case "Ammo_12G":
                    bulletType = 1;
                    break;
                case "Ammo_9mm_01":
                    bulletType = 2;
                    break;
                case "Ammo_45_01":
                    bulletType = 3;
                    break;
                case "Ammo_556_01":
                    bulletType = 4;
                    break;
                case "Ammo_762_01":
                    bulletType = 5;
                    break;
            }

            //Debug.Log(truckOtherItemInfo[currentKey].name);
            if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.bulletType == bulletType)
            {
                Debug.Log("불릿타입 일치");
                playerDataMgr.currentSquad[currentIndex].weapon.MainWeaponBullet += 1;
                playerDataMgr.truckOtherItemsNum[currentKey] -= 1;
            }
            else
            {
                return;
            }
            

            ////json.
            //int index = 0;
            //var id = truckOtherItemInfo[currentKey].id;
            //
            //index = playerDataMgr.currentSquad[currentIndex].saveId;
            //int firstIndex = playerDataMgr.saveData.bagOtherItemFirstIndex[index];
            //int lastIndex = playerDataMgr.saveData.bagOtherItemLastIndex[index];
            //
            //bool contain = false;
            //int containIndex = -1;
            //for (int i = firstIndex; i < lastIndex; i++)
            //{
            //    if (playerDataMgr.saveData.bagOtherItemList[i] != id) continue;
            //
            //    contain = true;
            //    containIndex = i;
            //}
            //
            //if (!contain)
            //{
            //    playerDataMgr.saveData.bagOtherItemList.Insert(lastIndex, id);
            //    playerDataMgr.saveData.bagOtherItemNumList.Insert(lastIndex, itemNum);
            //    playerDataMgr.saveData.bagOtherItemLastIndex[index]++;
            //
            //    for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
            //    {
            //        playerDataMgr.saveData.bagOtherItemFirstIndex[i]++;
            //        playerDataMgr.saveData.bagOtherItemLastIndex[i]++;
            //    }
            //}
            //else
            //{
            //    playerDataMgr.saveData.bagOtherItemNumList[containIndex] += itemNum;
            //}
            //
            //index = playerDataMgr.saveData.truckOtherItemList.IndexOf(id);
            //if (playerDataMgr.saveData.truckOtherItemNumList[index] - itemNum == 0)
            //{
            //    playerDataMgr.saveData.truckOtherItemList.Remove(id);
            //    playerDataMgr.saveData.truckOtherItemNumList.RemoveAt(index);
            //}
            //else playerDataMgr.saveData.truckOtherItemNumList[index] -= itemNum;
            //
            //PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            //
            ////playerDataMgr.
            //if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            //{
            //    //현재데이터 관련.
            //    bagOtherItemInfo.Add(currentKey, truckOtherItemInfo[currentKey]);
            //    bagOtherItemNumInfo.Add(currentKey, itemNum);
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            //}
            //else
            //{
            //    //현재데이터 관련.
            //    bagOtherItemNumInfo[currentKey] += itemNum;
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            //}
            //
            //if (playerDataMgr.truckOtherItemsNum[id] - itemNum == 0)
            //{
            //    //현재 데이터.
            //    truckOtherItemInfo.Remove(currentKey);
            //    truckOtherItemNumInfo.Remove(currentKey);
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.truckOtherItems.Remove(id);
            //    playerDataMgr.truckOtherItemsNum.Remove(id);
            //}
            //else
            //{
            //    //현재 데이터.
            //    truckOtherItemNumInfo[currentKey] -= itemNum;
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.truckOtherItemsNum[id] -= itemNum;
            //}
        }
        PrintTrunkItems();
        PrintCharInventory();
    }

    public void ReturnItem(string key)
    {
        currentKey = key;

        if (bagWeaponInfo.ContainsKey(currentKey))
        {
            var weight = bagWeaponInfo[currentKey].weight;
            //if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

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
                //현재데이터 관련.
                truckWeaponInfo.Add(currentKey, bagWeaponInfo[currentKey]);
                truckWeaponNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[currentKey] += itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] += itemNum;
            }

            PrintTrunkItems();

            if (bagWeaponNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
            }
            else
            {
                //현재 데이터.
                bagWeaponNumInfo[currentKey] -= itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            PrintCharInventory();
        }
        else if (bagConsumableInfo.ContainsKey(currentKey))
        {
            var weight = bagConsumableInfo[currentKey].weight;
            //if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

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
                //현재데이터 관련.
                truckConsumableInfo.Add(currentKey, bagConsumableInfo[currentKey]);
                truckConsumableNumInfo.Add(currentKey, itemNum);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckConsumableNumInfo[currentKey] += itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id] += itemNum;
            }

            PrintTrunkItems();

            if (bagConsumableNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagConsumableInfo.Remove(currentKey);
                bagConsumableNumInfo.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);
            }
            else
            {
                //현재 데이터.
                bagConsumableNumInfo[currentKey] -= itemNum;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            PrintCharInventory();
        }
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

    public void BattleStart(bool value)
    {
        isBattlePopup = value;
    }
}
