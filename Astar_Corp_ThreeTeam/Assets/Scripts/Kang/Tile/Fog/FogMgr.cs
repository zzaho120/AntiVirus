using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTileBase
{
    public TileBase tileBase;
    public WallBase wallBase;
    public bool isInSight;

    public FogTileBase(TileBase tileBase)
    {
        this.tileBase = tileBase;
    }

    public FogTileBase(WallBase wallBase)
    {
        this.wallBase = wallBase;
    }

    public void Init()
    {
        isInSight = false;
    }
}

public class FogMgr : MonoBehaviour
{
    // 합쳐놓은 타일
    // key의 x는 vector3의 x
    // key의 y는 vector3의 z
    public Dictionary<Vector2, List<FogTileBase>> curFogDics =
        new Dictionary<Vector2, List<FogTileBase>>();

    public List<Dictionary<Vector2, List<FogTileBase>>> playerSightDics =
        new List<Dictionary<Vector2, List<FogTileBase>>>();

    // 한 번이라도 시야가 확보된 시야
    public Dictionary<Vector3, bool> everFogDics =
        new Dictionary<Vector3, bool>();

    private List<PlayerableChar> playerableChars;

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

            if (!curFogDics.ContainsKey(checkIdx))
                curFogDics.Add(checkIdx, new List<FogTileBase>());
            curFogDics[checkIdx].Add(new FogTileBase(pair.Value));

