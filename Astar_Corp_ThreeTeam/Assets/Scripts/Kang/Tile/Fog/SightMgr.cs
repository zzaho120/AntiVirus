using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SightTileBase
{
    public TileBase tileBase;
    public bool isInSight;
    public SightTileBase(TileBase tileBase)
    {
        this.tileBase = tileBase;
    }

    public SightTileBase(SightTileBase sightTile)
    {
        tileBase = sightTile.tileBase;
        isInSight = sightTile.isInSight;
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
    private bool inited = false;

    private int MAX_X_IDX;
    private int MAX_Z_IDX;

    private readonly int MaxFrontTile = 8;
    public void Init()
    {
        playerableChars = BattleMgr.Instance.playerMgr.playerableChars;
        MAX_X_IDX = TileMgr.MAX_X_IDX;
        MAX_Z_IDX = TileMgr.MAX_Z_IDX;
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
        InitAllSight();

        InitPlayerSight();

        if (!(BattleMgr.Instance.turn == BattleTurn.Player))
        {
            foreach (var player in playerableChars)
            {
                UpdateFrontSight(player);
            }
        }

        UpdateObj();


        //Debug.Log($"checkTime : {checkTime}");
        //Debug.Log($"playerCheck : {playerCheck}");
        //Debug.Log($"rayCheck : {rayCheck}");
    }

    public void UpdateFog(PlayerableChar curPlayer)
    {
        InitAllSight();

        InitPlayerSight();

        foreach (var player in playerableChars)
        {
            if (player != curPlayer)
                UpdateFrontSight(player);
        }

        UpdateObj();
    }

    private void InitAllSight()
    {
        checkTime = 0;
        playerCheck = 0;
        rayCheck = 0;
        foreach (var list in sightList)
        {
            list.Clear();
        }

        foreach (var list in frontSightList)
        {
            list.Clear();
        }

        foreach (var pair in totalSightDics)
        {
            var list = pair.Value;
            foreach (var elem in list)
            {
                elem.Init();
            }
        }
    }

    private void InitPlayerSight()
    {
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            var player = playerableChars[idx];
            if (player.gameObject == null)
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
                InitObstacle(idx);
                var tempList = new List<SightTileBase>();
                foreach (var sightTile in sightList[idx])
                {
                    tempList.Add(new SightTileBase(sightTile));
                }
                calculateSightDics[idx].Add(curTileIdx, tempList);
            }
            else
            {
                var tempList = new List<SightTileBase>();
                foreach (var sightTile in calculateSightDics[idx][curTileIdx])
                {
                    tempList.Add(new SightTileBase(sightTile));
                }
                sightList[idx] = tempList;
            }
        }
    }

    private void InitObstacle(int playerIdx)
    {
        var player = playerableChars[playerIdx];
        if (!player.gameObject.activeSelf)
            return;

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

                var endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + j, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + i, 0, MAX_Z_IDX));
                var absX = Mathf.Abs(j);
                var absY = Mathf.Abs(i);
                var specificCase = absX == absY &&
                    (absX + absY) == sightDist - 1;
                if (absY + absX == sightDist || specificCase)
                    CastRayTile(endTileIdx, startTileIdx, sightDist, playerIdx);
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
                        if (current == start)
                            continue;

                        isExistWall = true;
                        sightTile.isInSight = true;
                        break;
                    }
                }
                if (isExistWall)
                {
                    foreach (var elemList in list)
                    {
                        foreach (var elem in elemList)
                        {
                            elem.isInSight = false;
                        }
                    }
                    isExistWall = false;
                    list.Clear();
                }
                list.Add(totalSightDics[current]);
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

                tile.tileBase.SetHighlight();
            }
        }
    }

    public void UpdateFrontSight(PlayerableChar player)
    {
        var playerIdx = -1;
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            if (playerableChars[idx] == player)
                playerIdx = idx;
        }

        RemoveFrontSight(player);

        var curTileIdx = playerableChars[playerIdx].currentTile.tileIdx;

        switch (player.direction)
        {
            case DirectionType.Top:
                UpdateTopFront(curTileIdx, playerIdx);
                break;
            case DirectionType.Bot:
                UpdateBotFront(curTileIdx, playerIdx);
                break;
            case DirectionType.Left:
                UpdateLeftFront(curTileIdx, playerIdx);
                break;
            case DirectionType.Right:
                UpdateRightFront(curTileIdx, playerIdx);
                break;
        }

        UpdateObj();
    }

    private void UpdateTopFront(Vector3 curTileIdx, int playerIdx)
    {
        var maxI = 0;
        for (var i = 1; i < MaxFrontTile; ++i)
        {
            var maxJ = 0;
            if (i == 1 || i == 2)
                maxJ = 2;
            else if (i > 2)
                maxJ = i;

            maxI = i;
            var addTileIdx = new Vector2(curTileIdx.x, curTileIdx.z + i);
            if (addTileIdx.y >= TileMgr.MAX_Z_IDX)
                break;

            foreach (var sightTile in totalSightDics[addTileIdx])
            {
                sightTile.tileBase.accuracy = maxJ;
                frontSightList[playerIdx].Add(sightTile);
            }

            for (var j = 1; j < maxJ; ++j)
            {
                addTileIdx = new Vector2(curTileIdx.x - j, curTileIdx.z + i);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxJ;
                    frontSightList[playerIdx].Add(sightTile);
                }
                addTileIdx = new Vector2(curTileIdx.x + j, curTileIdx.z + i);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxJ;
                    frontSightList[playerIdx].Add(sightTile);
                }
            }
        }
        var startTileIdx = new Vector2(curTileIdx.x, curTileIdx.z);

        var val = 0;
        if (maxI < 2)
            val = 1;
        else
            val = maxI - 1;

        for (var i = -val; i <= val; ++i)
        {
            var endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + i, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + maxI, 0, MAX_Z_IDX));
            CastRayTile(endTileIdx, startTileIdx, MaxFrontTile, playerIdx);
        }
    }

    private void UpdateBotFront(Vector3 curTileIdx, int playerIdx)
    {
        var maxI = 0;
        for (var i = 1; i < MaxFrontTile; ++i)
        {
            var maxJ = 0;
            if (i == 1 || i == 2)
                maxJ = 2;
            else if (i > 2)
                maxJ = i;

            maxI = i;
            var addTileIdx = new Vector2(curTileIdx.x, curTileIdx.z - i);
            if (addTileIdx.y >= TileMgr.MAX_Z_IDX)
                break;

            foreach (var sightTile in totalSightDics[addTileIdx])
            {
                sightTile.tileBase.accuracy = maxJ;
                frontSightList[playerIdx].Add(sightTile);
            }

            for (var j = 1; j < maxJ; ++j)
            {
                addTileIdx = new Vector2(curTileIdx.x - j, curTileIdx.z - i);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxJ;
                    frontSightList[playerIdx].Add(sightTile);
                }
                addTileIdx = new Vector2(curTileIdx.x + j, curTileIdx.z - i);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxJ;
                    frontSightList[playerIdx].Add(sightTile);
                }
            }
        }
        var startTileIdx = new Vector2(curTileIdx.x, curTileIdx.z);

        var val = 0;
        if (maxI < 2)
            val = 1;
        else
            val = maxI - 1;

        for (var i = -val; i <= val; ++i)
        {
            var endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + i, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y - maxI, 0, MAX_Z_IDX));
            CastRayTile(endTileIdx, startTileIdx, MaxFrontTile, playerIdx);
        }
    }

    private void UpdateLeftFront(Vector3 curTileIdx, int playerIdx)
    {
        var maxJ = 0;
        for (var i = 1; i < MaxFrontTile; ++i)
        {
            var maxI = 0;
            if (i == 1 || i == 2)
                maxI = 2;
            else if (i > 2)
                maxI = i;

            maxJ = i;
            var addTileIdx = new Vector2(curTileIdx.x - i, curTileIdx.z);
            if (addTileIdx.y >= TileMgr.MAX_Z_IDX)
                break;

            foreach (var sightTile in totalSightDics[addTileIdx])
            {
                sightTile.tileBase.accuracy = maxI;
                frontSightList[playerIdx].Add(sightTile);
            }

            for (var j = 1; j < maxI; ++j)
            {
                addTileIdx = new Vector2(curTileIdx.x - i, curTileIdx.z - j);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxI;
                    frontSightList[playerIdx].Add(sightTile);
                }
                addTileIdx = new Vector2(curTileIdx.x - i, curTileIdx.z + j);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxI;
                    frontSightList[playerIdx].Add(sightTile);
                }
            }
        }
        var startTileIdx = new Vector2(curTileIdx.x, curTileIdx.z);

        var val = 0;
        if (maxJ < 2)
            val = 1;
        else
            val = maxJ - 1;

        for (var i = -val; i <= val; ++i)
        {
            var endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x - maxJ, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + i, 0, MAX_Z_IDX));
            CastRayTile(endTileIdx, startTileIdx, MaxFrontTile, playerIdx);
        }
    }
    private void UpdateRightFront(Vector3 curTileIdx, int playerIdx)
    {
        var maxJ = 0;
        for (var i = 1; i < MaxFrontTile; ++i)
        {
            var maxI = 0;
            if (i == 1 || i == 2)
                maxI = 2;
            else if (i > 2)
                maxI = i;

            maxJ = i;
            var addTileIdx = new Vector2(curTileIdx.x + i, curTileIdx.z);
            if (addTileIdx.y >= TileMgr.MAX_Z_IDX)
                break;

            foreach (var sightTile in totalSightDics[addTileIdx])
            {
                sightTile.tileBase.accuracy = maxI;
                frontSightList[playerIdx].Add(sightTile);
            }

            for (var j = 1; j < maxI; ++j)
            {
                addTileIdx = new Vector2(curTileIdx.x + i, curTileIdx.z - j);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxI;
                    frontSightList[playerIdx].Add(sightTile);
                }
                addTileIdx = new Vector2(curTileIdx.x + i, curTileIdx.z + j);
                foreach (var sightTile in totalSightDics[addTileIdx])
                {
                    sightTile.tileBase.accuracy = maxI;
                    frontSightList[playerIdx].Add(sightTile);
                }
            }
        }
        var startTileIdx = new Vector2(curTileIdx.x, curTileIdx.z);

        var val = 0;
        if (maxJ < 2)
            val = 1;
        else
            val = maxJ - 1;

        for (var i = -val; i <= val; ++i)
        {
            var endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + maxJ, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + i, 0, MAX_Z_IDX));
            CastRayTile(endTileIdx, startTileIdx, MaxFrontTile, playerIdx);
        }
    }
    public List<SightTileBase> GetFrontSight(PlayerableChar player)
    {
        for (var idx = 0; idx < frontSightList.Count; ++idx)
        {
            if (playerableChars[idx] == player)
                return frontSightList[idx];
        }
        return null;
    }

    public void RemoveFrontSight(PlayerableChar player)
    {
        var playerIdx = -1;
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            if (playerableChars[idx] == player)
                playerIdx = idx;
        }

        if (frontSightList[playerIdx].Count > 0)
        {
            foreach (var sightTile in frontSightList[playerIdx])
            {
                sightTile.Init();
            }
        }
        frontSightList[playerIdx].Clear();
        InitPlayerSight();
        foreach (var frontSight in frontSightList)
        {
            foreach (var tile in frontSight)
            {
                tile.isInSight = true;
            }
        }
        UpdateObj();
    }
}