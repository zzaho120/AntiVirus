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
    public CreateLabArea createLabArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;
    public GetVirusPenalty getVirusPenalty;
    public WorldMonsterMgr worldMonsterMgr;

    private void Awake()
    {
        Instance = this;

        // Ŭ���� ã��
        var player = GameObject.Find("Player");
        createLabArea = Instance.GetComponent<CreateLabArea>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        worldMonsterMgr = GetComponent<WorldMonsterMgr>();
        getVirusPenalty = player.GetComponentInChildren<GetVirusPenalty>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Ŭ������ �ʱ�ȭ
        createLabArea.Init();

        // ���������ҵ� �ʱ�ȭ -> ���ѷ��� ��
        //foreach (var element in createLabArea.laboratoryObjs)
        //{
            //element.GetComponentInParent<CreateMonsterAreas>().Init();
        //
        //}
        for (int i = 0; i < createMonsterArea.Length; i++)
        {
            createMonsterArea[i].GetComponent<CreateMonsterAreas>().Init();
        }
        monsterPool.Init();
        playerController.Init(); 
    }

    private void Update()
    {
        //foreach (var element in createLabArea.laboratoryObjs)
        //{
        //    element.GetComponent<LaboratoryInfo>().LaboratoryUpdate();
        //}
        getVirusPenalty.VirusUpdate();
        playerController.PlayerControllerUpdate();
        worldMonsterMgr.MonsterUpdate();
    }

    // PlayerPrefs ���� �׽���
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}