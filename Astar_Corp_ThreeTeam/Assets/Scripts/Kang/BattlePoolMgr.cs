using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePoolMgr : PoolManager
{
    public Transform moveTilePool;
    public Transform virusTilePool;
    public Transform particlePool;

    public override void CreatePoolsTr()
    {
        base.CreatePoolsTr();
    }

    public GameObject CreateMoveTile()
    {
        var moveTile = pools[(int)BattlePoolName.MoveTile].Pool.Get();
        moveTile.transform.SetParent(moveTilePool);
        return moveTile;
    }

    public GameObject CreateVirusTile()
    {
        var virusTile = pools[(int)BattlePoolName.VirusTile].Pool.Get();
        virusTile.transform.SetParent(virusTilePool);
        return virusTile;
    }

    public GameObject CreateParticles()
    {
        var particle = pools[(int)BattlePoolName.Particle].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }
}
