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

    [Header("Turn")]
    public BattleTurn turn;
    public int turnCount;
    public int fieldVirusLevel;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        sightMgr = GameObject.FindWithTag("FogMgr").GetComponent<SightMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        battleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
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