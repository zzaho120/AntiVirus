using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFootprint : MonoBehaviour
{
    private float totalTime = 0;

    private FootprintPool footprintPool;
    //public Transform footprints;

    private void Start()
    {
        footprintPool = GameObject.Find("FootprintPool").GetComponent<FootprintPool>();
    }

    private void Update()
    {
        totalTime += Time.deltaTime;

        // 지금 몬스터 DB 수정됨
        // Pool 0. Bear
        // Pool 1. Boar
        // Pool 2. Fox
        // Pool 3. Rabbit

        var randFootprint = Random.Range(0, 4);
        GetFootprintsFromPool(randFootprint);
        // 수정
        //if (gameObject.name == MonsterName.monster1)
        //{
        //    GetFootprintsFromPool(0);
        //}
        //else if (gameObject.name == MonsterName.monster2)
        //{
        //    GetFootprintsFromPool(1);
        //}
        //else if (gameObject.name == MonsterName.monster3)
        //{
        //    GetFootprintsFromPool(2);
        //}
        //else if (gameObject.name == MonsterName.monster4)
        //{
        //    GetFootprintsFromPool(3);
        //}
        //else
        //{
        //    Debug.LogError("뭐지");
        //}
    }

    private void GetFootprintsFromPool(int num)
    {
        footprintPool.pools[num].prefab.transform.LookAt(transform);

        if (totalTime > 0.8)
        {
            GameObject go = footprintPool.pools[num].Pool.Get();
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;

            // Footprint position tester
            //go.transform.position = GameObject.Find("Player").transform.position + new Vector3(Random.Range(5f, 20f), 10f, Random.Range(5f, 20f));

            totalTime = 0;
        }
    }
}
