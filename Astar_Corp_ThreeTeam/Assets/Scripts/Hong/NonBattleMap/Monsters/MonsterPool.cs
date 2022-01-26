using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MonsterPool : PoolManager
{
    private NonBattleMgr nonBattleMgr;
    public List<GameObject> monsterArea;

    private Transform[] testZone;
    private int[] monsterNum;
    private bool[] isMaxPool;
    private int poolNum;
    //private List<GameObject> monsterList = new List<GameObject>();

    public void Init()
    {
        // 기존 Awake에서 실행
        poolBox = new GameObject[pools.Count];
        poolNum = transform.childCount;
        CreatePoolsTr();

        //testZone = transform.childCount;
        isMaxPool = new bool[poolNum];
        isMaxPool = new bool[poolNum];
        
        //testZone = GameObject.Find("MonsterArea").GetComponentsInChildren<Transform>();
        //monsterNum = new int[testZone.Length];
        //monsterNum = new int[testZone.Length];

        InvokeRepeating("GetMonstersFromPool", 0.5f, 5f);
    }

    // 풀 생성
    public override void CreatePoolsTr()
    {
        Debug.Log(poolNum);

        for (int i = 0; i < poolNum; i++)
        {
            pools[i].poolTr = transform.GetChild(i);
            poolBox[i] = transform.GetChild(i).gameObject;

            if (i >= poolNum)
                return;
        }

        //for (int i = 0; i < pools.Count; i++)
        //{
        //    // 풀로 사용할 게임오브젝트 생성
        //    newPool = new GameObject($"{pools[i].poolName}_{pools[i].prefab.name}");
        //    newPool.transform.SetParent(this.transform);
        //    //newPool.transform.localScale = 
        //    pools[i].poolTr = newPool.transform;
        //
        //    // 풀로 사용할 게임오브젝트 저장
        //    poolBox[i] = newPool;
        //}
    }

    private void GetMonstersFromPool()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (i >= poolNum)
            {
                //Debug.Log("Return");
                return;
            }
            else
            {
                CreateMonster(i);
            }


            //// 풀 초기 생성 시 Monster 수 정의
            //if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            //// 풀 Max size 도달 시 Monster 수 정의
            //else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;
            //
            // 몬스터 생성
            //monsterNum[i] = poolBox[i].transform.childCount;
            //
            //if (monsterNum[i] < pools[i].quantity)
            //{
            //    // MonsterArea(몬스터 영역)의 갯수만큼만 풀에서 몬스터 프리팹 Get 해오기
            //    // i : PoolCount (풀 갯수)
            //    if (i >= poolNum)
            //    {
            //        //Debug.Log("Return");
            //        return;
            //    }
            //    else
            //    {
            //        CreateMonster(i);
            //    }
            //}
            //else
            //{
            //    isMaxPool[i] = true;
            //
            //    //Debug.Log(i + " : Max Pool size");
            //    //CancelInvoke("GetMonstersFromPool");
            //}
        }

        ////for (int i = 0; i < testZone.Length; i++)
        //for (int i = 0; i < pools.Count; i++)
        //{
        //    // 풀 초기 생성 시 Monster 수 정의
        //    if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
        //    // 풀 Max size 도달 시 Monster 수 정의
        //    else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;

        //    // 몬스터 생성
        //    if (monsterNum[i] < pools[i].quantity)
        //    {
        //        // MonsterArea(몬스터 영역)의 갯수만큼만 풀에서 몬스터 프리팹 Get 해오기
        //        // i : PoolCount (풀 갯수)
        //        if (i >= poolNum)
        //        {
        //            //Debug.Log("Return");
        //            return;
        //        }
        //        else
        //        {
        //            CreateMonster(i);
        //        }
        //    }
        //    else
        //    {
        //        isMaxPool[i] = true;
                
        //        //Debug.Log(i + " : Max Pool size");
        //        //CancelInvoke("GetMonstersFromPool");
        //    }
        //}
    }
    
    //private IEnumerator CreateMonster(int poolNum)
    private void CreateMonster(int poolNum)
    {
        isMaxPool[poolNum] = false;

        // 풀에서 꺼내기
        var ps = pools[poolNum].Pool.Get();

        // 위치 설정
        //ps.transform.position = new Vector3(testZone[poolNum + 1].position.x, 0f, testZone[poolNum + 1].position.z);
        ps.transform.position = new Vector3(transform.parent.position.x, 0f, transform.parent.position.z);

        // 몬스터 스탯 설정해주는 클래스 초기화
        ps.AddComponent<SetMonsterStat>().Init();

        // 플레이어 시야 안에서만 보이도록 하는 스크립트 추가
        var playerSight = ps.AddComponent<InPlayerSight>();
        playerSight.printType = InPlayerSight.PrintType.Mesh;
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    //}
}
