using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleTurn
{
    None,
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
    public BattlePoolMgr battlePoolMgr;
   // public MultiTouch touchMgr;
    public PlayerDataMgr playerDataMgr;     // 여기 저장하기 위해

    [Header("Turn")]
    public BattleTurn startTurn; // For Test
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
        //touchMgr = GameObject.FindWithTag("TouchMgr").GetComponent<MultiTouch>();
        battlePoolMgr = GameObject.FindWithTag("BattlePoolMgr").GetComponent<BattlePoolMgr>();
    }

    public void Start()
    {
        Time.timeScale = 1f;
        var battleTest = GetComponent<BattleTest>();
        battleTest.Init();
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        sightMgr.Init();
        pathMgr.Init();
        hintMgr.Init();

        turn = startTurn;
        var window = battleWindowMgr.Open(0) as BattleBasicWindow;
        window.Init();

        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
        EventBusMgr.Subscribe(EventType.DestroyChar, DestroyChar);


        switch (turn)
        {
            case BattleTurn.Player:
                window.SetSelectedChar(playerMgr.playerableChars[0]);
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
        Invoke("ChangeTurn", 0.1f);
    }

    private void ChangeTurn()
    {
        switch (turn)
        {
            case BattleTurn.Player:
                turn = BattleTurn.Enemy;
                EventBusMgr.Publish(EventType.StartEnemy);
                break;
            case BattleTurn.Enemy:
                turn = BattleTurn.Player;
                var window = battleWindowMgr.Open(0) as BattleBasicWindow;
                window.SetSelectedChar(playerMgr.playerableChars[0]);
                EventBusMgr.Publish(EventType.StartPlayer);
                break;
        }
        if (startTurn == turn)
            turnCount++;
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
        var playerableChars = playerMgr.playerableChars;
        if (playerableChars.Count == 0 || monsterMgr.monsters.Count == 0)
        {
            turn = BattleTurn.None;

            var exp = ScriptableMgr.Instance.GetMissionExp($"MEP_{fieldVirusLevel}").exp;
            foreach (var player in playerableChars)
            {
                player.characterStats.GetExp(exp);
            }

            battleWindowMgr.Open(1);
        }
    }
}