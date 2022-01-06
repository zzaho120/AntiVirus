using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    //private bool isBattle;
    //private NonBattlePopUps nonBattlePopUps;
    public WindowManager windowManager;
    private TimeController timeController;

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            // 렌더러가 활성화 되어있을때만 유효하게
            if (other.GetComponent<MeshRenderer>().enabled)
            {
                var windowId = (int)Windows.MonsterWindow - 1;
                var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;

                timeController.PauseTime();
                timeController.isPause = true;
            }

            // 나중에 전투 구현되면
            //if (!isBattle)
            //{
            //    Debug.Log("전투 발생");
            //    isBattle = true;
            //}
            //
            //else
            //    return;
        }
    }

    // Stay 시에도 전투발생 처리 ? 

    //public void CreateSquad()
    //{
    //    var windowId = (int)Windows.MemberSelectPopup - 1;
    //    var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    //}
}
