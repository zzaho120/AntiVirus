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
    // True: ���� ����, False: �÷��̾� ����

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
           

            // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ� // �ش����� ��� Off
            if (other.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
            {
                 //���� ���� �յ� �Ǵ�
                GameObject target = other.gameObject;
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                // �÷��̾� ��
                if (Vector3.Dot(transform.forward, dirToTarget) > 0) 
                {
                    Debug.Log("��");
                    isMonsterAtk = false;
                }
                // �÷��̾� ��
                else 
                {
                    Debug.Log("��");
                    isMonsterAtk = true;
                }
                PlayerDataMgr.Instance.isMonsterAtk = isMonsterAtk;
                PlayerDataMgr.Instance.worldMonster = other.GetComponent<WorldMonsterChar>();
                // ��ü�� �Ͻ�����
                timeController.Pause();
                timeController.isPause = true;

                SetMonsterInfo(other.GetComponent<WorldMonsterChar>());

                //StringBuilder info = new StringBuilder();
                //info.Append(monsterChar.monsterStat.monster.name);
                //info.Append(monsterChar.monsterStat.nonBattleMonster.battleMinNum);
                //info.Append(monsterChar.monsterStat.nonBattleMonster.battleMaxNum);
                //info.Append(monsterChar.monsterStat.virus);

                // / ���� �˾�â ����
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
