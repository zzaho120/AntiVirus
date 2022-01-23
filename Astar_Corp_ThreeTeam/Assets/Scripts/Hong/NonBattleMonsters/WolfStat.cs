using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �����ؾ� �ϴ� ���� (������ ���� ����)
// �̵� �ӵ� v
// ��� Ȯ��
// �����Ÿ�
// ���� ũ�� (������)
// ���� �� �ִ� �� ��
// 1���� �� ��Ÿ��
// �浹 �� ���� �ּ� ���� �� ~ �ִ� ���� ��

public class WolfStat : MonoBehaviour
{
    private ScriptableMgr scriptableMgr;
    private WorldMonster wolfStat;
    private string id;

    private NavMeshAgent agent;

    private void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        id = "NBM_0001";
        wolfStat = scriptableMgr.worldMonsterList[id];
        //StartCoroutine(GetNavMesh());

        //NavMeshHit hitInfo;
        //if (NavMesh.SamplePosition(transform.position, out hitInfo, 3f, NavMesh.AllAreas))
        //{
        //    Debug.Log(hitInfo.mask);
        //    
        //}
    }

    private IEnumerator GetNavMesh()
    {
        yield return new WaitForSeconds(1.0f);

        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent.gameObject.name);
        agent.speed = wolfStat.speed;
    }

    public void OnTestButtonClick()
    {
        Debug.Log(wolfStat);
    }
}
