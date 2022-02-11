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
    public GameObject arrowPrefab;

    public float maxDelayTime = 3f;
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
                switch (directionType)
                {
                    case DirectionType.None:
                        break;
                    case DirectionType.Top:
                        rot = Quaternion.Euler(90f, 0f, 0f);
                        break;
                    case DirectionType.Bot:
                        rot = Quaternion.Euler(90f, 180f, 0f);
                        break;
                    case DirectionType.Left:
                        rot = Quaternion.Euler(90f, 270f, 0f);
                        break;
                    case DirectionType.Right:
                        rot = Quaternion.Euler(90f, 90f, 0f);
                        break;
                }
                break;
            case HintType.Bloodprint:
                prefab = bloodprint;
                break;
        }
        var newVector = tileIdx + new Vector3(0, .55f, 0);
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

            var minAudibleDis = playerableChar[idx].SightDistance;
            var maxAudibleDis = playerableChar[idx].audibleDistance * 3;
            if (minAudibleDis + maxAudibleDis > absSum)
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
        var player = playerableChar[min.Item1];
        var minAudible = player.SightDistance;
        var audible = player.audibleDistance;
        if (minAudible + audible > min.Item2)
            level = 3;
        else if (minAudible + audible * 2 > min.Item2)
            level = 2;
        else if (minAudible + audible * 3 > min.Item2)
            level = 1;

        CameraController.Instance.SetCameraTrs(player.transform);
        var dir = (monster - player.tileIdx);
        var newPos = new Vector3(dir.x, 0f, dir.z);

        var arrow = Instantiate(arrowPrefab, player.transform.position, Quaternion.identity);
        var rot = Quaternion.FromToRotation(arrow.transform.forward, newPos);

        rot.x = 0;
        rot.z = 0;

        arrow.transform.rotation = rot;
        arrow.transform.position += newPos.normalized;


        if (level < 3)
            arrow.transform.GetChild(1).gameObject.SetActive(false);
        if (level < 2)
            arrow.transform.GetChild(0).gameObject.SetActive(false);

        Destroy(arrow, maxDelayTime);
    }
}