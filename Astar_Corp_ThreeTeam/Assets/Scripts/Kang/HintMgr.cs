using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintMgr : MonoBehaviour
{
    [Header("List")]
    public List<HintBase> hintList = new List<HintBase>();

    [Header("Prefabs")]
    public GameObject footprint;
    public GameObject bloodprint;
    public void Init()
    {
        var startTurn = BattleMgr.Instance.startTurn;

        switch (startTurn)
        {
            case BattleTurn.Player:
                EventBusMgr.Subscribe(EventType.StartPlayer, CheckLifeTime);
                break;
            case BattleTurn.Enemy:
                EventBusMgr.Subscribe(EventType.StartEnemy, CheckLifeTime);
                break;
        }
    }

    public void AddPrint(HintType hintType, DirectionType directionType, Vector3 tileIdx)
    {
        GameObject prefab = null;
        var rot = Quaternion.identity;
        switch (hintType)
        {
            case HintType.Footprint:
                prefab = footprint;
                rot = Quaternion.Euler(90f, 0f, 0f);
                break;
            case HintType.Bloodprint:
                prefab = bloodprint;
                break;
        }
        var newVector = tileIdx + new Vector3(0, 1f, 0);
        var printGo = Instantiate(prefab, newVector, rot);
        var hintBase = printGo.GetComponent<HintBase>();
        hintBase.Init(directionType);

        printGo.transform.SetParent(gameObject.transform);

        hintList.Add(hintBase);
    }

    public void CheckLifeTime(object empty)
    {
        var removeList = new List<HintBase>();
        var turnCount = BattleMgr.Instance.turnCount;
        for (var idx = 0; idx < hintList.Count; ++idx)
        {
            if (hintList[idx].CheckLifeTime(turnCount))
                removeList.Add(hintList[idx]);
        }

        foreach(var hint in removeList)
        {
            hintList.Remove(hint);
            hint.currentTile.RemoveHint(hint);
            Destroy(hint.gameObject);
        }
    }

    public void CheckRader(Vector3 monster)
    {
        var playerableChar = BattleMgr.Instance.playerMgr.playerableChars;

        var min = (-1, 10000);
        for (var idx = 0; idx < playerableChar.Count; ++idx)
        {
            var vector = playerableChar[idx].tileIdx - monster;
            var absSum = (int)(Mathf.Abs(vector.x) + Mathf.Abs(vector.z));

            if (playerableChar[idx].audibleDistance * 4 > absSum)
            {
                if (min.Item2 > absSum)
                {
                    min.Item1 = idx;
                    min.Item2 = absSum;
                }
            }
        }

        if (min.Item1 == -1)
            return;

        var level = 0;
        var audible = playerableChar[min.Item1].audibleDistance;
        if (audible * 2 > min.Item2)
            level = 3;
        else if (audible * 3 > min.Item2)
            level = 2;
        else if (audible * 4 > min.Item2)
            level = 1;

        //CameraController.Instance.SetFollowObject(playerableChar[min.Item1].transform);
        //var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.RaderWindow - 1) as RaderWindow;
        //window.StartRader(playerableChar[min.Item1].tileIdx, monster, level);
        
    }
}