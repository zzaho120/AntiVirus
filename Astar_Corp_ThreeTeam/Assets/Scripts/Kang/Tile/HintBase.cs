using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HintType
{
    None = -1,
    Footprint,
    Bloodprint
}

public class HintBase : BattleTile
{
    [Header("Edit Time")]
    public GameObject hintObj;
    public HintType type;
    public int lifeTime;

    [Header("Run Time")]
    public MonsterChar monster;
    public DirectionType direction;

    public void Init(DirectionType direction)
    {
        base.Init();
        currentTile.AddHint(this);

        gameObject.name = $"Hint ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
        this.direction = direction;
        var turn = BattleMgr.Instance.turnCount;
        lifeTime += turn;
    }

    public bool CheckLifeTime(int turnCount)
    {
        if (turnCount > lifeTime)
        {
            return true;
        }
        return false;
    }
}
