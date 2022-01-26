using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintPool : PoolManager
{
    private void Awake()
    {
        poolBox = new GameObject[pools.Count];
        CreatePoolsTr();
    }
}
