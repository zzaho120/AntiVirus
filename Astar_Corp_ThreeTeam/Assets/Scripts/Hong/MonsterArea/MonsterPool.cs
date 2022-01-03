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
            // Ǯ �ʱ� ���� �� Monster �� ����
            if (!isMaxPool[i]) monsterNum = newPool.transform.childCount;
            // Ǯ Max size ���� �� Monster �� ����
            else monsterNum = pools[i].quantity - pools[i].Pool.CountInactive;
            
            // ���� ����
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
        //Debug.Log("�� ����");
        isMaxPool[poolNum] = false;
        //int amount = Random.Range(1, pools[i].quantity);    // 5

        // ���� ���� ����
        var randX = Random.Range(-3f, 3f);
        var randY = Random.Range(-3f, 3f);

        // ���� ��ġ ����
        var ps = pools[poolNum].Pool.Get();
        ps.transform.position = testZone[poolNum + 1].position + new Vector3(randX, ps.transform.position.y, randY);
    }

    private void OnGUI()
    {
        GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    }
}
