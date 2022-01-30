using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // ��Ŀ ��ġ ���
    public Transform bunkerPos;
    // Monster Area
    public CreateMonsterAreas[] createMonsterArea;
    //public List<GameObject> monsterArea;

    [Header("Public Class")]
    public CreateLabArea laboratoryArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;
    public PlayerMove playerMove;
    public WorldMonsterMgr worldMonsterMgr;

    private void Awake()
    {
        Instance = this;

        // Ŭ���� ã��
        var player = GameObject.Find("Player");
        laboratoryArea = Instance.GetComponent<CreateLabArea>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        worldMonsterMgr = GetComponent<WorldMonsterMgr>();
        playerController = player.GetComponent<PlayerController>();
        playerMove = player.GetComponent<PlayerMove>();

    }

    private void Start()
    {
        // PlayerPrefs.DeleteAll();

        // Ŭ������ �ʱ�ȭ
        laboratoryArea.Init();
        for (int i = 0; i < createMonsterArea.Length; i++)
        {
            createMonsterArea[i].Init();
        }
        monsterPool.Init();
        //worldMonsterMgr.Init();
        playerMove.Init();
        playerController.Init(); 
    }

    private void Update()
    {
        playerController.PlayerControllerUpdate();
        //playerMove.PlayerMoveUpdate();
        worldMonsterMgr.MonsterUpdate();
    }

    private void LateUpdate()
    {
        playerMove.PlayerMoveLateUpdate();
    }

    // PlayerPrefs ���� �׽���
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}