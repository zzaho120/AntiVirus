using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // 벙커 위치 담기
    public Transform bunkerPos;

    [Header("Public Class")]
    public CreateLabArea laboratoryArea;
    public CreateMonsterAreas monsterArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;
    public PlayerMove playerMove;

    private void Awake()
    {
        Instance = this;

        // 클래스 찾기
        var player = GameObject.Find("Player");
        laboratoryArea = Instance.GetComponent<CreateLabArea>();
        monsterArea = Instance.GetComponent<CreateMonsterAreas>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        playerController = player.GetComponent<PlayerController>();
        playerMove = player.GetComponent<PlayerMove>();
    }

    private void Start()
    {
        // 클래스들 초기화
        laboratoryArea.Init();
        monsterArea.Init();
        monsterPool.Init();
        playerMove.Init();
        playerController.Init();
    }

    private void Update()
    {
        playerController.PlayerControllerUpdate();
        //playerMove.PlayerMoveUpdate();
    }

    private void LateUpdate()
    {
        playerMove.PlayerMoveLateUpdate();
    }

    // PlayerPrefs 삭제 테스터
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}