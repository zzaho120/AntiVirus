using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayer player;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            player.StartTurn();
    }
}
