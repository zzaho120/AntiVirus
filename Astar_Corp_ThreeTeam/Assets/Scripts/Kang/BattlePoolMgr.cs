using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePoolMgr : PoolManager
{
    public Transform moveTilePool;
    public Transform virusTilePool;
    public Transform particlePool;
    public Transform sightPool;
    public Transform scrollingTextPool;

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

    public GameObject CreateFootStep()
    {
        var particle = pools[(int)BattlePoolName.FootStep].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateBloodSplat()
    {
        var particle = pools[(int)BattlePoolName.Blood].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateGunOneShot()
    {
        var particle = pools[(int)BattlePoolName.GunOneShot].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateBulletEjection()
    {
        var particle = pools[(int)BattlePoolName.BulletEjection].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateDetect()
    {
        var particle = pools[(int)BattlePoolName.Detect].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateSightTile()
    {
        var particle = pools[(int)BattlePoolName.SightTile].Pool.Get();
        particle.transform.SetParent(sightPool);
        return particle;
    }
    public GameObject CreateMonsterSightTile()
    {
        var particle = pools[(int)BattlePoolName.MonsterSightTile].Pool.Get();
        particle.transform.SetParent(sightPool);
        return particle;
    }

    public GameObject CreateScrollingText()
    {
        var particle = pools[(int)BattlePoolName.ScrollingText].Pool.Get();
        particle.transform.SetParent(scrollingTextPool);
        return particle;
    }

    public GameObject CreateUseItemFx()
    {
        var particle = pools[(int)BattlePoolName.UseItemFx].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }

    public GameObject CreateBullet()
    {
        var particle = pools[(int)BattlePoolName.Bullet].Pool.Get();
        particle.transform.SetParent(particlePool);
        return particle;
    }
}
