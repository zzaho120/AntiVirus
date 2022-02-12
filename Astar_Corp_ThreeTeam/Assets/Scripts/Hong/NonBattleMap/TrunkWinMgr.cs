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
    public GameObject truckItemPrefab;
    public GameObject truckItemList;

    // Truck Members
    public GameObject truckMemberPrefab;
    public GameObject truckMemberList;

    // Weight
    public TextMeshProUGUI totalWeight;
    private int trunkCurrentWeight;

    private bool isInit;
    public bool isBattlePopup;

    public Dictionary<string, GameObject> TruckUnitGOs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();


    private void Update()
    {
        // [����] Ʈ�� ���� ����
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

        //// �ӽ� ������ �ֱ�
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

        if (isBattlePopup)
        {

        }

        PrintTrunkItems();
    }


    public void PrintTrunkItems()
    {
        // ���� ���� ����
        if (TruckUnitGOs.Count != 0)
        {
            foreach (var element in TruckUnitGOs)
            {
                Destroy(element.Value);
            }
            TruckUnitGOs.Clear();
        }

        // ����
        // 1. ����
        foreach (var element in playerDataMgr.truckEquippables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();

            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.price.ToString();

            TruckUnitGOs.Add(element.Key, go);
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckEquippablesNum[element.Key]);
        }
        // �Ҹ�ǰ
        foreach (var element in playerDataMgr.truckConsumables)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();

            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckConsumablesNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.price.ToString();

            TruckUnitGOs.Add(element.Key, go);
            trunkCurrentWeight += (element.Value.weight * playerDataMgr.truckConsumablesNum[element.Key]);
        }
        // 3. ��Ÿ��
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();

            // �̹��� 0
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            // �̸� 1
            go.GetComponentInChildren<Text>().text = element.Value.name;
            // ���� 2 
            go.transform.GetChild(2).GetComponent<Text>().text = $"{playerDataMgr.truckOtherItemsNum[element.Key]}��";
            // ���� 3
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.weight.ToString();
            // ���� 4
            go.transform.GetChild(3).GetComponent<Text>().text = element.Value.price.ToString();

            TruckUnitGOs.Add(element.Key, go);
            trunkCurrentWeight += (int.Parse(element.Value.weight) * playerDataMgr.truckOtherItemsNum[element.Key]);
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
}