            if (!everFogDics.ContainsKey(pair.Key))
                everFogDics.Add(pair.Key, false);
        }

        foreach (var pair in wallDics)
        {
            var checkIdx = new Vector2(pair.Key.x, pair.Key.z);

            if (!curFogDics.ContainsKey(checkIdx))
                curFogDics.Add(checkIdx, new List<FogTileBase>());
            curFogDics[checkIdx].Add(new FogTileBase(pair.Value));
        }

        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            playerSightDics.Add(new Dictionary<Vector2, List<FogTileBase>>());
        }
    }

    public void UpdateFog()
    {
        //UpdateEverFog();
        UpdateCurrentFog();
        InitPlayerSight();
        InitObstacle();
        UpdateObj();
    }

    private void UpdateCurrentFog()
    {
        foreach (var dics in playerSightDics)
        {
            dics.Clear();
        }

        foreach (var pair in curFogDics)
        {
            var list = pair.Value;
            foreach (var tile in list)
            {
                tile.Init();
            }
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
            var currentTile = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z);
            var sightCnt = 0;

            CheckPlayerSight(new Vector2(currentTile.x, currentTile.y + 1), new Vector2(currentTile.x, currentTile.y + 0.5f), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x, currentTile.y - 1), new Vector2(currentTile.x, currentTile.y - 0.5f), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x - 1, currentTile.y), new Vector2(currentTile.x - 0.5f, currentTile.y), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x + 1, currentTile.y), new Vector2(currentTile.x + 0.5f, currentTile.y), sightCnt, sightDistance, idx);
        }
    }

    private void CheckPlayerSight(Vector2 tileIdx, Vector2 wallIdx, int sightCnt, int maxSight, int playerIdx)
    {
        if (!curFogDics.ContainsKey(tileIdx))
            return;

        sightCnt++;
        if (sightCnt > maxSight)
            return;


        if (curFogDics.ContainsKey(wallIdx))
        {
            foreach (var wall in curFogDics[wallIdx])
                wall.isInSight = true;
        }

        //foreach (var tile in curFogDics[tileIdx])
        //{
        //    tile.isInSight = true;
        //}

        if (!playerSightDics[playerIdx].ContainsKey(tileIdx))
            playerSightDics[playerIdx].Add(tileIdx, curFogDics[tileIdx]);

        CheckPlayerSight(new Vector2(tileIdx.x, tileIdx.y + 1), new Vector2(tileIdx.x, tileIdx.y + 0.5f), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x, tileIdx.y - 1), new Vector2(tileIdx.x, tileIdx.y - 0.5f), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x - 1, tileIdx.y), new Vector2(tileIdx.x - 0.5f, tileIdx.y), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x + 1, tileIdx.y), new Vector2(tileIdx.x + 0.5f, tileIdx.y), sightCnt, maxSight, playerIdx);
    }

    private void InitObstacle()
    {
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            var curTileIdx = playerableChars[idx].currentTile.tileIdx;
            var checkIdx = new Vector2(curTileIdx.x, curTileIdx.z);
            foreach (var pair in playerSightDics[idx])
            {
                var target = pair.Key;
                if (target.x == checkIdx.x)
                    CheckSameX(target, checkIdx);
                else if (target.y == checkIdx.y)
                    CheckSameY(target, checkIdx);
                else
                    CheckSlope(target, checkIdx);
            }
        }
    }

    private void CheckSameX(Vector2 targetIdx, Vector2 checkIdx)
    {
        var wallIdx = new Vector2(-1, -1);

        do
        {
            if (targetIdx.y > checkIdx.y)
            {
                wallIdx = new Vector2(checkIdx.x, checkIdx.y + 0.5f);
                checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
            }
            else if (targetIdx.y < checkIdx.y)
            {
                wallIdx = new Vector2(checkIdx.x, checkIdx.y - 0.5f);
                checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
            }

            if (!curFogDics.ContainsKey(checkIdx))
                continue;

            if (curFogDics.ContainsKey(wallIdx))
            {
                return;
            }

        }
        while (targetIdx.y != checkIdx.y);
        foreach (var fogTile in curFogDics[targetIdx])
        {
            fogTile.isInSight = true;
        }
    }

    private void CheckSameY(Vector2 targetIdx, Vector2 checkIdx)
    {
        var wallIdx = new Vector2(-1, -1);

        do
        {
            if (targetIdx.x > checkIdx.x)
            {
                wallIdx = new Vector2(checkIdx.x + 0.5f, checkIdx.y);
                checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
            }
            else if (targetIdx.x < checkIdx.x)
            {
                wallIdx = new Vector2(checkIdx.x - 0.5f, checkIdx.y);
                checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
            }

            if (!curFogDics.ContainsKey(checkIdx))
                continue;

            if (curFogDics.ContainsKey(wallIdx))
            {
                return;
            }

        }
        while (targetIdx.x != checkIdx.x);

        foreach (var fogTile in curFogDics[targetIdx])
        {
            fogTile.isInSight = true;
        }
    }

    private void CheckSlope(Vector2 targetIdx, Vector2 checkIdx)
    {
        var aX = 0;
        var aY = 0;
        var slopeX = Mathf.Abs(targetIdx.x - checkIdx.x);
        var slopeY = Mathf.Abs(targetIdx.y - checkIdx.y);
        var aG = slopeY / slopeX;
        var wallIdx = new Vector2(-1, -1);

        if (slopeX == 1 && slopeY == 1)
        {
            CheckCaseOne(targetIdx, checkIdx);
            return;
        }

        if (aG >= 1)
        {
            do
            {
                if (slopeX == 0)
                {
                    CheckSameX(targetIdx, checkIdx);
                    break;
                }

                var fomula = (aX / (aY + 1)) < aG;
                if (fomula)
                {
                    ++aX;
                    if (targetIdx.x > checkIdx.x)
                    {
                        wallIdx = new Vector2(checkIdx.x + 0.5f, checkIdx.y);
                        checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
                    }
                    else
                    {
                        wallIdx = new Vector2(checkIdx.x - 0.5f, checkIdx.y);
                        checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
                    }
                }
                else
                {
                    ++aY;
                    if (targetIdx.y > checkIdx.y)
                    {
                        wallIdx = new Vector2(checkIdx.x, checkIdx.y + 0.5f);
                        checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
                    }
                    else
                    {
                        wallIdx = new Vector2(checkIdx.x, checkIdx.y - 0.5f);
                        checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
                    }
                }

                if (!curFogDics.ContainsKey(checkIdx))
                    continue;

                if (curFogDics.ContainsKey(wallIdx))
                {
                    return;
                }

                slopeX = Mathf.Abs(targetIdx.x - checkIdx.x);
                slopeY = Mathf.Abs(targetIdx.y - checkIdx.y);
                aG = slopeY / slopeX;
            }
            while (targetIdx != checkIdx);
            foreach (var fogTile in curFogDics[targetIdx])
            {
                fogTile.isInSight = true;
            }
        }
        else
        {
            do
            {
                if (slopeX == 0)
                {
                    CheckSameX(targetIdx, checkIdx);
                    break;
                }
                var fomula = (aY / (aX + 1)) < aG;
                if (fomula)
                {
                    ++aY;
                    if (targetIdx.y > checkIdx.y)
                    {
                        wallIdx = new Vector2(checkIdx.x, checkIdx.y + 0.5f);
                        checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
                    }
                    else
                    {
                        wallIdx = new Vector2(checkIdx.x, checkIdx.y - 0.5f);
                        checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
                    }
                }
                else
                {
                    ++aX;
                    if (targetIdx.x > checkIdx.x)
                    {
                        wallIdx = new Vector2(checkIdx.x + 0.5f, checkIdx.y);
                        checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
                    }
                    else
                    {
                        wallIdx = new Vector2(checkIdx.x - 0.5f, checkIdx.y);
                        checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
                    }
                }

                if (!curFogDics.ContainsKey(checkIdx))
                    continue;

                if (curFogDics.ContainsKey(wallIdx))
                {
                    return;
                }

                slopeX = Mathf.Abs(targetIdx.x - checkIdx.x);
                slopeY = Mathf.Abs(targetIdx.y - checkIdx.y);
                aG = slopeY / slopeX;
            }
            while (targetIdx != checkIdx);
            foreach (var fogTile in curFogDics[targetIdx])
            {
                fogTile.isInSight = true;
            }
        }
    }

    private void CheckCaseOne(Vector2 targetIdx, Vector2 checkIdx)
    {
        var startCheckIdx = checkIdx;
        var wallIdx = new Vector2(-1, -1);
        var wallCollision1 = false;
        var wallCollision2 = false;

        if (targetIdx.y > checkIdx.y)
        {
            wallIdx = new Vector2(checkIdx.x, checkIdx.y + 0.5f);
            checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
        }
        else
        {
            wallIdx = new Vector2(checkIdx.x, checkIdx.y - 0.5f);
            checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
        }

        if (curFogDics.ContainsKey(wallIdx))
            wallCollision1 = true;

        if (targetIdx.x > checkIdx.x)
        {
            wallIdx = new Vector2(checkIdx.x + 0.5f, checkIdx.y);
            checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
        }
        else
        {
            wallIdx = new Vector2(checkIdx.x - 0.5f, checkIdx.y);
            checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
        }

        if (curFogDics.ContainsKey(wallIdx))
            wallCollision1 = true;

        checkIdx = startCheckIdx;

        if (targetIdx.x > checkIdx.x)
        {
            wallIdx = new Vector2(checkIdx.x + 0.5f, checkIdx.y);
            checkIdx = new Vector2(checkIdx.x + 1, checkIdx.y);
        }
        else
        {
            wallIdx = new Vector2(checkIdx.x - 0.5f, checkIdx.y);
            checkIdx = new Vector2(checkIdx.x - 1, checkIdx.y);
        }

        if (curFogDics.ContainsKey(wallIdx))
            wallCollision2 = true;

        if (targetIdx.y > checkIdx.y)
        {
            wallIdx = new Vector2(checkIdx.x, checkIdx.y + 0.5f);
            checkIdx = new Vector2(checkIdx.x, checkIdx.y + 1);
        }
        else
        {
            wallIdx = new Vector2(checkIdx.x, checkIdx.y - 0.5f);
            checkIdx = new Vector2(checkIdx.x, checkIdx.y - 1);
        }


        if (curFogDics.ContainsKey(wallIdx))
            wallCollision2 = true;

        if (!wallCollision1 && !wallCollision2)
        {
            foreach (var fogTile in curFogDics[targetIdx])
            {
                fogTile.isInSight = true;
            }
        }
    }

    private void UpdateObj()
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;
        var wallDics = BattleMgr.Instance.tileMgr.wallDics;

        foreach (var pair in tileDics)
        {
            var tile = pair.Value;
            tile.EnableDisplay(false);
        }

        foreach (var pair in wallDics)
        {
            var wall = pair.Value;
            wall.EnableDisplay(false);
        }

        foreach (var pair in curFogDics)
        {
            var list = pair.Value;
            foreach (var fogTile in list)
            {
                var tile = fogTile.tileBase;
                if (tile != null)
                    tile.EnableDisplay(fogTile.isInSight);

                var wall = fogTile.wallBase;
                if (wall != null)
                    wall.EnableDisplay(fogTile.isInSight);
            }
        }
    }
}