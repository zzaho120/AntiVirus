using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
