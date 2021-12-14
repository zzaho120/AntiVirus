using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public ObjectPool3[] pools;
    private GameObject newPool;

    //public List<Transform> trList;      // 트랜스폼 저장용

    private void Start()
    {
        CreatePoolsTr();
    }

    // private List<GameObject> goList = new List<GameObject>();

    // 풀 생성
    private void CreatePoolsTr()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            //GameObject poolTr = new GameObject(pools[i].poolName.ToString());
            //poolTr.transform.SetParent(this.transform);
            //pools[i].poolTr = poolTr.transform;

            newPool = new GameObject(pools[i].poolName.ToString());
            newPool.transform.SetParent(this.transform);
            pools[i].poolTr = newPool.transform;


            // goList = poolTr.GetComponentsInChildren<GameObject>();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Pool size: " + pools[0].Pool.CountInactive);
        if (GUILayout.Button("Create Particles"))
        {
            var amount = UnityEngine.Random.Range(1, 5);
            for (int i = 0; i < amount; ++i)
            {
                var ps = pools[0].Pool.Get();
                ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            }
        }

        // 두번째 풀 실험용
        if (GUILayout.Button("Create Objects"))
        {
            // var amount = UnityEngine.Random.Range(1, 5);
            for (int i = 0; i < pools[i].quantity; ++i)
            {
                var ps = pools[1].Pool.Get();
                ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            }
        }
    }


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
