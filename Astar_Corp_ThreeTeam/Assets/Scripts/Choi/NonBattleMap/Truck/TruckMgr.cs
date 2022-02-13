using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum WarehouseKind
{ 
    None,
    Trunk,
    Bag
}

public class TruckMgr : MonoBehaviour
{
    public WindowManager windowManager;

    [Header("Truck Squad")]
    public GameObject TruckWin;
    public GameObject InventoryWin;
    public GameObject Contents;
    public GameObject TruckUnitPrefab;
    public Text RemainingNum;

    public Dictionary<int, GameObject> TruckUnitGOs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> BattleSqudeObj = new Dictionary<int, GameObject>();

    [Header("Selected Characters")]
    public GameObject selectedCharPrefab;
    public GameObject selectedChars;
    public GameObject truckEquips;
    public GameObject truckConsums;
    public TextMeshProUGUI charInfoTxt;

    [Header("Truck Inventory")]
    public GameObject equipTruckItem;
    public GameObject consumTruckItem;

    [Header("Character Inventory")]
    public GameObject SquadItemPrefab;
    public GameObject char1Inventory;
    public GameObject char2Inventory;
    public GameObject char3Inventory;
    public GameObject char4Inventory;

    //지은.
    [Header("버튼 관련")]
    public List<GameObject> trunkButtons;
    public List<GameObject> bagButtons;
    
    [Header("총알 관련")]
    public Text trunkBullet5Txt;
    public Text trunkBullet7Txt;
    public Text trunkBullet9Txt;
    public Text trunkBullet45Txt;
    public Text trunkBullet12Txt;

    public Text bagBullet5Txt;
    public Text bagBullet7Txt;
    public Text bagBullet9Txt;
    public Text bagBullet45Txt;
    public Text bagBullet12Txt;

    public Text trunkWeightTxt;
    public Text bagWeightTxt;

    public GameObject trunkAllToggle;
    public GameObject bagAllToggle;

    [Header("팝업창 관련")]
    public GameObject popupWin;
    public Text itemNameTxt;
    public Text itemNumTxt;
    public Slider slider;
    public GameObject mainWeaponButton;
    public GameObject subWeaponButton;
    public GameObject bagButton;
    public GameObject disarmButton;
    public GameObject Add10Button;
    public GameObject Add100Button;
    public GameObject AddMaxButton;

    public GameObject truckContents;
    public GameObject truckPrefab;
    public GameObject bagContents;
    public GameObject bagPrefab;

    public GameObject mainWeaponObj;
    public GameObject subWeaponObj;
    public Text mainWeaponTxt;
    public Text subWeaponTxt;

    Dictionary<string, GameObject> truckObjs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> bagObjs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();

    Dictionary<string, Weapon> bagWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> bagWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> bagConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> bagConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> bagOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> bagOtherItemNumInfo = new Dictionary<string, int>();

    int trunkCurrentWeight;
    int trunkTotalWeight;
    int bagCurrentWeight;
    int bagTotalWeight;

    int currentIndex;
    string currentKey;
    WarehouseKind currentKind;
    EquipKind currentEquipKind;
    int disarmMode;

    TrunkMode trunkMode;
    BagMode bagMode;

    PlayerDataMgr playerDataMgr;

    //나중에 삭제해야됨.
    public void GoToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void Init()
    {
        if(playerDataMgr == null) playerDataMgr = PlayerDataMgr.Instance;

        if (playerDataMgr.battleSquad.Count != 0) playerDataMgr.battleSquad.Clear();
        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
        if (popupWin.activeSelf) popupWin.SetActive(false);

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

        mainWeaponTxt.text = "비어있음";
        subWeaponTxt.text = "비어있음";

        currentIndex = -1;
        currentKey = null;
        currentKind = WarehouseKind.None;
        currentEquipKind = EquipKind.None;
        disarmMode = -1;
    }

