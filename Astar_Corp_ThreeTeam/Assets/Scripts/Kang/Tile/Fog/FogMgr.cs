using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTileBase
{
    public TileBase tileBase;
    public bool isTileInSight;
    public DirectionType enabledDirType;

    public FogTileBase(TileBase tileBase)
    {
        this.tileBase = tileBase;
    }

    public void Init()
    {
        isTileInSight = false;
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
        playerableChars = BattleMgr.Instance.player.playerableChars;
        InitFog();
        UpdateFog();
    }

    private void InitFog()
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;

        foreach (var pair in tileDics)
        {
            var checkIdx = new Vector2(pair.Key.x, pair.Key.z);

            if (!curFogDics.ContainsKey(checkIdx))
                curFogDics.Add(checkIdx, new List<FogTileBase>());
            curFogDics[checkIdx].Add(new FogTileBase(pair.Value));

            if (!everFogDics.ContainsKey(pair.Key))
                everFogDics.Add(pair.Key, false);
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
        InitObstacleSight();
        UpdateObj();
    }
    private void UpdateEverFog()
    {
        // 에버포그에 커런트 정보를 추가할것
    }

    private void UpdateCurrentFog()
    {
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
            var player = playerableChars[0];
            if (!player.gameObject.activeSelf)
                continue;

            var sightDistance = player.sightDistance;
            var currentTile = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z);
            var sightCnt = 0;

            CheckPlayerSight(new Vector2(currentTile.x, currentTile.y + 1), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x, currentTile.y - 1), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x - 1, currentTile.y), sightCnt, sightDistance, idx);
            CheckPlayerSight(new Vector2(currentTile.x + 1, currentTile.y), sightCnt, sightDistance, idx);
        }
    }

    private void CheckPlayerSight(Vector2 tileIdx, int sightCnt, int maxSight, int playerIdx)
    {
        if (!curFogDics.ContainsKey(tileIdx))
            return;

        sightCnt++;
        if (sightCnt > maxSight)
            return;


        foreach (var tile in curFogDics[tileIdx])
        {
            tile.isTileInSight = true;
        }

        if (!playerSightDics[playerIdx].ContainsKey(tileIdx))
            playerSightDics[playerIdx].Add(tileIdx, curFogDics[tileIdx]);

        CheckPlayerSight(new Vector2(tileIdx.x, tileIdx.y + 1), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x, tileIdx.y - 1), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x - 1, tileIdx.y), sightCnt, maxSight, playerIdx);
        CheckPlayerSight(new Vector2(tileIdx.x + 1, tileIdx.y), sightCnt, maxSight, playerIdx);
    }

    private void InitObstacleSight()
    {
        
    }


    private void UpdateObj()
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;

        foreach (var pair in tileDics)
        {
            var tile = pair.Value;
            EnableTile(tile, false, DirectionType.None);
        }

        foreach (var pair in curFogDics)
        {
            var list = pair.Value;
            foreach (var fogTile in list)
            {
                var tile = fogTile.tileBase;
                EnableTile(tile, fogTile.isTileInSight, fogTile.enabledDirType);
            }
        }
    }

    private void EnableTile(TileBase tileBase, bool isTileEnabled, DirectionType direction)
    {
        tileBase.EnableDisplay(isTileEnabled);

        var objList = tileBase.objList;
        foreach (var obj in objList)
        {
            var wall = obj.GetComponent<WallBase>();
            if (wall != null)
            {
                //if ((direction & wall.type) != 0)
                    wall.EnableDisplay(true);
                //else
                //    wall.EnableDisplay(false);
            }

            var door = obj.GetComponent<DoorBase>();
            if (door != null)
                door.EnableDisplay();
        }
    }
}