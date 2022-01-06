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
            // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
            if (other.GetComponent<MeshRenderer>().enabled)
            {
                var windowId = (int)Windows.MonsterWindow - 1;
                var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;

                timeController.PauseTime();
                timeController.isPause = true;
            }

            // ���߿� ���� �����Ǹ�
            //if (!isBattle)
            //{
            //    Debug.Log("���� �߻�");
            //    isBattle = true;
            //}
            //
            //else
            //    return;
        }
    }

    // Stay �ÿ��� �����߻� ó�� ? 

    //public void CreateSquad()
    //{
    //    var windowId = (int)Windows.MemberSelectPopup - 1;
    //    var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    //}
}
