using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFootprint : MonoBehaviour
{
    private float totalTime = 0;

    private FootprintPool footprintPool;
    public Transform footprints;

    private void Start()
    {
        footprintPool = GameObject.Find("FootprintPool").GetComponent<FootprintPool>();
    }

    private void Update()
    {
        totalTime += Time.deltaTime;
        
        footprintPool.pools[0].prefab.transform.LookAt(transform);
        //footprintPool.pools[0].prefab.transform.rotation.x = 90f;

        if (totalTime > 0.8)
        {
            GameObject go = footprintPool.pools[0].Pool.Get();
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;

            totalTime = 0;
        }
    }
}
