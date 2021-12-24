using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayer player;
    public FogMgr fogMgr;
    public AStar aStar;
    public int turn;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        player = GameObject.FindWithTag("Player").GetComponent<BattlePlayer>();
        fogMgr = GameObject.FindWithTag("FogMgr").GetComponent<FogMgr>();
    }

    public void Start()
    {
        tileMgr.Init();
        player.Init();
        aStar.Init();
        fogMgr.Init();
    }

    public void OnTurnEnd()
    {
        if (turn < 5)
        {
            EventBusMgr.Publish(EventType.TurnEnd);
            turn++;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
