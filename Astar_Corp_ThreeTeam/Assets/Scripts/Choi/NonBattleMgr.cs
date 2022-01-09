using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public static NonBattleMgr Instance;

    // 벙커 위치 담기
    public Transform bunkerPos;

    //[Header("Public Class")]
    public CreateLabArea laboratoryArea;
    public CreateMonsterAreas monsterArea;
    public MonsterPool monsterPool;

    private void Awake()
    {
        Instance = this;

        // 클래스 찾기
        laboratoryArea = Instance.GetComponent<CreateLabArea>();
        monsterArea = Instance.GetComponent<CreateMonsterAreas>();
        monsterPool = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
    }

    private void Start()
    {
        // 클래스들 초기화
        laboratoryArea.Init();
        monsterArea.Init();
        monsterPool.Init();
    }

    // PlayerPrefs 삭제 테스터
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}