    public void Open()
    {
        var currentWinId = (int)Windows.MemberSelectPopup - 1;
        windowManager.Open(currentWinId);

        Init();

        // MemberSelectPopup 창에서 띄울 캐릭터 정보 텍스트
        // 해당 팝업창이 Open 될때 Find
        //charInfoTxt = GameObject.Find("CharacterInfo").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Close()
    {
        TruckWin.SetActive(false);
    }

    // 전투 씬에 데려갈 캐릭터 선택
    void SelectBattleCharacter(int num)
    {
        //중복될 때.
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            playerDataMgr.battleSquad.Remove(num);
            var go = TruckUnitGOs[num];
            var child = go.transform.GetChild(0).GetComponent<Image>();
            child.color = Color.white;

            RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
            return;
        }

        //가득찼을때.
        if (playerDataMgr.battleSquad.Count == 4) return;

        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        var selectedGo = TruckUnitGOs[num];
        var selectedChild = selectedGo.transform.GetChild(0).GetComponent<Image>();
        selectedChild.color = Color.red;

        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();

        //클릭 시 옆에 캐릭터 정보 표시
        charInfoTxt.text = playerDataMgr.battleSquad[num].Name + "\n" +
            "Hp : " + playerDataMgr.battleSquad[num].currentHp;
    }

    // 트럭UI 세팅 (초기화)
    public void UISetting()
    {
        if (BattleSqudeObj.Count != 0)
        {
            foreach (var element in BattleSqudeObj)
            {
                Destroy(element.Value);
            }
            BattleSqudeObj.Clear();

            selectedChars.transform.DetachChildren();
        }

        // BattleSquad Setting
        foreach (var element in playerDataMgr.battleSquad)
        {
            var go = Instantiate(selectedCharPrefab, selectedChars.transform);
            var goName = go.GetComponentInChildren<TextMeshProUGUI>();
            goName.text = element.Value.character.name;

            var button = go.GetComponent<Button>();
            int key = element.Key;
            button.onClick.AddListener(delegate { SelectCharInventory(key); });

            BattleSqudeObj.Add(element.Key, go);
        }

        if (truckWeaponInfo.Count != 0) truckWeaponInfo.Clear();
        if (truckWeaponNumInfo.Count != 0) truckWeaponNumInfo.Clear();
        if (truckConsumableInfo.Count != 0) truckConsumableInfo.Clear();
        if (truckConsumableNumInfo.Count != 0) truckConsumableNumInfo.Clear();
        if (truckOtherItemInfo.Count != 0) truckOtherItemInfo.Clear();
        if (truckOtherItemNumInfo.Count != 0) truckOtherItemNumInfo.Clear();

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

       DisplayTruckItem(0);
       DisplayBagItem(0);
    }

    //// 트럭 아이템 카테고리 선택 (장비 vs 소비)
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

    public void SelectWeapon(int index)
    {
        switch (index)
        {
            case 0:
                currentEquipKind = EquipKind.MainWeapon;
                break;
            case 1:
                currentEquipKind = EquipKind.SubWeapon;
                break;
        }
    }

    public void Equip()
    {
        if (currentIndex == -1) return;
        if (currentKey == null) return;
        var weaponType = playerDataMgr.equippableList[currentKey].type;
        if (!playerDataMgr.currentSquad[currentIndex].character.weapons.Contains(weaponType))
            return;

        Disarm();
        if (currentEquipKind == EquipKind.MainWeapon)
        {
            playerDataMgr.saveData.mainWeapon[currentIndex] = currentKey;
            var weapon = playerDataMgr.equippableList[currentKey];
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = weapon;

            mainWeaponTxt.text = weapon.name;
        }
        else if (currentEquipKind == EquipKind.SubWeapon)
        {
            playerDataMgr.saveData.subWeapon[currentIndex] = currentKey;
            var weapon = playerDataMgr.equippableList[currentKey];
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = weapon;

           subWeaponTxt.text = weapon.name;
        }

        if (currentKind == WarehouseKind.Trunk)
        {
            var id = playerDataMgr.equippableList[currentKey].id;
            var index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
            if (playerDataMgr.saveData.truckEquippableNumList[index] - 1 == 0)
            {
                playerDataMgr.saveData.truckEquippableList.Remove(id);
                playerDataMgr.saveData.truckEquippableNumList.RemoveAt(index);
            }
            else
            {
                playerDataMgr.saveData.truckEquippableNumList[index] -= 1;
                Debug.Log($"value : {playerDataMgr.saveData.truckEquippableNumList[index]}");
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            if (playerDataMgr.truckEquippablesNum[id] - 1 == 0)
            {
                //현재 데이터.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                Destroy(truckObjs[currentKey]);
                truckObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                truckWeaponNumInfo[currentKey] -= 1;
                var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] -= 1;
            }
        }
        else if (currentKind == WarehouseKind.Bag)
        {
            var id = playerDataMgr.equippableList[currentKey].id;
            var index = currentIndex;
            int firstIndex = playerDataMgr.saveData.bagEquippableFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagEquippableLastIndex[index];

            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagEquippableList[i] != id) continue;

                containIndex = i;
            }

