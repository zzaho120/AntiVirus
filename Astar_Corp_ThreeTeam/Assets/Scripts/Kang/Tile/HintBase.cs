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
    public string ownerName;

    public void Init(DirectionType direction, MonsterChar monster)
    {
        InitHint();
        currentTile.AddHint(this);

        gameObject.name = $"Hint ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
        this.direction = direction;
        this.monster = monster;
        ownerName = monster.ownerName;

        var turn = BattleMgr.Instance.turnCount;
        lifeTime += (turn + 1);
    }

    public bool CheckLifeTime(int turnCount)
    {
        return turnCount > lifeTime;
    }
}
