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

    public bool isOpen;

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

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        // 트럭 인벤토리 무게 계산
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

            switch (element.Key)
            {
                case "BUL_0004":
                    bullet5Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0005":
                    bullet7Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0002":
                    bullet9Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0003":
                    bullet45Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
                case "BUL_0001":
                    bullet12Num += playerDataMgr.truckOtherItemsNum[element.Key];
                    break;
            }
        }

        //// Update Trunk weight
        NonBattleMgr.Instance.worldUIMgr.printTruckUI.truckCurWeight = trunkCurrentWeight;
        NonBattleMgr.Instance.worldUIMgr.printTruckUI.Init();

        // 탄알 무게 계산
        if (isBattlePopup)
        {
            var bulletGroup = transform.GetChild(2).transform.GetChild(2).gameObject;
            var bullet5 = bulletGroup.transform.GetChild(0).GetComponentInChildren<Text>();
            bullet5.text = bullet5Num.ToString();
            var bullet7 = bulletGroup.transform.GetChild(1).GetComponentInChildren<Text>();
            bullet7.text = bullet7Num.ToString();
            var bullet9 = bulletGroup.transform.GetChild(2).GetComponentInChildren<Text>();
            bullet9.text = bullet9Num.ToString();
            var bullet45 = bulletGroup.transform.GetChild(3).GetComponentInChildren<Text>();
            bullet45.text = bullet45Num.ToString();
            var bullet12 = bulletGroup.transform.GetChild(4).GetComponentInChildren<Text>();
            bullet12.text = bullet12Num.ToString();
        }
        else
        {
            var bulletGroup = transform.GetChild(0).transform.GetChild(3).gameObject;
            var bullet5 = bulletGroup.transform.GetChild(0).GetComponentInChildren<Text>();
            bullet5.text = bullet5Num.ToString();
            var bullet7 = bulletGroup.transform.GetChild(1).GetComponentInChildren<Text>();
            bullet7.text = bullet7Num.ToString();
            var bullet9 = bulletGroup.transform.GetChild(2).GetComponentInChildren<Text>();
            bullet9.text = bullet9Num.ToString();
            var bullet45 = bulletGroup.transform.GetChild(3).GetComponentInChildren<Text>();
            bullet45.text = bullet45Num.ToString();
            var bullet12 = bulletGroup.transform.GetChild(4).GetComponentInChildren<Text>();
            bullet12.text = bullet12Num.ToString();
        }


        PrintTrunkItems();
    }

    public void PrintTrunkItems()
    {
        trunkCurrentWeight = 0;

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

            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }
        // 소모품
        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);

            if (isBattlePopup)
            {
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key); });
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

            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }
        // 3. 기타템
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            
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

            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);
            truckItemObj.Add(element.Key, go);
        }

        if (isBattlePopup)
        {
            // 무게 표시
            var trunkUI = transform.GetChild(2).transform.GetChild(1);
            trunkUI.GetComponentInChildren<Text>().text = $"{trunkCurrentWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
            
            // 캐릭터 인벤토리 표시
            PrintCharInventory();
        }
        else
        {
            // 무게 표시
            var trunkUI = transform.GetChild(0).transform.GetChild(2);
            trunkUI.GetComponentInChildren<Text>().text = $"{trunkCurrentWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
        }
    }

    // 캐릭터
    private void PrintCharInventory()
    {
        // 이전 정보 삭제
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
                // 트럭 -> 개별 인벤토리
                var moveButton = character.transform.GetChild(0).GetComponentInChildren<Button>();
                moveButton.onClick.AddListener(delegate { MoveItem(element.Key); });

                // 개별 -> 트럭 인벤토리
                var returnButton = character.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
                returnButton.onClick.AddListener(delegate { SelectCharacter(element.Key); });
            }

            // 2. 캐릭터 아이콘
            var charImg = character.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = playerDataMgr.currentSquad[element.Value].character.halfImg;

            // 3. 분과 정보
            var child = character.transform.GetChild(2).gameObject;
            child.GetComponentInChildren<Image>().sprite = playerDataMgr.currentSquad[element.Value].character.icon;
            child.GetComponentInChildren<Text>().text = $"Lv {playerDataMgr.currentSquad[element.Value].level}";

            // 4. 무게 & 탄창
            // 4-1. 무게
            //var weight = character.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            //weight.text = $"{bagCurrentWeight} / {playerDataMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
            // 4-2. 탄창
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

                // 소모품
                if (playerDataMgr.consumableList.ContainsKey(item.Key))
                {
                    // 아이템 이미지
                    itemGo.transform.GetChild(0).GetComponent<Image>().sprite = playerDataMgr.consumableList[item.Key].img;
                    // 무게
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

        // characterStats.bag : 아이템id, 개수
        // 2. Consume
        if (playerDataMgr.truckConsumables.ContainsKey(currentKey))
        {
            if (playerDataMgr.truckConsumablesNum[currentKey] <= 0)
            {
                Debug.Log("수량 부족");
                return;
            }

            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(currentKey))
            {
                // 플데메
                playerDataMgr.currentSquad[currentIndex].bag.Add(currentKey, 1);
                //playerDataMgr.currentSquad[currentIndex].bag[currentKey] += 1;  // 0 -> 1
            }
            else
            {
                // 플데메
                playerDataMgr.currentSquad[currentIndex].bag[currentKey] += 1;
                // 현재 데이터
                //bagConsumableNumInfo[currentKey] += 1;
            }

            Debug.Log($"{playerDataMgr.currentSquad[currentIndex].Name}의 가방에 {playerDataMgr.currentSquad[currentIndex].bag.Count}개의 아이템 있음");
            Debug.Log($"추가된 아이템 이름 : {playerDataMgr.truckConsumables[currentKey].name}");

            // 마지막 아이템 옮길 때
            if (playerDataMgr.truckConsumablesNum[currentKey] - 1 == 0)
            {
                playerDataMgr.truckConsumables.Remove(currentKey);
                playerDataMgr.truckConsumablesNum.Remove(currentKey);
            }
            // 남은 아이템이 더 있을 때
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
            //index = playerDataMgr.currentSquad[currentIndex].saveId;        // Scout 등 현재 선택된 캐릭터 정보 담김
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
            //    //현재데이터 관련.
            //    bagConsumableInfo.Add(currentKey, truckConsumableInfo[currentKey]);
            //    bagConsumableNumInfo.Add(currentKey, itemNum);
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            //}
            //else
            //{
            //    //현재데이터 관련.
            //    bagConsumableNumInfo[currentKey] += itemNum;
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            //}
            //
            //if (playerDataMgr.truckConsumablesNum[id] - itemNum == 0)
            //{
            //    //현재 데이터.
            //    truckConsumableInfo.Remove(currentKey);
            //    truckConsumableNumInfo.Remove(currentKey);
            //
            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.truckConsumables.Remove(id);
            //    playerDataMgr.truckConsumablesNum.Remove(id);
            //}
            //else
            //{
            //    //현재 데이터.
            //    truckConsumableNumInfo[currentKey] -= itemNum;
            //
            //    //플레이어 데이터 매니저 관련.
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
            //Debug.Log("선택된 아이템 : " + playerDataMgr.consumableList[key].name);
            
            // 마지막 아이템일 때
            if(playerDataMgr.currentSquad[curSelectedChar].bag[key] - 1 <= 0)
            {
                playerDataMgr.currentSquad[curSelectedChar].bag.Remove(key);
            }
            // 여러 개 남았을 때
            else
            {
                playerDataMgr.currentSquad[curSelectedChar].bag[key] -= 1;
                Debug.Log(playerDataMgr.currentSquad[curSelectedChar].bag[key]);
            }

            // 트럭 인벤토리에 없을 때
            if(!playerDataMgr.truckConsumables.ContainsKey(key))
            {
                playerDataMgr.truckConsumables.Add(key, playerDataMgr.consumableList[key]);
                playerDataMgr.truckConsumablesNum.Add(key, 1);
            }
            // 트럭 인벤토리에 있을 때
            else
            {
                playerDataMgr.truckConsumablesNum[key] += 1;
            }
        }

        Init();
    }

    public override void OpenWindow()
    {
        base.OpenWindow();
        Init();
        isOpen = true;
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
        isOpen = false;
    }
}
