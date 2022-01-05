using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveState : StateBase
{
    private MonsterChar monster;
    private PlayerableChar target;
    private Stack<AStarTile> pathList;
    private float timer;
    private Vector3 nextTile;
    private bool isSetPath;
    private bool inited;

    public BattleMoveState(MonsterChar monster, FSM fsm)
    {
        this.monster = monster;
        this.fsm = fsm;
    }

    public override void Enter()
    {
        inited = false;
        isSetPath = false;
        target = monster.target;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (!inited)
        {
            inited = true;
            SetPath();
        }

        if (!isSetPath)
        {
            isSetPath = true;

            if (pathList.Count > 0)
                nextTile = pathList.Pop().tileBase.tileIdx;
            if (target.tileIdx == nextTile)
                fsm.ChangeState((int)BattleMonState.Attack);
        }

        if (target.tileIdx != nextTile)
        {
            if (timer < 0.1f)
            {
                timer += Time.deltaTime;

                monster.MoveTile(nextTile);
                BattleMgr.Instance.sightMgr.UpdateFog();
            }
            else
            {
                timer = 0;
                isSetPath = false;
            }
        }
    }

    private void SetPath()
    {
        var aStar = BattleMgr.Instance.aStar;

        Debug.Log(monster.tileIdx);
        Debug.Log(target);
        aStar.InitAStar(monster.tileIdx, target.currentTile.tileIdx);
        pathList = aStar.pathList;
    }
}