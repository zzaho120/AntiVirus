using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<ObjectPool> pools;
    //public ObjectPool[] pools;
    private GameObject newPool;
    protected GameObject[] poolBox;

    private void Awake()
    {
        poolBox = new GameObject[pools.Count];
        //poolBox = new GameObject[pools.Length];

        CreatePoolsTr();
    }

    // 풀 생성
    private void CreatePoolsTr()
    {
        //for (int i = 0; i < pools.Length; i++)
        for (int i = 0; i < pools.Count; i++)
        {
            // 풀로 사용할 게임오브젝트 생성
            newPool = new GameObject($"{pools[i].poolName}_{pools[i].prefab.name}");
            newPool.transform.SetParent(this.transform);
            pools[i].poolTr = newPool.transform;

            // 풀로 사용할 게임오브젝트 저장
            poolBox[i] = newPool;
        }
    }

    //protected abstract void GetObjFromPool();

    // 실험용
    //void OnGUI()
    //{
    //    // Get : 꺼내가기
    //    GUILayout.Label("Pool size: " + pools[0].Pool.CountInactive);
    //    if (GUILayout.Button("Create Particles"))
    //    {
    //        var amount = UnityEngine.Random.Range(1, 5);
    //        for (int i = 0; i < amount; ++i)
    //        {
    //            var ps = pools[0].Pool.Get();
    //            ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
    //        }
    //    }
    //
    //    // 두번째 풀 실험용
    //    if (GUILayout.Button("Create Objects"))
    //    {
    //        var amount = UnityEngine.Random.Range(1, 5);
    //        //for (int i = 0; i < pools[i].quantity; ++i)
    //        for (int i = 0; i < amount; ++i)
    //        {
    //            var ps = pools[1].Pool.Get();
    //            ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
    //        }
    //    }
    //}


    //// 실험용
    //void Update()
    //{
    //    // Pool에 Return 해줄 타이밍 체크
    //    // Release 해서 돌려주면 됨
    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        for (int j = 0; j < newPool.transform.childCount; j++)
    //        {
    //            goList.Add(newPool.transform.GetChild(j).gameObject);
    //        }

    //        foreach (var activeGos in goList)
    //        {
    //            pools[0].Pool.Release(activeGos);
    //            Debug.Log("Returned at " + DateTime.Now.ToString());

    //            if (pools[0].maxPoolSize > pools[0].Pool.CountInactive)
    //            {
    //                //pools[0].OnDestroyPoolObject();
    //            }
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.F2))
    //    {
    //        pools[1].Pool.Release(gameObject);
    //        Debug.Log("Returned at " + DateTime.Now.ToString());
    //    }
    //    if (Input.GetKeyDown(KeyCode.F3))
    //    {
    //        pools[2].Pool.Release(gameObject);
    //        Debug.Log("Returned at " + DateTime.Now.ToString());
    //    }
    //}
}
