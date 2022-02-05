using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    public WindowManager windowManager;
    private TimeController timeController;

    [HideInInspector]
    public bool isMonsterAtk;
    // True: 몬스터 선공, False: 플레이어 선공

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 렌더러가 활성화 되어있을때만 유효하게 // 해당조건 잠깐 Off
            //if (other.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
            //{
                 //몬스터 공격 앞뒤 판단
                GameObject target = other.gameObject;
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                // 플레이어 앞
                if (Vector3.Dot(transform.forward, dirToTarget) > 0) 
                {
                    Debug.Log("앞");
                    isMonsterAtk = false;
                }
                // 플레이어 뒤
                else 
                {
                    Debug.Log("뒤");
                    isMonsterAtk = true;
                }
                PlayerDataMgr.Instance.isMonsterAtk = isMonsterAtk;

                // 전체맵 일시정지
                timeController.PauseTime();
                timeController.isPause = true;

                // / 전투 팝업창 띄우기
                var windowId = (int)Windows.MonsterWindow - 1;
                var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
            //}
        }
    }
}
