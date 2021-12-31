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
    //    //    Debug.Log("���� ���� �ߴ�");
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
        Debug.Log("�� ����");

        monsterNum = newPool.transform.childCount;

        for (int i = 0; i < pools.Length; i++)
        {
            int amount = Random.Range(1, pools[i].quantity);    // 5
    
            // ���� ���� ����
            var randX = Random.Range(-5f, 5f);
            var randY = Random.Range(-5f, 5f);
    
            // ���� ��ġ ����
            var ps = pools[0].Pool.Get();
            ps.transform.position = testZone.position + new Vector3(randX, ps.transform.position.y, randY);

            //Debug.Log(pools[i].Pool.CountInactive);   // ���� ��Ȱ��ȭ ������Ʈ ����
            //Debug.Log(pools[i].maxPoolSize);          // 10

            // ���� ���� ����, �ߴ� ����
            //if (pools[0].Pool.CountInactive >= pools[i].quantity)
            if (monsterNum >= pools[i].quantity)
            {
                CancelInvoke("GetMonstersFromPool");
                Debug.Log("���� ���� �ߴ�");
            }

            //if (pools[0].Pool.CountInactive <= pools[0].quantity)
            //{
            //    //InvokeRepeating("GetMonstersFromPool", 1f, 3f);
            //    GetMonstersFromPool();
            //}
        }
    }
}
