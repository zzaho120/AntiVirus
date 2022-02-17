using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StartBattle : MonoBehaviour
{
    public WindowManager windowManager;
    private TimeController timeController;
    public GameObject monsterInfo;
    private WorldMonsterChar monsterChar;

    public bool isTestMode;

    [HideInInspector]
    public bool isMonsterAtk;
    // True: 몬스터 선공, False: 플레이어 선공

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
           

            // 렌더러가 활성화 되어있을때만 유효하게 // 해당조건 잠깐 Off
            if (other.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
            {
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
                PlayerDataMgr.Instance.worldMonster = other.GetComponent<WorldMonsterChar>();
                // 전체맵 일시정지
                timeController.Pause();
                timeController.isPause = true;

                SetMonsterInfo(other.GetComponent<WorldMonsterChar>());

                //StringBuilder info = new StringBuilder();
                //info.Append(monsterChar.monsterStat.monster.name);
                //info.Append(monsterChar.monsterStat.nonBattleMonster.battleMinNum);
                //info.Append(monsterChar.monsterStat.nonBattleMonster.battleMaxNum);
                //info.Append(monsterChar.monsterStat.virus);

                // / 전투 팝업창 띄우기
                var windowId = (int)Windows.MonsterWindow - 1;
                var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
            }
        }
    }

    public void SetMonsterInfo(WorldMonsterChar monsterChar)
    {
        monsterInfo.transform.GetChild(0).GetComponent<Text>().text = monsterChar.monsterStat.monster.name;
        monsterInfo.transform.GetChild(1).GetComponent<Image>().sprite = monsterChar.monsterImg;
        monsterInfo.transform.GetChild(2).GetComponent<Text>().text =
            $"{monsterChar.monsterStat.nonBattleMonster.battleMinNum} ~ {monsterChar.monsterStat.nonBattleMonster.battleMaxNum}";
        monsterInfo.transform.GetChild(3).GetComponent<Text>().text = monsterChar.monsterStat.virus.name;
    }
}