            if (playerDataMgr.saveData.bagEquippableNumList[containIndex] - 1 == 0)
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
            else playerDataMgr.saveData.bagEquippableNumList[containIndex] -= 1;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            if (bagWeaponNumInfo[id] - 1 == 0)
            {
                //현재 데이터.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);
                Destroy(bagObjs[currentKey]);
                bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                bagWeaponNumInfo[currentKey] -= 1;
                var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= 1;
            }
        }
        ClosePopup();
    }

    public void Disarm()
    {
        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null && currentEquipKind == EquipKind.MainWeapon
            || disarmMode == 0)
        {
            mainWeaponTxt.text = "비어있음";

            var id = playerDataMgr.saveData.mainWeapon[currentIndex];
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
            playerDataMgr.saveData.mainWeapon[currentIndex] = null;
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = null;

            //json.
            int index = 0;
            if (!playerDataMgr.saveData.truckEquippableList.Contains(id))
            {
                playerDataMgr.saveData.truckEquippableList.Add(id);
                playerDataMgr.saveData.truckEquippableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
                playerDataMgr.saveData.truckEquippableNumList[index] += 1;
            }

            //playerDataMgr.
            if (!playerDataMgr.truckEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                truckWeaponInfo.Add(id, weapon);
                truckWeaponNumInfo.Add(id, 1);

                //var go = Instantiate(truckPrefab, truckContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = weapon.name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"1개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = id;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                //truckObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, 1);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[id] += 1;
                //var child = truckObjs[id].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckWeaponNumInfo[id]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] += 1;
            }
        }
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null && currentEquipKind == EquipKind.SubWeapon
            || disarmMode == 1)
        {
            subWeaponTxt.text = "비어있음";

            var id = playerDataMgr.saveData.subWeapon[currentIndex];
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
            playerDataMgr.saveData.subWeapon[currentIndex] = null;
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = null;

            //json.
            int index = 0;
            if (!playerDataMgr.saveData.truckEquippableList.Contains(id))
            {
                playerDataMgr.saveData.truckEquippableList.Add(id);
                playerDataMgr.saveData.truckEquippableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.truckEquippableList.IndexOf(id);
                playerDataMgr.saveData.truckEquippableNumList[index] += 1;
            }

            //playerDataMgr.
            if (!playerDataMgr.truckEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                truckWeaponInfo.Add(id, weapon);
                truckWeaponNumInfo.Add(id, 1);

                //var go = Instantiate(truckPrefab, truckContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = weapon.name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"1개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = id;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                //truckObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, 1);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[id] += 1;
                //var child = truckObjs[id].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckWeaponNumInfo[id]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] += 1;
            }
        }
        var currentMode = (int)trunkMode;
        DisplayTruckItem(currentMode);
    }

    public void DisplayTruckItem(int index)
    {
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        //버튼 초기화.
        foreach (var element in trunkButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
        }

        switch (index)
        {
            case 0:
                trunkMode = TrunkMode.All;
                break;
            case 1:
                trunkMode = TrunkMode.Equippables;
                break;
            case 2:
                trunkMode = TrunkMode.Consumables;
                break;
            case 3:
                trunkMode = TrunkMode.OtherItems;
                break;
        }
        trunkButtons[index].GetComponent<Image>().color = Color.red;

        if (truckObjs.Count != 0)
        {
            foreach (var element in truckObjs)
            {
                Destroy(element.Value);
            }
            truckObjs.Clear();
            truckContents.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        trunkCurrentWeight = 0;
        trunkTotalWeight = 0;

        foreach (var element in playerDataMgr.truckEquippables)
        {
            if (trunkMode != TrunkMode.Consumables && trunkMode != TrunkMode.OtherItems)
            {
                var go = Instantiate(truckPrefab, truckContents.transform);
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}개";

                truckObjs.Add(element.Key, go);
            }
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.truckConsumables)
        {
            if (trunkMode != TrunkMode.Equippables && trunkMode != TrunkMode.OtherItems)
            {
                var go = Instantiate(truckPrefab, truckContents.transform);
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{playerDataMgr.truckConsumablesNum[element.Key]}개";

                truckObjs.Add(element.Key, go);
            }
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
        }

        foreach (var element in playerDataMgr.truckOtherItems)
        {
            if (trunkMode != TrunkMode.Equippables && trunkMode != TrunkMode.Consumables)
            {
                var go = Instantiate(truckPrefab, truckContents.transform);
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

                var child = go.transform.GetChild(0).gameObject;
                child.GetComponent<Text>().text = element.Value.name;
                child = go.transform.GetChild(1).gameObject;
                child.GetComponent<Text>().text = $"{playerDataMgr.truckOtherItemsNum[element.Key]}개";

                truckObjs.Add(element.Key, go);
            }
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

        if (playerDataMgr.saveData.currentCar != null)
        {
            var key = playerDataMgr.saveData.currentCar;
            trunkTotalWeight = playerDataMgr.truckList[key].weight;
        }
        trunkWeightTxt.text = $"무게 {trunkCurrentWeight}/{trunkTotalWeight}";

        trunkBullet5Txt.text = $"5탄x{bullet5Num.ToString("D3")}";
        trunkBullet7Txt.text = $"7탄x{bullet7Num.ToString("D3")}";
        trunkBullet9Txt.text = $"9탄x{bullet9Num.ToString("D3")}";
        trunkBullet45Txt.text = $"45탄x{bullet45Num.ToString("D3")}";
        trunkBullet12Txt.text = $"12게이지x{bullet12Num.ToString("D3")}";
    }

    public void DisplayBagItem(int index)
    {
        //버튼 초기화.
        foreach (var element in bagButtons)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
        }

        if (currentIndex == -1)
        {
            var charName = GameObject.Find("Char Name").GetComponent<TextMeshProUGUI>();
            charName.text = "Character";

            bagWeightTxt.text = $"무게 {0}/{0}";

            bagBullet5Txt.text = $"5탄x{0.ToString("D3")}";
            bagBullet7Txt.text = $"7탄x{0.ToString("D3")}";
            bagBullet9Txt.text = $"9탄x{0.ToString("D3")}";
            bagBullet45Txt.text = $"45탄x{0.ToString("D3")}";
            bagBullet12Txt.text = $"12게이지x{0.ToString("D3")}";

            bagButtons[0].GetComponent<Image>().color = Color.red;

            return;
        }
        //0.All
        //1.Equippables
        //2.Consumables
        //3.OtherItems

        switch (index)
        {
            case 0:
                bagMode = BagMode.All;
                break;
            case 1:
                bagMode = BagMode.Equippables;
                break;
            case 2:
                bagMode = BagMode.Consumables;
                break;
            case 3:
                bagMode = BagMode.OtherItems;
                break;
        }
        bagButtons[index].GetComponent<Image>().color = Color.red;

        if (bagObjs.Count != 0)
        {
            foreach (var element in bagObjs)
            {
                Destroy(element.Value);
            }
            bagObjs.Clear();
            bagContents.transform.DetachChildren();
        }

        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        bagCurrentWeight = 0;
        bagTotalWeight = 0;
        foreach (var element in playerDataMgr.currentSquad[currentIndex].bag)
        {
            if (playerDataMgr.equippableList.ContainsKey(element.Key))
            {
                if (bagMode != BagMode.Consumables && bagMode != BagMode.OtherItems)
                {
                    var go = Instantiate(bagPrefab, bagContents.transform);
                    var button = go.AddComponent<Button>();
                    button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Bag); });

                    var child = go.transform.GetChild(0).gameObject;
                    child.GetComponent<Text>().text = $"{element.Value}개";

                    bagObjs.Add(element.Key, go);
                }
                bagCurrentWeight += (playerDataMgr.equippableList[element.Key].weight * element.Value);
            }
            if (playerDataMgr.consumableList.ContainsKey(element.Key))
            {
                if (bagMode != BagMode.Equippables && bagMode != BagMode.OtherItems)
                {
                    var go = Instantiate(bagPrefab, bagContents.transform);
                    var button = go.AddComponent<Button>();
                    button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Bag); });

                    var child = go.transform.GetChild(0).gameObject;
                    child.GetComponent<Text>().text = $"{element.Value}개";

                    bagObjs.Add(element.Key, go);
                }
                bagCurrentWeight += (playerDataMgr.consumableList[element.Key].weight * element.Value);
            }
            if (playerDataMgr.otherItemList.ContainsKey(element.Key))
            {
                if (bagMode != BagMode.Equippables && bagMode != BagMode.Consumables)
                {
                    var go = Instantiate(bagPrefab, bagContents.transform);
                    var button = go.AddComponent<Button>();
                    button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Bag); });

                    var child = go.transform.GetChild(0).gameObject;
                    child.GetComponent<Text>().text = $"{element.Value}개";

                    bagObjs.Add(element.Key, go);
                }
                bagCurrentWeight += (int.Parse(playerDataMgr.otherItemList[element.Key].weight) * element.Value);

                switch (element.Key)
                {
                    case "BUL_0004":
                        bullet5Num += element.Value;
                        break;
                    case "BUL_0005":
                        bullet7Num += element.Value;
                        break;
                    case "BUL_0002":
                        bullet9Num += element.Value;
                        break;
                    case "BUL_0003":
                        bullet45Num += element.Value;
                        break;
                    case "BUL_0001":
                        bullet12Num += element.Value;
                        break;
                }
            }
        }

        var level = playerDataMgr.currentSquad[currentIndex].bagLevel;
        switch (level)
        {
            case 1:
                bagTotalWeight = playerDataMgr.bagList["BAG_0001"].weight;
                break;
        }
        bagWeightTxt.text = $"무게 {bagCurrentWeight}/{bagTotalWeight}";

        bagBullet5Txt.text = $"5탄x{bullet5Num.ToString("D3")}";
        bagBullet7Txt.text = $"7탄x{bullet7Num.ToString("D3")}";
        bagBullet9Txt.text = $"9탄x{bullet9Num.ToString("D3")}";
        bagBullet45Txt.text = $"45탄x{bullet45Num.ToString("D3")}";
        bagBullet12Txt.text = $"12게이지x{bullet12Num.ToString("D3")}";
    }

    public void NumAdjustment()
    {
        itemNumTxt.text = $"{slider.value}개";
    }

    public void SelectItem(string key, WarehouseKind kind)
    {
        if (!Add10Button.activeSelf) Add10Button.SetActive(true);
        if (!Add100Button.activeSelf) Add100Button.SetActive(true);
        if (!AddMaxButton.activeSelf) AddMaxButton.SetActive(true);

        if (currentKey != null)
        {
            if (kind == WarehouseKind.Trunk && truckObjs.ContainsKey(currentKey))
            {
                var image = truckObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
            else if (kind == WarehouseKind.Bag && bagObjs.ContainsKey(currentKey))
            {
                var image = bagObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }

        currentKey = key;
        currentKind = kind;

        if (playerDataMgr.equippableList.ContainsKey(currentKey))
        {
            if (!mainWeaponButton.activeSelf) mainWeaponButton.SetActive(true);
            if (!subWeaponButton.activeSelf) subWeaponButton.SetActive(true);
        }
        else if (playerDataMgr.otherItemList.ContainsKey(currentKey))
        {
            if (mainWeaponButton.activeSelf) mainWeaponButton.SetActive(false);
            if (subWeaponButton.activeSelf) subWeaponButton.SetActive(false);
        }

        if (currentKind == WarehouseKind.Trunk)
        {
            var child = bagButton.transform.GetChild(0).gameObject;
            if (!child.GetComponent<Text>().text.Equals("가방")) child.GetComponent<Text>().text = "가방";

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
            else if (truckOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = truckOtherItemInfo[currentKey].name;
                slider.maxValue = truckOtherItemNumInfo[currentKey];
            }
        }
        else if (currentKind == WarehouseKind.Bag)
        {
            var child = bagButton.transform.GetChild(0).gameObject;
            if (!child.GetComponent<Text>().text.Equals("차고")) child.GetComponent<Text>().text = "차고";

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
            else if (bagOtherItemInfo.ContainsKey(currentKey))
            {
                itemNameTxt.text = bagOtherItemInfo[currentKey].name;
                slider.maxValue = bagOtherItemNumInfo[currentKey];
            }
        }
        
        slider.value = 0;
        itemNumTxt.text = $"0개";
        OpenPopup();
    }

    public void Move()
    {
        if (currentIndex == -1) return;

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
            itemNumTxt.text = $"{slider.value}개";
        }
        else
        {
            slider.value += plus;
            itemNumTxt.text = $"{slider.value}개";
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

    // 개별 인벤토리 출력
    private void SelectCharInventory(int key)
    {
        if (currentIndex == key) return;

        if (bagWeaponInfo.Count != 0) bagWeaponInfo.Clear();
        if (bagWeaponNumInfo.Count != 0) bagWeaponNumInfo.Clear();
        if (bagConsumableInfo.Count != 0) bagConsumableInfo.Clear();
        if (bagConsumableNumInfo.Count != 0) bagConsumableNumInfo.Clear();
        if (bagOtherItemInfo.Count != 0) bagOtherItemInfo.Clear();
        if (bagOtherItemNumInfo.Count != 0) bagOtherItemNumInfo.Clear();

        //버튼 초기화.
        foreach (var element in BattleSqudeObj)
        {
            if (element.Value.GetComponent<Image>().color == Color.red)
                element.Value.GetComponent<Image>().color = Color.white;
        }
        BattleSqudeObj[key].GetComponent<Image>().color = Color.red;

        currentIndex = key;
        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
        {
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
            mainWeaponTxt.text = weapon.name;
        }
        else mainWeaponTxt.text = "비어있음";

        if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
        {
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
            subWeaponTxt.text = weapon.name;
        }
        else subWeaponTxt.text = "비어있음";

        // 임시로
        var charName = GameObject.Find("Char Name").GetComponent<TextMeshProUGUI>();
        charName.text = playerDataMgr.battleSquad[key].character.name;

        DisplayBagItem(0);
    }

    public void MoveToBag(int itemNum)
    {
        if (currentKey == null) return;
        if (itemNum == 0) return;
        OpenPopup();

        if (truckWeaponInfo.ContainsKey(currentKey))
        {
            var weight = truckWeaponInfo[currentKey].weight;
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

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
                //현재데이터 관련.
                bagWeaponInfo.Add(currentKey, truckWeaponInfo[currentKey]);
                bagWeaponNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagWeaponNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            var currentMode = (int)bagMode;
            DisplayBagItem(currentMode);

            if (playerDataMgr.truckEquippablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckWeaponInfo.Remove(currentKey);
                truckWeaponNumInfo.Remove(currentKey);
                //Destroy(truckObjs[currentKey]);
                //truckObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Remove(id);
                playerDataMgr.truckEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                truckWeaponNumInfo[currentKey] -= itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] -= itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
        else if (truckConsumableInfo.ContainsKey(currentKey))
        {
            var weight = truckConsumableInfo[currentKey].weight;
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

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

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagConsumableNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            var currentMode = (int)bagMode;
            DisplayBagItem(currentMode);

            if (playerDataMgr.truckConsumablesNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckConsumableInfo.Remove(currentKey);
                truckConsumableNumInfo.Remove(currentKey);
                //Destroy(truckObjs[currentKey]);
                //truckObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Remove(id);
                playerDataMgr.truckConsumablesNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                truckConsumableNumInfo[currentKey] -= itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id] -= itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
        else if (truckOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(truckOtherItemInfo[currentKey].weight);
            if (bagCurrentWeight + weight * itemNum > bagTotalWeight) return;

            //json.
            int index = 0;
            var id = truckOtherItemInfo[currentKey].id;

            index = playerDataMgr.currentSquad[currentIndex].saveId;
            int firstIndex = playerDataMgr.saveData.bagOtherItemFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagOtherItemLastIndex[index];

            bool contain = false;
            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagOtherItemList[i] != id) continue;

                contain = true;
                containIndex = i;
            }

            if (!contain)
            {
                playerDataMgr.saveData.bagOtherItemList.Insert(lastIndex, id);
                playerDataMgr.saveData.bagOtherItemNumList.Insert(lastIndex, itemNum);
                playerDataMgr.saveData.bagOtherItemLastIndex[index]++;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagOtherItemFirstIndex[i]++;
                    playerDataMgr.saveData.bagOtherItemLastIndex[i]++;
                }
            }
            else
            {
                playerDataMgr.saveData.bagOtherItemNumList[containIndex] += itemNum;
            }

            index = playerDataMgr.saveData.truckOtherItemList.IndexOf(id);
            if (playerDataMgr.saveData.truckOtherItemNumList[index] - itemNum == 0)
            {
                playerDataMgr.saveData.truckOtherItemList.Remove(id);
                playerDataMgr.saveData.truckOtherItemNumList.RemoveAt(index);
            }
            else playerDataMgr.saveData.truckOtherItemNumList[index] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentSquad[currentIndex].bag.ContainsKey(id))
            {
                //현재데이터 관련.
                bagOtherItemInfo.Add(currentKey, truckOtherItemInfo[currentKey]);
                bagOtherItemNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(bagPrefab, bagContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Bag); });
                //bagObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                bagOtherItemNumInfo[currentKey] += itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] += itemNum;
            }
            var currentMode = (int)bagMode;
            DisplayBagItem(currentMode);

            if (playerDataMgr.truckOtherItemsNum[id] - itemNum == 0)
            {
                //현재 데이터.
                truckOtherItemInfo.Remove(currentKey);
                truckOtherItemNumInfo.Remove(currentKey);
                //Destroy(truckObjs[currentKey]);
                //truckObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItems.Remove(id);
                playerDataMgr.truckOtherItemsNum.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                truckOtherItemNumInfo[currentKey] -= itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItemsNum[id] -= itemNum;
            }
            currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);
        }
    }

    public void MoveToTruck(int itemNum)
    {
        if (currentKey == null) return;
        OpenPopup();

        if (bagWeaponInfo.ContainsKey(currentKey))
        {
            var weight = bagWeaponInfo[currentKey].weight;
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

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

                //var go = Instantiate(truckPrefab, truckContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagWeaponInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                //truckObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.truckEquippablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckWeaponNumInfo[currentKey] += itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckEquippablesNum[id] += itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (bagWeaponNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagWeaponInfo.Remove(currentKey);
                bagWeaponNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                bagWeaponNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagWeaponNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBagItem(currentMode);
        }
        else if (bagConsumableInfo.ContainsKey(currentKey))
        {
            var weight = bagConsumableInfo[currentKey].weight;
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

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

                //var go = Instantiate(truckPrefab, truckContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagConsumableInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                //truckObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumables.Add(id, playerDataMgr.consumableList[id]);
                playerDataMgr.truckConsumablesNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckConsumableNumInfo[currentKey] += itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckConsumablesNum[id] += itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (bagConsumableNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagConsumableInfo.Remove(currentKey);
                bagConsumableNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                bagConsumableNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagConsumableNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBagItem(currentMode);
        }
        else if (bagOtherItemInfo.ContainsKey(currentKey))
        {
            var weight = int.Parse(bagOtherItemInfo[currentKey].weight);
            if (trunkCurrentWeight + weight * itemNum > trunkTotalWeight) return;

            //json.
            int index;
            var id = bagOtherItemInfo[currentKey].id;
            if (!playerDataMgr.saveData.truckOtherItemList.Contains(id))
            {
                playerDataMgr.saveData.truckOtherItemList.Add(id);
                playerDataMgr.saveData.truckOtherItemNumList.Add(itemNum);
            }
            else
            {
                index = playerDataMgr.saveData.truckOtherItemList.IndexOf(id);
                playerDataMgr.saveData.truckOtherItemNumList[index] += itemNum;
            }
            
            index = currentIndex;
            int firstIndex = playerDataMgr.saveData.bagOtherItemFirstIndex[index];
            int lastIndex = playerDataMgr.saveData.bagOtherItemLastIndex[index];

            int containIndex = -1;
            for (int i = firstIndex; i < lastIndex; i++)
            {
                if (playerDataMgr.saveData.bagOtherItemList[i] != id) continue;

                containIndex = i;
            }

            if (playerDataMgr.saveData.bagOtherItemNumList[containIndex] - itemNum == 0)
            {
                playerDataMgr.saveData.bagOtherItemList.RemoveAt(containIndex);
                playerDataMgr.saveData.bagOtherItemNumList.RemoveAt(containIndex);

                playerDataMgr.saveData.bagOtherItemLastIndex[index]--;

                for (int i = index + 1; i < playerDataMgr.saveData.id.Count; i++)
                {
                    playerDataMgr.saveData.bagOtherItemFirstIndex[i]--;
                    playerDataMgr.saveData.bagOtherItemLastIndex[i]--;
                }
            }
            else playerDataMgr.saveData.bagOtherItemNumList[containIndex] -= itemNum;

            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.truckOtherItems.ContainsKey(id))
            {
                //현재데이터 관련.
                truckOtherItemInfo.Add(currentKey, bagOtherItemInfo[currentKey]);
                truckOtherItemNumInfo.Add(currentKey, itemNum);

                //var go = Instantiate(truckPrefab, truckContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = bagOtherItemInfo[currentKey].name;

                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{itemNum}개";

                //var button = go.AddComponent<Button>();
                //string selectedKey = currentKey;
                //button.onClick.AddListener(delegate { SelectItem(selectedKey, WarehouseKind.Trunk); });
                //truckObjs.Add(selectedKey, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItems.Add(id, playerDataMgr.otherItemList[id]);
                playerDataMgr.truckOtherItemsNum.Add(id, itemNum);
            }
            else
            {
                //현재데이터 관련.
                truckOtherItemNumInfo[currentKey] += itemNum;
                //var child = truckObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{truckOtherItemNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.truckOtherItemsNum[id] += itemNum;
            }
            var currentMode = (int)trunkMode;
            DisplayTruckItem(currentMode);

            if (bagOtherItemNumInfo[id] - itemNum == 0)
            {
                //현재 데이터.
                bagOtherItemInfo.Remove(currentKey);
                bagOtherItemNumInfo.Remove(currentKey);
                //Destroy(bagObjs[currentKey]);
                //bagObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag.Remove(id);

                currentKey = null;
                currentKind = WarehouseKind.None;
            }
            else
            {
                //현재 데이터.
                bagOtherItemNumInfo[currentKey] -= itemNum;
                //var child = bagObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{bagOtherItemNumInfo[currentKey]}개";

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentSquad[currentIndex].bag[id] -= itemNum;
            }
            currentMode = (int)bagMode;
            DisplayBagItem(currentMode);
        }
    }

    //창 관련.
    public void DisArmMode(int index)
    {
        if (currentIndex == -1) return;

        if (Add10Button.activeSelf) Add10Button.SetActive(false);
        if (Add100Button.activeSelf) Add100Button.SetActive(false);
        if (AddMaxButton.activeSelf) AddMaxButton.SetActive(false);

        switch (index)
        {
            case 0:
                if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon == null) return;
                disarmMode = index;
                mainWeaponObj.GetComponent<Image>().color = Color.red;
                var name = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name;
                itemNameTxt.text = $"{name}";
                break;
            case 1:
                if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon == null) return;
                disarmMode = index;
                subWeaponObj.GetComponent<Image>().color = Color.red;
                name = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name;
                itemNameTxt.text = $"{name}";
                break;
        }

        OpenPopup();
    }

    public void OpenPopup()
    {
        if (disarmMode != -1)
        {
            if (slider.gameObject.activeSelf) slider.gameObject.SetActive(false);
            if (itemNumTxt.gameObject.activeSelf) itemNumTxt.gameObject.SetActive(false);
            //if (mainWeaponButton.activeSelf) mainWeaponButton.SetActive(false);
            //if (subWeaponButton.activeSelf) subWeaponButton.SetActive(false);
            if (bagButton.activeSelf) bagButton.SetActive(false);
            if (!disarmButton.activeSelf) disarmButton.SetActive(true);
        }
        else
        {
            if (!slider.gameObject.activeSelf) slider.gameObject.SetActive(true);
            if (!itemNumTxt.gameObject.activeSelf) itemNumTxt.gameObject.SetActive(true);
            //if (!mainWeaponButton.activeSelf) mainWeaponButton.SetActive(true);
            //if (!subWeaponButton.activeSelf) subWeaponButton.SetActive(true);
            if (!bagButton.activeSelf) bagButton.SetActive(true);
            if (disarmButton.activeSelf) disarmButton.SetActive(false);
        }

        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        if (disarmMode == 0) mainWeaponObj.GetComponent<Image>().color = Color.white;
        else if (disarmMode == 1) subWeaponObj.GetComponent<Image>().color = Color.white;
        disarmMode = -1;

        if (popupWin.activeSelf) popupWin.SetActive(false);

        if (currentKind == WarehouseKind.Trunk)
        {
            if (truckObjs.ContainsKey(currentKey))
            {
                var image = truckObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }
        else if (currentKind == WarehouseKind.Bag)
        {
            if (bagObjs.ContainsKey(currentKey))
            {
                var image = bagObjs[currentKey].GetComponent<Image>();
                image.color = Color.white;
            }
        }

        currentKind = WarehouseKind.None;
        currentKey = null;
    }

    public void OpenInventoryWin()
    {
        var currentWinId = (int)Windows.MemberSelectPopup - 1;
        windowManager.windows[currentWinId].Close();

        if (!InventoryWin.activeSelf) InventoryWin.SetActive(true);
        currentWinId = (int)Windows.TruckPopup - 1;
        windowManager.Open(currentWinId);
    }

    public void CloseInventoryWin()
    {
        var currentWinId = (int)Windows.TruckPopup - 1;
        windowManager.windows[currentWinId].Close();

        currentWinId = (int)Windows.MemberSelectPopup - 1;
        windowManager.Open(currentWinId);
    }
}
