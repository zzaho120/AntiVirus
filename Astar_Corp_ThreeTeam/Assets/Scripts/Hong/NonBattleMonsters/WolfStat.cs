using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 설정해야 하는 정보 (비전투 몬스터 스탯)
// 이동 속도 v
// 기습 확률
// 감지거리
// 영역 크기 (반지름)
// 영역 내 최대 젠 수
// 1마리 젠 쿨타임
// 충돌 시 전투 최소 마리 수 ~ 최대 마리 수

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
