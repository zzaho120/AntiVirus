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
        // ���� Awake���� ����
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

    // Ǯ ����
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
        //    // Ǯ�� ����� ���ӿ�����Ʈ ����
        //    newPool = new GameObject($"{pools[i].poolName}_{pools[i].prefab.name}");
        //    newPool.transform.SetParent(this.transform);
        //    //newPool.transform.localScale = 
        //    pools[i].poolTr = newPool.transform;
        //
        //    // Ǯ�� ����� ���ӿ�����Ʈ ����
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


            //// Ǯ �ʱ� ���� �� Monster �� ����
            //if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            //// Ǯ Max size ���� �� Monster �� ����
            //else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;
            //
            // ���� ����
            //monsterNum[i] = poolBox[i].transform.childCount;
            //
            //if (monsterNum[i] < pools[i].quantity)
            //{
            //    // MonsterArea(���� ����)�� ������ŭ�� Ǯ���� ���� ������ Get �ؿ���
            //    // i : PoolCount (Ǯ ����)
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
        //    // Ǯ �ʱ� ���� �� Monster �� ����
        //    if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
        //    // Ǯ Max size ���� �� Monster �� ����
        //    else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;

        //    // ���� ����
        //    if (monsterNum[i] < pools[i].quantity)
        //    {
        //        // MonsterArea(���� ����)�� ������ŭ�� Ǯ���� ���� ������ Get �ؿ���
        //        // i : PoolCount (Ǯ ����)
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

        // Ǯ���� ������
        var ps = pools[poolNum].Pool.Get();

        // ��ġ ����
        //ps.transform.position = new Vector3(testZone[poolNum + 1].position.x, 0f, testZone[poolNum + 1].position.z);
        ps.transform.position = new Vector3(transform.parent.position.x, 0f, transform.parent.position.z);

        // ���� ���� �������ִ� Ŭ���� �ʱ�ȭ
        ps.AddComponent<SetMonsterStat>().Init();

        // �÷��̾� �þ� �ȿ����� ���̵��� �ϴ� ��ũ��Ʈ �߰�
        var playerSight = ps.AddComponent<InPlayerSight>();
        playerSight.printType = InPlayerSight.PrintType.Mesh;
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    //}
}
