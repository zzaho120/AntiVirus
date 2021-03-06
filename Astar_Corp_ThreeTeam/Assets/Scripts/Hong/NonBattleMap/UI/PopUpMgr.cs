using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpMgr : MonoBehaviour
{
    public WindowManager windowManager;

    public void OpenBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenMonsterPopup()
    {
        var windowId = (int)Windows.MonsterWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    // value 값에 따라서 뜨는 문구 다르게 (몬스터 구분하기)
    //public void OpenFootprintPopup(int value)
    public void OpenFootprintPopup()
    {
        var windowId = (int)Windows.FootprintInfo - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void MoveToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void MoveToLab()
    {
        Debug.Log("연구소로 이동");
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}
