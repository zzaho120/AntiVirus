using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleTurn
{
    Player,
    Enemy
}

public class BattleMgr : MonoBehaviour
{
    public static BattleMgr Instance;

    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayerMgr playerMgr;
    public BattleMonsterMgr monsterMgr;
    public WindowManager battleWindowMgr;
    public SightMgr sightMgr;
    public PathMgr pathMgr;
    public HintMgr hintMgr;
    public PlayerDataMgr playerDataMgr;     // 여기 저장하기 위해

    [Header("Turn")]
    public BattleTurn startTurn;
    public BattleTurn turn;
    public int turnCount;
    public int fieldVirusLevel;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    public void Awake()
    {
        Instance = this;
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        sightMgr = GameObject.FindWithTag("FogMgr").GetComponent<SightMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        battleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
        pathMgr = GameObject.FindWithTag("PathMgr").GetComponent<PathMgr>();
        hintMgr = GameObject.FindWithTag("HintMgr").GetComponent<HintMgr>();

        // 비전투씬에서 넘어온 플레이어데이터매니저가 있으면
        // 아래 코드가 동작함

        var vectorList = new List<Vector3>();
        vectorList.Add(new Vector3(15, 0.5f, 15));
        vectorList.Add(new Vector3(15, 0.5f, 14));
        vectorList.Add(new Vector3(14, 0.5f, 14));
        vectorList.Add(new Vector3(14, 0.5f, 15));

        var playerDataMgrObj = GameObject.FindWithTag("PlayerDataMgr");
        var isExistDataMgr = playerDataMgrObj != null;
        if (isExistDataMgr)
        {
            playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

            for (var idx = 0; idx < playerDataMgr.battleSquad.Count; ++idx)
            {
                var player = Instantiate(playerPrefab, vectorList[idx], Quaternion.Euler(new Vector3(0, 180, 0)));
                player.transform.SetParent(playerMgr.transform);
                var playerableChar = player.GetComponent<PlayerableChar>();
                playerableChar.characterStats = playerDataMgr.battleSquad[idx];
            }

            if (playerDataMgr.isMonsterAtk)
                startTurn = BattleTurn.Enemy;
            else
                startTurn = BattleTurn.Player;
        }
        else
        {
            for (var idx = 0; idx < vectorList.Count; ++idx)
            {
                var player = Instantiate(playerPrefab, vectorList[idx], Quaternion.identity);
                player.transform.SetParent(playerMgr.transform);
                var playerableChar = player.GetComponent<PlayerableChar>();
            }
        }
    }

    public void Start()
    {
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        sightMgr.Init();
        pathMgr.Init();
        hintMgr.Init();

        turn = startTurn;
        var window = battleWindowMgr.Open((int)BattleWindows.TurnNotice - 1).GetComponent<TurnNoticeWindow>();
        window.NoticeTurn(turn);


        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
        EventBusMgr.Subscribe(EventType.DestroyChar, DestroyChar);


        switch (turn)
        {
            case BattleTurn.Player:
                EventBusMgr.Publish(EventType.StartPlayer);
                break;
            case BattleTurn.Enemy:
                EventBusMgr.Publish(EventType.StartEnemy);
                break;
        }

        sightMgr.UpdateFog();
    }

    public void Update()
    {
        switch (turn)
        {
            case BattleTurn.Player:
                break;
            case BattleTurn.Enemy:
                monsterMgr.UpdateTurn();
                break;
        }
    }

    public void OnChangeTurn(object empty)
    {
        Invoke("ChangeTurn", 1f);
    }

    private void ChangeTurn()
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
        if (startTurn == turn)
            turnCount++;
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
            battleWindowMgr.Open((int)BattleWindows.ResultWindow - 1);
        }
    }
}