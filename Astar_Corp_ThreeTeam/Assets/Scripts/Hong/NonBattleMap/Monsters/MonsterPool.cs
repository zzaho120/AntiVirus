using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MonsterPool : PoolManager
{
    private WorldMonsterMgr monsterMgr;

    private bool[]  isMaxPool;
    private int[]   monsterNum;
    private int     poolNum;

    public void Init()
    {
        monsterMgr = GameObject.Find("NonBattleMgr").GetComponent<WorldMonsterMgr>();

        // 기존 Awake에서 실행
        poolBox = new GameObject[pools.Count];
        poolNum = transform.childCount;
        CreatePoolsTr();

        isMaxPool = new bool[poolNum];
        monsterNum = new int[poolNum];
        
        InvokeRepeating("GetMonstersFromPool", 0.5f, 30f);
    }

    // 풀 생성
    public override void CreatePoolsTr()
    {
        for (int i = 0; i < poolNum; i++)
        {
            pools[i].poolTr = transform.GetChild(i);
            poolBox[i] = transform.GetChild(i).gameObject;
        
            if (i >= poolNum)
                return;
        }
    }

    private void GetMonstersFromPool()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (i >= poolNum)
                return;

            // 풀 초기 생성 시 Monster 수 정의
            if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            // 풀 Max size 도달 시 Monster 수 정의
            else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;
            
            if (monsterNum[i] < pools[i].quantity)
            {
                CreateMonster(i);
            }
            else
            {
                isMaxPool[i] = true;
            }
        }
    }
    
    private void CreateMonster(int poolNum)
    {
        isMaxPool[poolNum] = false;

        // 풀에서 꺼내기
        var ps = pools[poolNum].Pool.Get();

        // 위치 설정
        ps.transform.position = new Vector3(transform.GetChild(poolNum).position.x, 0, transform.GetChild(poolNum).position.z);

        // NavMesh 활성화
        if (ps.GetComponent<NavMeshAgent>() != null)
            ps.GetComponent<NavMeshAgent>().enabled = true;
        else
            ps.AddComponent<NavMeshAgent>();
        // NavMesh 높이 설정
        ps.GetComponent<NavMeshAgent>().height /= 2f;

        // 몬스터 스탯 설정해주는 클래스 초기화
        ps.AddComponent<SetMonsterStat>().Init();

        //monsterMgr.monsters.Add(ps.GetComponent<WorldMonsterChar>());
        monsterMgr.AddMonster(ps.GetComponent<WorldMonsterChar>());

        //if (ps.GetComponent<WorldMonsterChar>() != null)
        //    ps.GetComponent<WorldMonsterChar>().Init();

        // 플레이어 시야 안에서만 보이도록 하는 스크립트 추가
        var playerSight = ps.AddComponent<InPlayerSight>();
        playerSight.printType = InPlayerSight.PrintType.Mesh;
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    //}
}
