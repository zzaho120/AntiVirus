using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // Bunker Position
    public Transform bunkerPos;
    // Monster Area
    public CreateMonsterAreas[] createMonsterArea;

    [Header("Public Class")]
    public CreateLabArea laboratoryArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;
    public WorldMonsterMgr worldMonsterMgr;

    private void Awake()
    {
        Instance = this;

        // 클래스 찾기
        var player = GameObject.Find("Player");
        laboratoryArea = Instance.GetComponent<CreateLabArea>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        worldMonsterMgr = GetComponent<WorldMonsterMgr>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        // 클래스들 초기화
        laboratoryArea.Init();
        for (int i = 0; i < createMonsterArea.Length; i++)
        {
            createMonsterArea[i].Init();
        }
        monsterPool.Init();
        playerController.Init(); 
    }

    private void Update()
    {
        playerController.PlayerControllerUpdate();
        worldMonsterMgr.MonsterUpdate();
    }

    // PlayerPrefs 삭제 테스터
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}