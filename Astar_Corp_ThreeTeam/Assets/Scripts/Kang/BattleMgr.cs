using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayer player;
    public FogMgr fogMgr;
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
        fogMgr.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            EventBusMgr.Publish(EventType.TurnEnd);
    }
}
