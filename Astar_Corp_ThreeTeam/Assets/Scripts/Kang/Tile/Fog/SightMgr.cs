using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SightTileBase : IComparable<SightTileBase>
{
    public TileBase tileBase;
    public bool isInSight;
    public SightTileBase(TileBase tileBase)
    {
        this.tileBase = tileBase;
    }

    public int CompareTo(SightTileBase other)
    {
        var thisTileIdx = tileBase.tileIdx;
        var otherTileIdx = other.tileBase.tileIdx;

        var thisValue = thisTileIdx.x * TileMgr.MAX_Z_IDX + thisTileIdx.z;
        var otherValue = otherTileIdx.x * TileMgr.MAX_Z_IDX + otherTileIdx.z;

        if (thisValue == otherValue)
            return 0;
        else if (thisValue > otherValue)
            return 1;
        else
            return -1;
    }

    public void Init()
    {
        isInSight = false;
        tileBase.EnableDisplay(false);
    }
}

public class SightMgr : MonoBehaviour
{
    // 합쳐놓은 타일
    // key의 x는 vector3의 x
    // key의 y는 vector3의 z
    public Dictionary<Vector2, List<SightTileBase>> totalSightDics =
        new Dictionary<Vector2, List<SightTileBase>>();

    public List<List<SightTileBase>> sightList =
        new List<List<SightTileBase>>();

    public List<List<SightTileBase>> frontSightList =
        new List<List<SightTileBase>>();

    public List<Dictionary<Vector2, List<SightTileBase>>> calculateSightDics =
        new List<Dictionary<Vector2, List<SightTileBase>>>();

    private List<PlayerableChar> playerableChars;

    private int checkTime = 0;
    private int playerCheck = 0;
    private int rayCheck = 0;
    public void Init()
    {
        playerableChars = BattleMgr.Instance.playerMgr.playerableChars;
        InitFog();
        UpdateFog();
    }

    private void InitFog()
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var wallDics = BattleMgr.Instance.tileMgr.wallDics;

        foreach (var pair in tileDics)
        {
            var checkIdx = new Vector2(pair.Key.x, pair.Key.z);

            if (!totalSightDics.ContainsKey(checkIdx))
                totalSightDics.Add(checkIdx, new List<SightTileBase>());
            totalSightDics[checkIdx].Add(new SightTileBase(pair.Value));
        }

        foreach (var pair in wallDics)
        {
            var checkIdx = new Vector2(pair.Key.x, pair.Key.z);

            if (!totalSightDics.ContainsKey(checkIdx))
                totalSightDics.Add(checkIdx, new List<SightTileBase>());
            totalSightDics[checkIdx].Add(new SightTileBase(pair.Value));
        }

