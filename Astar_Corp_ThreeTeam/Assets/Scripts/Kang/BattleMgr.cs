using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleTurn
{
    Player,
    Enemy
}

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayerMgr playerMgr;
    public BattleMonsterMgr monsterMgr;
    public WindowManager battleWindowMgr;
    public SightMgr sightMgr;
    public AStar aStar;
    public PlayerDataMgr playerDataMgr;     // ���� �����ϱ�����

    [Header("Turn")]
    public BattleTurn turn;
    public int turnCount;
    public int fieldVirusLevel;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        sightMgr = GameObject.FindWithTag("FogMgr").GetComponent<SightMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        battleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
        
        // ������������ �Ѿ�� �÷��̾���͸Ŵ����� ������
        // �Ʒ� �ڵ尡 ������
        var playerDataMgrObj = GameObject.FindWithTag("PlayerDataMgr");
        var isExistDataMgr = playerDataMgrObj != null;
        if (isExistDataMgr)
        {
            playerDataMgr = GameObject.FindWithTag("PlayerDataMgr").GetComponent<PlayerDataMgr>();

            var vectorList = new List<Vector3>();
            vectorList.Add(new Vector3(11, 1, 11));
            vectorList.Add(new Vector3(10, 1, 11));
            vectorList.Add(new Vector3(11, 1, 10));
            vectorList.Add(new Vector3(10, 1, 10));

            for (var idx = 0; idx < playerDataMgr.battleSquad.Count; ++idx)
            {
                var player = Instantiate(playerPrefab, vectorList[idx], Quaternion.identity);
                player.transform.SetParent(playerMgr.transform);
                var playerableChar = player.GetComponent<PlayerableChar>();
                playerableChar.characterStats = playerDataMgr.battleSquad[idx];
            }
        }
    }

    public void Start()
    {
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        sightMgr.Init();
        aStar.Init();

        turn = BattleTurn.Player;
        var window = battleWindowMgr.Open((int)BattleWindows.TurnNotice - 1).GetComponent<TurnNoticeWindow>();
        window.NoticeTurn(turn);

        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
        EventBusMgr.Subscribe(EventType.DestroyChar, DestroyChar);

        EventBusMgr.Publish(EventType.StartPlayer);
    }

    public void Update()
    {
        switch (turn)
        {
            case BattleTurn.Player:
                break;
            case BattleTurn.Enemy:
                monsterMgr.TurnUpdate();
                break;
        }
    }

    public void OnChangeTurn(object empty)
    {
        var windowId = (int)BattleWindows.TurnNotice - 1;
        var window = battleWindowMgr.Open(windowId).GetComponent<TurnNoticeWindow>();
        switch (turn)
        {
            case BattleTurn.Player:
                turn = BattleTurn.Enemy;
                EventBusMgr.Publish(EventType.StartEnemy);
                break;
            case BattleTurn.Enemy:
                turn = BattleTurn.Player;
                EventBusMgr.Publish(EventType.StartPlayer);
                break;
        }
        window.NoticeTurn(turn);
    }

    public void DestroyChar(object[] param)
    {
        var tempType = (int)param[1];
        switch (tempType)
        {
            case 0:
                var player = (PlayerableChar)param[0];
                playerMgr.RemovePlayer(player);
                break;
            case 1:
                var monster = (MonsterChar)param[0];
                monsterMgr.RemoveMonster(monster);
                break;
        }

        CheckGameover();
    }

    private void CheckGameover()
    {
        if (playerMgr.playerableChars.Count == 0 || monsterMgr.monsters.Count == 0)
        {
            Time.timeScale = 0;
            // ���⿡ â �߰�
        }
    }
}