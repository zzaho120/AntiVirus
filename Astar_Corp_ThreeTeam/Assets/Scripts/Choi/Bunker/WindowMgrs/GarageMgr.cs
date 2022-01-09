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

    public void Init()
    {
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
