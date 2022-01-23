using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolTester
{
    public GameObject[] monsterPrefabs;
    private int quantity;

    // ���� ������ �ش� ������� �����ǰ�
    public MonsterPoolTester(int quantity)
    {
        this.quantity = quantity;
    }

    public ObjectPool randMonsterPool
    {
        get
        {
            return randMonsterPool;
        }
        set
        {
            randMonsterPool.quantity = this.quantity;
            randMonsterPool.poolName = PoolName.Monster;
            var randIndex = Random.Range(0, monsterPrefabs.Length);
            randMonsterPool.prefab = monsterPrefabs[randIndex];
        }
    }
}
