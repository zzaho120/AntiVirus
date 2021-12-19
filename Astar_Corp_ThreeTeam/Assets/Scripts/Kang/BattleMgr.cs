using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public PlayerTest player;
    public TileMgr tileMgr;
    public AStar aStar;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();
    }

    public void Start()
    {
        tileMgr.Init();
        player.Init();
        aStar.Init();
    }
}
