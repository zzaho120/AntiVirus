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
        var newVector = tileIdx + new Vector3(0, 1, 0);
        GameObject prefab = null;
        switch (hintType)
        {
            case HintType.Footprint:
                prefab = footprint;
                break;
            case HintType.Bloodprint:
                prefab = bloodprint;
                break;
        }
        var printGo = Instantiate(prefab, newVector, Quaternion.identity);
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
}
