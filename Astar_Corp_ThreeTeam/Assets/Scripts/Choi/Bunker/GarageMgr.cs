using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageMgr : MonoBehaviour
{
    public GameObject carSelectWin;
    public GameObject TrunkWin;
    public GameObject carBoardingWin;

    public void OpenCarSelectWin()
    {
        carSelectWin.SetActive(true);
    }

    public void CloseCarSelectWin()
    {
        carSelectWin.SetActive(false);
    }

    public void OpenTrunkWin()
    {
        TrunkWin.SetActive(true);
    }

    public void CloseTrunkWin()
    {
        TrunkWin.SetActive(false);
    }

    public void OpenCarBoardingWin()
    {
        carBoardingWin.SetActive(true);
    }

    public void CloseCarBoardingWin()
    {
        carBoardingWin.SetActive(false);
    }
}
