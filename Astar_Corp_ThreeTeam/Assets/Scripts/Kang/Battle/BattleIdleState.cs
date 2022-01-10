using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIdleState : StateBase
{
    private int moveRange = 4;
    private Vector3 nextTile;
    private Stack<AStarTile> pathList;
    private MonsterChar monster;
    private float timer;
    private bool isSetPath;
    private bool inited;

    public BattleIdleState(MonsterChar monster, FSM fsm)
    {
        this.fsm = fsm;
        this.monster = monster;
    }

    public override void Enter()
    {
        inited = false;
        isSetPath = false;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (monster.target == null)
        {
            if (!inited)
            {
                inited = true;
                SetPath();
            }

            if (!isSetPath)
            {
                isSetPath = true; 
                if (pathList.Count == 0)
                    EventBusMgr.Publish(EventType.EndEnemy);
                else
                    nextTile = pathList.Pop().tileBase.tileIdx;
            }

            if (timer < .5f)
            {
                timer += Time.deltaTime;
                monster.MoveTile(nextTile);
                if (BattleMgr.Instance.turn == BattleTurn.Enemy)
                    BattleMgr.Instance.sightMgr.UpdateFog();
            }
            else
            {
                timer = 0;
                isSetPath = false;
            }

            
        }
        else
            fsm.ChangeState((int)BattleMonState.Move);
    }

    private void SetPath()
    {
        var randomX = Random.Range(-moveRange, moveRange);
        var randomZ = Random.Range(-moveRange, moveRange);
        var currentTile = monster.currentTile.tileIdx;
        var endTile = new Vector3(Mathf.Clamp(randomX, 0, randomZ), 0, currentTile.z);
        timer = 0;

        if (BattleMgr.Instance.sightMgr.totalSightDics.ContainsKey(new Vector2(endTile.x, endTile.z)))
        {
            var aStar = BattleMgr.Instance.aStar;
            aStar.InitAStar(currentTile, endTile);
            pathList = aStar.pathList;
        }
    }
}
