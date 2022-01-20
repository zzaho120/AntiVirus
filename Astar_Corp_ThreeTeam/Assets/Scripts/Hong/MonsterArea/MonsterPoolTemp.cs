using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolTemp : MonoBehaviour
{
    public GameObject[] monsterPrefabs;

    public ObjectPool randMonsterPool
    {
        get
        {
            return randMonsterPool;
        }
        set
        {
            randMonsterPool.quantity = Random.Range(1, 10);
            randMonsterPool.poolName = PoolName.Monster;
            var randIndex = Random.Range(0, monsterPrefabs.Length);
            randMonsterPool.prefab = monsterPrefabs[randIndex];
        }
    }
}
