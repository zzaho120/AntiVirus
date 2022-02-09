using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrunkWinMgr : ModalWindowManager
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    public GameObject truckItemPrefab;
    public GameObject truckItemList;

    public Dictionary<string, GameObject> TruckUnitGOs = new Dictionary<string, GameObject>();

    Dictionary<string, Weapon> truckWeaponInfo = new Dictionary<string, Weapon>();
    //Dictionary<string, int> truckWeaponNumInfo = new Dictionary<string, int>();
    Dictionary<string, Consumable> truckConsumableInfo = new Dictionary<string, Consumable>();
    //Dictionary<string, int> truckConsumableNumInfo = new Dictionary<string, int>();
    Dictionary<string, OtherItem> truckOtherItemInfo = new Dictionary<string, OtherItem>();
    //Dictionary<string, int> truckOtherItemNumInfo = new Dictionary<string, int>();

    public void Init()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        Debug.Log("Init");

        // 임시 데이터 넣기
        Weapon weapon1 = new Weapon();
        weapon1 = soMgr.GetEquippable("WEP_0003");
        Weapon weapon2 = new Weapon();
        weapon2 = soMgr.GetEquippable("WEP_0010");
        
        if (!playerDataMgr.truckEquippables.ContainsKey("0"))
            playerDataMgr.truckEquippables.Add("0", weapon1);
        
        if (!playerDataMgr.truckEquippables.ContainsKey("1"))
            playerDataMgr.truckEquippables.Add("1", weapon2);

        PrintTrunkItems();
    }

    public void PrintTrunkItems()
    {
        // 이전 정보 삭제
        if (TruckUnitGOs.Count != 0)
        {
            foreach (var element in TruckUnitGOs)
            {
                Destroy(element.Value);
            }
            TruckUnitGOs.Clear();
        }

        // 생성
        foreach (var element in playerDataMgr.truckEquippables)
        {
            //truckWeaponInfo.Add(element.Key, element.Value);
            //truckWeaponNumInfo.Add(element.Key, playerDataMgr.truckEquippablesNum[element.Key]);

            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();
            //button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            go.GetComponentInChildren<Text>().text = element.Value.name;
            //var child = go.transform.GetChild(0).gameObject;
            //child.GetComponent<Text>().text = element.Value.name;
            //child = go.transform.GetChild(1).gameObject;
            //child.GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}개";

            TruckUnitGOs.Add(element.Key, go);
        }
        foreach (var element in playerDataMgr.truckConsumables)
        {
            //truckWeaponInfo.Add(element.Key, element.Value);
            //truckWeaponNumInfo.Add(element.Key, playerDataMgr.truckEquippablesNum[element.Key]);

            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();
            //button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });

            go.transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            go.GetComponentInChildren<Text>().text = element.Value.name;
            //var child = go.transform.GetChild(0).gameObject;
            //child.GetComponent<Text>().text = element.Value.name;
            //child = go.transform.GetChild(1).gameObject;
            //child.GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}개";

            TruckUnitGOs.Add(element.Key, go);
        }
        foreach (var element in playerDataMgr.truckOtherItems)
        {
            //truckWeaponInfo.Add(element.Key, element.Value);
            //truckWeaponNumInfo.Add(element.Key, playerDataMgr.truckEquippablesNum[element.Key]);

            var go = Instantiate(truckItemPrefab, truckItemList.transform);
            var button = go.AddComponent<Button>();
            //button.onClick.AddListener(delegate { SelectItem(element.Key, WarehouseKind.Trunk); });
            go.transform.GetChild(0).GetComponent<Image>().sprite = element.Value.img;
            go.GetComponentInChildren<Text>().text = element.Value.name;
            //var child = go.transform.GetChild(0).gameObject;
            //child.GetComponent<Text>().text = element.Value.name;
            //child = go.transform.GetChild(1).gameObject;
            //child.GetComponent<Text>().text = $"{playerDataMgr.truckEquippablesNum[element.Key]}개";

            TruckUnitGOs.Add(element.Key, go);
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
