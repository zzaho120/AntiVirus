using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject carBoardingWin;
    public GameObject trunkWin;

    public TrunkMgr trunkMgr;
    public BoardingMgr boardingMgr;

    int carCenterLevel;
    int maxCarCapacity;
    public void Init()
    {
        carCenterLevel = playerDataMgr.saveData.carCenterLevel;
        Bunker carCenterLevelInfo = playerDataMgr.bunkerList["BUN_0003"];
        switch (carCenterLevel)
        {
            case 1:
                maxCarCapacity = carCenterLevelInfo.level1;
                break;
            case 2:
                maxCarCapacity = carCenterLevelInfo.level2;
                break;
            case 3:
                maxCarCapacity = carCenterLevelInfo.level3;
                break;
            case 4:
                maxCarCapacity = carCenterLevelInfo.level4;
                break;
            case 5:
                maxCarCapacity = carCenterLevelInfo.level5;
                break;
        }
        int currentCarNum = playerDataMgr.saveData.cars.Count;

        trunkMgr.playerDataMgr = playerDataMgr;
        trunkMgr.Init();

        boardingMgr.playerDataMgr = playerDataMgr;
        boardingMgr.Init();

        OpenMainWin();
    }

    //Ã¢ °ü·Ã.
    public void OpenMainWin()
    {
        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (carBoardingWin.activeSelf) carBoardingWin.SetActive(false);
        if (trunkWin.activeSelf) trunkWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
        if (!bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(true);
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
}
