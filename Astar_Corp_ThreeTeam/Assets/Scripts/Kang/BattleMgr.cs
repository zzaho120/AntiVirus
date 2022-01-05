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
    public WindowManager BattleWindowMgr;
    public SightMgr sightMgr;
    public AStar aStar;

    [Header("Turn")]
    public BattleTurn turn;
    public int turnCount;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        sightMgr = GameObject.FindWithTag("FogMgr").GetComponent<SightMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        BattleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
    }

    public void Start()
    {
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        sightMgr.Init();
        aStar.Init();
        BattleWindowMgr.Open(0);

        turn = BattleTurn.Player;
        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
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
    }
}