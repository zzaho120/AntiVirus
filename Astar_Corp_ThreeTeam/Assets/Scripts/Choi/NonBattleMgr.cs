using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // ��Ŀ ��ġ ���
    public Transform bunkerPos;

    [Header("Public Class")]
    public CreateLabArea laboratoryArea;
    public CreateMonsterAreas monsterArea;
    public MonsterPool monsterPool;
    public PlayerController playerController;

    private void Awake()
    {
        Instance = this;

        // Ŭ���� ã��
        laboratoryArea = Instance.GetComponent<CreateLabArea>();
        monsterArea = Instance.GetComponent<CreateMonsterAreas>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Ŭ������ �ʱ�ȭ
        laboratoryArea.Init();
        monsterArea.Init();
        monsterPool.Init();
        playerController.Init();
    }

    private void Update()
    {
        playerController.ActivePlayer();
    }

    // PlayerPrefs ���� �׽���
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}