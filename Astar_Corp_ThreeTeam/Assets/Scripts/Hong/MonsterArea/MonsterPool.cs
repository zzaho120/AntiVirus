using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : PoolManager
{
    private Transform[] testZone;
    private int[] monsterNum;
    private bool[] isMaxPool;
    //private bool[] isNextCreate;

    private void Start()
    {
        //StartCoroutine(Init());
        testZone = GameObject.Find("MonsterArea").GetComponentsInChildren<Transform>();
        
        isMaxPool = new bool[testZone.Length];
        monsterNum = new int[testZone.Length];
        //isNextCreate = new bool[testZone.Length];
        
        InvokeRepeating("GetMonstersFromPool", 0.5f, 3f);
    }

    //private IEnumerator GetMonstersFromPool()
    private void GetMonstersFromPool()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            // Ǯ �ʱ� ���� �� Monster �� ����
            if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            // Ǯ Max size ���� �� Monster �� ����
            else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;

            // ���� ����
            if (monsterNum[i] < pools[i].quantity)
            {
                CreateMonster(i);
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
        //Debug.Log("�� ����");
        isMaxPool[poolNum] = false;

        // ���� ���� ����
        var randX = Random.Range(-3f, 3f);
        var randY = Random.Range(-3f, 3f);

        var ps = pools[poolNum].Pool.Get();
        ps.transform.position = testZone[poolNum].position + new Vector3(randX, ps.transform.position.y, randY);

        //int randNum = Random.Range(0, 9);
        //int randPool = Random.Range(0, pools.Length);
        //if (randNum < 5)
        //{
        //    var ps = pools[randPool].Pool.Get();
        //    ps.transform.position = testZone[poolNum + 1].position + new Vector3(randX, ps.transform.position.y, randY);
        //}
        //else
        //{
        //    var ps = pools[poolNum].Pool.Get();
        //    ps.transform.position = testZone[poolNum + 1].position + new Vector3(randX, ps.transform.position.y, randY);
        //}

        // ó�� ������ �� ��������
        //int amount;
        //if (!isNextCreate[poolNum]) amount = Random.Range(1, pools[poolNum].quantity);
        //else amount = 1;
        //for (int i = 0; i < amount; ++i)
        //{
        //    Debug.Log(poolNum + " : " + amount);
        //    var ps = pools[poolNum].Pool.Get();
        //    ps.transform.position = testZone[poolNum + 1].position + new Vector3(randX, ps.transform.position.y, randY);
        //    isNextCreate[poolNum] = true;
        //}
    }

    private void OnGUI()
    {
        GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    }
}
