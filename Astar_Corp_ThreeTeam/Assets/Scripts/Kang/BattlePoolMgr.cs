using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePoolMgr : PoolManager
{
    public override void CreatePoolsTr()
    {
        base.CreatePoolsTr();
    }

    public GameObject CreateMoveTile()
    {
        var moveTile = pools[(int)BattlePoolName.MoveTile].Pool.Get();
        return moveTile;
    }
}
