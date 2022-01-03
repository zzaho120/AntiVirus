using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : PoolManager
{
    private Transform[] testZone;
    private int monsterNum;

    private bool[] isMaxPool;

    private void Start()
    {
        testZone = GameObject.Find("MonsterArea").GetComponentsInChildren<Transform>();
        //Debug.Log(testZone.position);

        isMaxPool = new bool[testZone.Length];
        for (int i = 0; i < isMaxPool.Length; i++)
        {
            isMaxPool[i] = false;
        }

        InvokeRepeating("GetMonstersFromPool", 0f, 3f);
    }


    //private IEnumerator GetMonstersFromPool()
    private void GetMonstersFromPool()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            // 풀 초기 생성 시 Monster 수 정의
            if (!isMaxPool[i]) monsterNum = newPool.transform.childCount;
            // 풀 Max size 도달 시 Monster 수 정의
            else monsterNum = pools[i].quantity - pools[i].Pool.CountInactive;
            
            // 몬스터 생성
            if (monsterNum < pools[i].quantity)
            {
                CreateMonster(i);
            }
            else
            {
                isMaxPool[i] = true;

                //Debug.Log("Max Pool size");
                //CancelInvoke("GetMonstersFromPool");
            }
        }
    }

    private void CreateMonster(int poolNum)
    {
        //Debug.Log("몹 생성");
        isMaxPool[poolNum] = false;
        //int amount = Random.Range(1, pools[i].quantity);    // 5

        // 랜덤 범위 설정
        var randX = Random.Range(-3f, 3f);
        var randY = Random.Range(-3f, 3f);

        // 랜덤 위치 설정
        var ps = pools[poolNum].Pool.Get();
        ps.transform.position = testZone[poolNum + 1].position + new Vector3(randX, ps.transform.position.y, randY);
    }

    private void OnGUI()
    {
        GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    }
}
