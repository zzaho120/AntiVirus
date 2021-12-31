using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : PoolManager
{
    private Transform testZone;
    private int monsterNum;

    private void Start()
    {
        testZone = GameObject.Find("RabbitZone").transform;
        
        InvokeRepeating("GetMonstersFromPool", 1f, 3f);
    }

    //private void Update()
    //{
    //    //if (monsterNum >= pools[0].quantity)
    //    //{
    //    //    CancelInvoke("GetMonstersFromPool");
    //    //    Debug.Log("몬스터 생성 중단");
    //    //}
    //
    //    //if (pools[0].Pool.CountInactive <= pools[0].quantity)
    //    //{
    //    //    //InvokeRepeating("GetMonstersFromPool", 1f, 3f);
    //    //    GetMonstersFromPool();
    //    //}
    //}

    public void GetMonstersFromPool()
    {
        Debug.Log("몹 생성");

        monsterNum = newPool.transform.childCount;

        for (int i = 0; i < pools.Length; i++)
        {
            int amount = Random.Range(1, pools[i].quantity);    // 5
    
            // 랜덤 범위 설정
            var randX = Random.Range(-5f, 5f);
            var randY = Random.Range(-5f, 5f);
    
            // 랜덤 위치 설정
            var ps = pools[0].Pool.Get();
            ps.transform.position = testZone.position + new Vector3(randX, ps.transform.position.y, randY);

            //Debug.Log(pools[i].Pool.CountInactive);   // 남은 비활성화 오브젝트 개수
            //Debug.Log(pools[i].maxPoolSize);          // 10

            // 몬스터 생성 실행, 중단 조건
            //if (pools[0].Pool.CountInactive >= pools[i].quantity)
            if (monsterNum >= pools[i].quantity)
            {
                CancelInvoke("GetMonstersFromPool");
                Debug.Log("몬스터 생성 중단");
            }

            //if (pools[0].Pool.CountInactive <= pools[0].quantity)
            //{
            //    //InvokeRepeating("GetMonstersFromPool", 1f, 3f);
            //    GetMonstersFromPool();
            //}
        }
    }
}
