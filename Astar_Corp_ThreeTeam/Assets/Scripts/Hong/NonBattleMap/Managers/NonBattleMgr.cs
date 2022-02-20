using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // Bunker Position
    public Transform playerStartPos;
    // Monster Area
    public CreateMonsterAreas[] createMonsterArea;

    // Player Object
    private GameObject player;

    [Header("Public Class")]
    public CreateLabArea createLabArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;
    public GetVirusPenalty getVirusPenalty;
    public WorldMonsterMgr worldMonsterMgr;
    public WorldAudioMgr worldAudioMgr;
    public SetTruck setTruck;

    [Header("UI")]
    public WorldUIMgr worldUIMgr;

    private void Awake()
    {
        Instance = this;

        // 클래스 찾기
        player = GameObject.Find("Player");
        createLabArea = Instance.GetComponent<CreateLabArea>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        setTruck = GameObject.Find("Trucks").GetComponent<SetTruck>();
        worldMonsterMgr = GetComponent<WorldMonsterMgr>();
        playerController = player.GetComponent<PlayerController>();
        worldUIMgr = GetComponent<WorldUIMgr>();
        worldAudioMgr = GameObject.Find("AudioMgr").GetComponent<WorldAudioMgr>();
    }

    private void Start()
    {
        // 클래스들 초기화
        createLabArea.Init();

        // 랜덤연구소도 초기화 -> 무한루프 됨
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
        getVirusPenalty = player.GetComponentInChildren<GetVirusPenalty>();
        playerController.Init();
        worldUIMgr.Init();
        worldAudioMgr.Init();
    }

    private void Update()
    {
        getVirusPenalty.VirusUpdate();
        playerController.PlayerControllerUpdate();
        worldMonsterMgr.MonsterUpdate();
        worldAudioMgr.AudioUpdate();
    }
}