        foreach (var pair in totalSightDics)
        {
            foreach (var tile in pair.Value)
            {
                tile.Init();
            }
        }

        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            frontSightList.Add(new List<SightTileBase>());
            sightList.Add(new List<SightTileBase>());
            calculateSightDics.Add(new Dictionary<Vector2, List<SightTileBase>>());
        }
    }

    public void UpdateFog()
    {
        UpdateCurrentFog();
        InitPlayerSight();
        InitObstacle();
        UpdateObj();

        Debug.Log($"checkTime : {checkTime}");
        Debug.Log($"playerCheck : {playerCheck}");
        Debug.Log($"rayCheck : {rayCheck}");
    }

    private void UpdateCurrentFog()
    {
        checkTime = 0;
        playerCheck = 0;
        rayCheck = 0;
        foreach (var list in sightList)
        {
            foreach (var tile in list)
            {
                tile.Init();
            }
            list.Clear();
        }

        foreach (var list in frontSightList)
        {
            foreach (var tile in list)
            {
                tile.Init();
            }
            list.Clear();
        }
    }

    private void InitPlayerSight()
    {
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            var player = playerableChars[idx];
            if (!player.gameObject.activeSelf)
                continue;

            var sightDistance = player.sightDistance;
            var curTileIdx = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z);

            if (!calculateSightDics[idx].ContainsKey(curTileIdx))
            {
                for (var i = -sightDistance; i <= sightDistance; ++i)
                {
                    if (curTileIdx.y + i < 0)
                        continue;

                    for (var j = -sightDistance; j <= sightDistance; ++j)
                    {
                        checkTime++;
                        playerCheck++;

                        if (curTileIdx.x + j < 0)
                            continue;

                        var checkIdx = new Vector2(curTileIdx.x + j, curTileIdx.y + i);
                        if (Mathf.Abs(i) + Mathf.Abs(j) > sightDistance)
                            continue;

                        if (totalSightDics.ContainsKey(checkIdx))
                        {
                            foreach (var sightTile in totalSightDics[checkIdx])
                            {
                                sightList[idx].Add(sightTile);
                            }
                        }
                    }
                }
                var sight = sightList[idx].ToList();
                calculateSightDics[idx].Add(curTileIdx, sight);
            }
            else
            {
                sightList[idx] = calculateSightDics[idx][curTileIdx];
            }
        }
    }

    private void InitObstacle()
    {
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            var player = playerableChars[idx];
            if (!player.gameObject.activeSelf)
                continue;

            var sightDist = player.sightDistance;
            var maxX = sightDist;
            var maxY = sightDist;
            var startTileIdx = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z);

            for (var i = -sightDist; i <= maxY; ++i)
            {
                var startY = startTileIdx.y + i;
                if (startY < 0)
                    i = i + (int)Mathf.Abs(startY);

                for (var j = -sightDist; j <= maxX; ++j)
                {
                    var startX = startTileIdx.x + j;
                    if (startX < 0)
                        j = j + (int)Mathf.Abs(startX);

                    var endTileIdx = new Vector2(startTileIdx.x + j, startTileIdx.y + i);
                    var absX = Mathf.Abs(j);
                    var absY = Mathf.Abs(i);
                    var specificCase = absX == absY &&
                        (absX + absY) == sightDist - 1;
                    if (absY + absX == sightDist || specificCase)
                        CastRayTile(startTileIdx, endTileIdx, sightDist, idx);
                }
            }
        }
    }

    private void CastRayTile(Vector2 start, Vector2 end, int sightDist, int playerIdx)
    {
        var delta = end - start;

        Vector2 primaryStep;
        Vector2 secondaryStep;

        int primary;
        int secondary;

        if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
        {
            primary = (int)Mathf.Abs(delta.y);
            secondary = (int)Mathf.Abs(delta.x);

            primaryStep = new Vector2(0, (int)Mathf.Sign(delta.y));
            secondaryStep = new Vector2((int)Mathf.Sign(delta.x), 0);
        }
        else
        {
            primary = (int)Mathf.Abs(delta.x);
            secondary = (int)Mathf.Abs(delta.y);

            primaryStep = new Vector2((int)Mathf.Sign(delta.x), 0);
            secondaryStep = new Vector2(0, (int)Mathf.Sign(delta.y));
        }

        var current = start;
        int error = 0;
        var list = new List<List<SightTileBase>>();
        bool isExistWall = false;
        while (true)
        {
            rayCheck++;
            if (DistPow(current, start) > sightDist * sightDist)
                break;

            if (totalSightDics.ContainsKey(current))
            {
                foreach (var sightTile in totalSightDics[current])
                {
                    if (sightTile.tileBase.isWall)
                    {
                        isExistWall = true;
                        break;
                    }    
                }

                list.Add(totalSightDics[current]);
                if (isExistWall)
                    break;
            }

            if (current == end)
                break;

            current += primaryStep;
            error += secondary;

            if (error * 2 >= primary)
            {
                current += secondaryStep;
                error -= primary;
            }
        }

        foreach (var elemList in list)
        {
            foreach (var elem in elemList)
            {
                elem.isInSight = true;
            }
        }
    }

    private int DistPow(Vector2 from, Vector2 to)
    {
        var delta = to - from;

        return (int)(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2));
    }

    private void UpdateObj()
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var wallDics = BattleMgr.Instance.tileMgr.wallDics;

        foreach (var list in sightList)
        {
            foreach (var tile in list)
            {
                tile.tileBase.EnableDisplay(tile.isInSight);
            }
        }

        foreach (var list in frontSightList)
        {
            foreach (var tile in list)
            {
                tile.tileBase.EnableDisplay(tile.isInSight);
            }
        }
    }

    public void AddFrontSight(PlayerableChar player)
    {
        UpdateFrontSight(player);
        UpdateObj();
    }

    public void UpdateFrontSight(PlayerableChar player)
    {
        List<SightTileBase> playerSight = null;
        for (int idx = 0; idx < playerableChars.Count; ++idx)
        {
            if (playerableChars[idx] == player)
            {
                playerSight = frontSightList[idx];
                break;
            }
        }

        var checkIdx = Vector2.zero;
        switch (player.direction)
        {
            case DirectionType.Top:
                checkIdx = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z + 1);
                CheckFrontSight(0, checkIdx, DirectionType.Top, playerSight);
                break;
            case DirectionType.Bot:
                checkIdx = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z - 1);
                CheckFrontSight(0, checkIdx, DirectionType.Bot, playerSight);
                break;
            case DirectionType.Left:
                checkIdx = new Vector2(player.currentTile.tileIdx.x - 1, player.currentTile.tileIdx.z);
                CheckFrontSight(0, checkIdx, DirectionType.Left, playerSight);
                break;
            case DirectionType.Right:
                checkIdx = new Vector2(player.currentTile.tileIdx.x + 1, player.currentTile.tileIdx.z);
                CheckFrontSight(0, checkIdx, DirectionType.Right, playerSight);
                break;
        }
    }

    public void CheckFrontSight(int forwardCnt, Vector2 checkIdx, DirectionType direction, List<SightTileBase> playerSight, bool isIncrease = true)
    {
        if (isIncrease)
        {
            forwardCnt++;
            if (forwardCnt > 5)
                isIncrease = false;
        }
        else
        {
            forwardCnt--;
            if (forwardCnt < 0)
                return;
        }

        //if (!playerSight.Exists(tile => new Vector2(tile.tileBase.tileIdx.x, tile.tileBase.tileIdx.z) == checkIdx))
        //{
        //    foreach (var tile in curFogDics[checkIdx])
        //        playerSight.Add(tile);
        //}

        //if (curFogDics.ContainsKey(checkIdx))
        //{
        //    var idx = playerSight.FindIndex(tile => new Vector2(tile.tileBase.tileIdx.x, tile.tileBase.tileIdx.z) == checkIdx);
        //    foreach (var tile in curFogDics[checkIdx])
        //        playerSight.Add(tile);
        //    playerSight[idx] = curFogDics[checkIdx];
        //}

        //switch (direction)
        //{
        //    case DirectionType.Top:
        //        checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
        //        CheckFrontSight(forwardCnt, checkIdx, DirectionType.Top, playerSight, isIncrease);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Left, playerSight);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Right, playerSight);
        //        break;
        //    case DirectionType.Bot:
        //        checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
        //        CheckFrontSight(forwardCnt, checkIdx, DirectionType.Bot, playerSight, isIncrease);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Left, playerSight);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Right, playerSight);
        //        break;
        //    case DirectionType.Left:
        //        checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
        //        CheckFrontSight(forwardCnt, checkIdx, DirectionType.Left, playerSight, isIncrease);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Top, playerSight);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Bot, playerSight);
        //        break;
        //    case DirectionType.Right:
        //        checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
        //        CheckFrontSight(forwardCnt, checkIdx, DirectionType.Right, playerSight, isIncrease);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Top, playerSight);
        //        CheckSideSight(0, forwardCnt, checkIdx, DirectionType.Bot, playerSight);
        //        break;
        //}

    }

    public void CheckSideSight(int sideCnt, int maxCnt, Vector2 checkIdx, DirectionType direction, Dictionary<Vector2, List<SightTileBase>> playerSight)
    {
        if (maxCnt < sideCnt)
            return;

        sideCnt++;
        if (!playerSight.ContainsKey(checkIdx))
            playerSight.Add(checkIdx, new List<SightTileBase>());

        if (totalSightDics.ContainsKey(checkIdx))
            playerSight[checkIdx] = totalSightDics[checkIdx];

        switch (direction)
        {
            case DirectionType.Top:
                checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
                CheckSideSight(sideCnt, maxCnt, checkIdx, DirectionType.Top, playerSight);
                break;
            case DirectionType.Bot:
                checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
                CheckSideSight(sideCnt, maxCnt, checkIdx, DirectionType.Bot, playerSight);
                break;
            case DirectionType.Left:
                checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
                CheckSideSight(sideCnt, maxCnt, checkIdx, DirectionType.Left, playerSight);
                break;
            case DirectionType.Right:
                checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
                CheckSideSight(sideCnt, maxCnt, checkIdx, DirectionType.Right, playerSight);
                break;
        }
    }
}