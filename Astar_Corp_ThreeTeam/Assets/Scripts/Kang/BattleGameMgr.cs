using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameMgr : Singleton<BattleGameMgr>
{
    public CommandMgr commandMgr;

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
