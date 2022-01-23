using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject mainWin;
    public GameObject buyWin;
    public GameObject trunkWin;
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

        //Ã¢ °ü·Ã.
        if (carBoardingWin.activeSelf) carBoardingWin.SetActive(false);
        if (trunkWin.activeSelf) trunkWin.SetActive(false);
        if(buyWin.activeSelf) buyWin.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
    }

    public void OpenCarBoardingWin()
    {
        mainWin.SetActive(false);
        carBoardingWin.SetActive(true);
    }

    public void CloseCarBoardingWin()
    {
        carBoardingWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenTrunkWin()
    {
        mainWin.SetActive(false);
        trunkWin.SetActive(true);
    }

    public void CloseTrunkWin()
    {
        trunkWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenBuyWin()
    {
        mainWin.SetActive(false);
        buyWin.SetActive(true);
    }

    public void CloseBuyWin()
    {
        buyWin.SetActive(false);
        mainWin.SetActive(true);
    }
}
