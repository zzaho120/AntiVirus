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
    public PlayerDataMgr playerDataMgr;     // 여기 저장하기위해

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
        
        // 비전투씬에서 넘어온 플레이어데이터매니저가 있으면
        // 아래 코드가 동작함
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

        // 프로토타입용
        if (playerDataMgr != null)
        {

        }

        turn = BattleTurn.Player;
        var window = battleWindowMgr.Open((int)BattleWindows.TurnNotice - 1).GetComponent<TurnNoticeWindow>();
        window.NoticeTurn(turn);

        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
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
}