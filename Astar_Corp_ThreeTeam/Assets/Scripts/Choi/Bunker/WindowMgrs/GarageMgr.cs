using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject carSelectWin;
    public GameObject TrunkWin;
    public GameObject carBoardingWin;

    public TrunkMgr trunkMgr;
    public BoardingMgr boardingMgr;

    int garageLevel;
    int maxCarCapacity;
    public void Init()
    {
        garageLevel = playerDataMgr.saveData.garageLevel;
        Bunker garageLevelInfo = playerDataMgr.bunkerList["BUN_0003"];
        switch (garageLevel)
        {
            case 1:
                maxCarCapacity = garageLevelInfo.level1;
                break;
            case 2:
                maxCarCapacity = garageLevelInfo.level2;
                break;
            case 3:
                maxCarCapacity = garageLevelInfo.level3;
                break;
            case 4:
                maxCarCapacity = garageLevelInfo.level4;
                break;
            case 5:
                maxCarCapacity = garageLevelInfo.level5;
                break;
        }
        int currentCarNum = playerDataMgr.saveData.cars.Count;

        trunkMgr.playerDataMgr = playerDataMgr;
        trunkMgr.Init();

        boardingMgr.playerDataMgr = playerDataMgr;
        boardingMgr.Init();

        OpenCarSelectWin();
    }

    public void OpenCarSelectWin()
    {
        if (TrunkWin.activeSelf) TrunkWin.SetActive(false);
        if (carBoardingWin.activeSelf) carBoardingWin.SetActive(false);
        carSelectWin.SetActive(true);
    }

    public void CloseCarSelectWin()
    {
        carSelectWin.SetActive(false);
    }

    public void OpenTrunkWin()
    {
        carSelectWin.SetActive(false);
        TrunkWin.SetActive(true);
    }

    public void CloseTrunkWin()
    {
        TrunkWin.SetActive(false);
        carSelectWin.SetActive(true);
    }

    public void OpenCarBoardingWin()
    {
        TrunkWin.SetActive(false);
        carBoardingWin.SetActive(true);
    }

    public void CloseCarBoardingWin()
    {
        carBoardingWin.SetActive(false);
        TrunkWin.SetActive(true);
    }
}
