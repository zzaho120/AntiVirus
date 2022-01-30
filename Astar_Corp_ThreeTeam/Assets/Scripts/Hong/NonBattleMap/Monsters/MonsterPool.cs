using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MonsterPool : PoolManager
{
    private WorldMonsterMgr monsterMgr;

    private bool[]  isMaxPool;
    private int[]   monsterNum;
    private int     poolNum;

    public void Init()
    {
        monsterMgr = GameObject.Find("NonBattleMgr").GetComponent<WorldMonsterMgr>();

        // ���� Awake���� ����
        poolBox = new GameObject[pools.Count];
        poolNum = transform.childCount;
        CreatePoolsTr();

        isMaxPool = new bool[poolNum];
        monsterNum = new int[poolNum];
        
        InvokeRepeating("GetMonstersFromPool", 0.5f, 30f);
    }

    // Ǯ ����
    public override void CreatePoolsTr()
    {
        for (int i = 0; i < poolNum; i++)
        {
            pools[i].poolTr = transform.GetChild(i);
            poolBox[i] = transform.GetChild(i).gameObject;
        
            if (i >= poolNum)
                return;
        }
    }

    private void GetMonstersFromPool()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (i >= poolNum)
                return;

            // Ǯ �ʱ� ���� �� Monster �� ����
            if (!isMaxPool[i]) monsterNum[i] = poolBox[i].transform.childCount;
            // Ǯ Max size ���� �� Monster �� ����
            else monsterNum[i] = pools[i].quantity - pools[i].Pool.CountInactive;
            
            if (monsterNum[i] < pools[i].quantity)
            {
                CreateMonster(i);
            }
            else
            {
                isMaxPool[i] = true;
            }
        }
    }
    
    private void CreateMonster(int poolNum)
    {
        isMaxPool[poolNum] = false;

        // Ǯ���� ������
        var ps = pools[poolNum].Pool.Get();

        // ��ġ ����
        ps.transform.position = new Vector3(transform.GetChild(poolNum).position.x, 0, transform.GetChild(poolNum).position.z);

        // NavMesh Ȱ��ȭ
        if (ps.GetComponent<NavMeshAgent>() != null)
            ps.GetComponent<NavMeshAgent>().enabled = true;
        else
            ps.AddComponent<NavMeshAgent>();
        // NavMesh ���� ����
        ps.GetComponent<NavMeshAgent>().height /= 2f;

        // ���� ���� �������ִ� Ŭ���� �ʱ�ȭ
        ps.AddComponent<SetMonsterStat>().Init();

        //monsterMgr.monsters.Add(ps.GetComponent<WorldMonsterChar>());
        monsterMgr.AddMonster(ps.GetComponent<WorldMonsterChar>());

        //if (ps.GetComponent<WorldMonsterChar>() != null)
        //    ps.GetComponent<WorldMonsterChar>().Init();

        // �÷��̾� �þ� �ȿ����� ���̵��� �ϴ� ��ũ��Ʈ �߰�
        var playerSight = ps.AddComponent<InPlayerSight>();
        playerSight.printType = InPlayerSight.PrintType.Mesh;
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("Count Inactive: " + pools[0].Pool.CountInactive);
    //}
}
