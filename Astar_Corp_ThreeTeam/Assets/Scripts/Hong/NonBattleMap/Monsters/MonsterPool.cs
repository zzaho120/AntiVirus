using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MonsterPool : PoolManager
{
    private Transform[] testZone;
    private int[] monsterNum;
    private bool[] isMaxPool;
    //private List<GameObject> monsterList = new List<GameObject>();

    public void Init()
    {
        testZone = GameObject.Find("MonsterArea").GetComponentsInChildren<Transform>();
        isMaxPool = new bool[testZone.Length];
        monsterNum = new int[testZone.Length];

        InvokeRepeating("GetMonstersFromPool", 0.5f, 3f);
    }

    private void GetMonstersFromPool()
    {
        //for (int i = 0; i < testZone.Length; i++)
        for (int i = 0; i < pools.Count; i++)
        {
            // 풀 초기 생성 시 Monster 수 정의
            if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            // 풀 Max size 도달 시 Monster 수 정의
            else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;

            // 몬스터 생성
            if (monsterNum[i] < pools[i].quantity)
            {
                // MonsterArea(몬스터 영역)의 갯수만큼만 풀에서 몬스터 프리팹 Get 해오기
                // i : PoolCount (풀 갯수)
                if (i >= testZone.Length - 1)
                {
                    //Debug.Log("Return");
                    return;
                }
                else
                {
                    CreateMonster(i);
                }
            }
            else
            {
                isMaxPool[i] = true;
                
                //Debug.Log(i + " : Max Pool size");
                //CancelInvoke("GetMonstersFromPool");
            }
        }
    }
    
    private void CreateMonster(int poolNum)
    {
        isMaxPool[poolNum] = false;

        var ps = pools[poolNum].Pool.Get();
        ps.transform.position = new Vector3(testZone[poolNum + 1].position.x, 0f, testZone[poolNum + 1].position.z);
        //ps.GetComponent<SetMonsterStat>().Init();
        ps.AddComponent<SetMonsterStat>().Init();
        var playerSight = ps.AddComponent<InPlayerSight>();
        playerSight.printType = InPlayerSight.PrintType.Mesh;
        //ps.transform.localScale = new Vector3(10f, 10f, 10f);

        //monsterList.Add(ps);
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    //}
}
