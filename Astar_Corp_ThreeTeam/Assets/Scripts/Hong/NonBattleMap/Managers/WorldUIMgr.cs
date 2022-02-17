using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIMgr : MonoBehaviour
{
    private GameObject screen;

    [Header("UI")]
    public SquadStatusMgr squadStatusMgr;
    public PrintTruckUI printTruckUI;
    public TrunkWinMgr trunkWinMgr;

    private void Start()
    {
        //// 임시 Sqaud 데이타 넣기
        ////===================================================
        //CharacterStats temp1 = new CharacterStats();
        //temp1.character = soMgr.GetCharacter("CHAR_0001");
        //CharacterStats temp2 = new CharacterStats();
        //temp2.character = soMgr.GetCharacter("CHAR_0003");
        //
        //if (!playerDataMgr.currentSquad.ContainsKey(1))
        //    playerDataMgr.currentSquad.Add(1, temp1);
        //
        //if (!playerDataMgr.currentSquad.ContainsKey(2))
        //    playerDataMgr.currentSquad.Add(2, temp2);
        //
        //playerDataMgr.currentSquad[1].Init();
        //playerDataMgr.currentSquad[1].level = 3;
        //playerDataMgr.currentSquad[2].Init();
        //playerDataMgr.currentSquad[2].level = 5;
        //playerDataMgr.currentSquad[2].currentHp = 10;
        //
        ////playerDataMgr.currentSquad[1].weapon.subWeapon = soMgr.GetEquippable("WEP_0005"); 
        //
        //if (!playerDataMgr.boardingSquad.ContainsKey(0))
        //    playerDataMgr.boardingSquad.Add(0, 1);
        //
        //if (!playerDataMgr.boardingSquad.ContainsKey(1))
        //    playerDataMgr.boardingSquad.Add(1, 2);
        ////===================================================

        //// 임시 무기 데이터 넣기
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
    }
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
        screen = GameObject.Find("Screen");

        // SquadStatusMgr
        squadStatusMgr = screen.GetComponentInChildren<SquadStatusMgr>();
        squadStatusMgr.Init();

        // PrintTruckUI
        printTruckUI = screen.GetComponentInChildren<PrintTruckUI>();
        printTruckUI.Init();

        // TrunkWinMGr
        trunkWinMgr = screen.GetComponentInChildren<TrunkWinMgr>();
        StartCoroutine(CoTrunkWinInit());
    }

    public void UpdateUI()
    {
        // SquadStatusMgr
        squadStatusMgr.SquadUpdate();
    }

    private IEnumerator CoTrunkWinInit()
    {
        yield return new WaitForSeconds(0.5f);
        trunkWinMgr.Init();
    }
}
