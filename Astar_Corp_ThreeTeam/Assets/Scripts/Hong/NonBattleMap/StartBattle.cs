using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    public WindowManager windowManager;
    private TimeController timeController;

    [HideInInspector]
    public bool isMonsterAtk;
    // True: ���� ����, False: �÷��̾� ����

    private void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ� // �ش����� ��� Off
            //if (other.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
            //{
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

                // ��ü�� �Ͻ�����
                timeController.PauseTime();
                timeController.isPause = true;

                // / ���� �˾�â ����
                var windowId = (int)Windows.MonsterWindow - 1;
                var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
            //}
        }
    }
}
