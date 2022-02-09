using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Player,
    Monster
}

public class SightTileBase
{
    public TileBase tileBase;
    public bool isInSight;
    public bool isInMonsterSight;
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
        tileBase.EnableDisplay(isInSight);
    }

    public void MonsterInit()
    {
        isInMonsterSight = false;
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

    public List<List<SightTileBase>> monsterSightList =
        new List<List<SightTileBase>>();

    private List<PlayerableChar> playerableChars;
    private List<MonsterChar> monsters;

    private int MAX_X_IDX;
    private int MAX_Z_IDX;

    private readonly int MaxFrontTile = 9;
    private readonly int MaxSideFrontTile = 7;
    public void Init()
    {
        playerableChars = BattleMgr.Instance.playerMgr.playerableChars;
        monsters = BattleMgr.Instance.monsterMgr.monsters;
        MAX_X_IDX = TileMgr.MAX_X_IDX;
        MAX_Z_IDX = TileMgr.MAX_Z_IDX;

        InitFog();
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

        for (var idx = 0; idx < monsters.Count; ++idx)
        {
            monsterSightList.Add(new List<SightTileBase>());
        }
    }

    public void UpdateFog()
    {
        InitAllSight();

        InitPlayerSight();

        if (BattleMgr.Instance.turn != BattleTurn.Player)
        {
            foreach (var player in playerableChars)
            {
                UpdateFrontSight(player);
            }
        }

        UpdateObj();
    }

    public void UpdateFog(PlayerableChar curPlayer)
    {
        InitAllSight();

        InitPlayerSight();

        foreach (var player in playerableChars)
        {
            if (player != curPlayer)
                UpdateFrontSight(player);
            else
                UpdateObj();
        }
    }

    private void InitAllSight()
    {
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

    public void InitPlayerSight()
    {
        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            var player = playerableChars[idx];
            if (player.gameObject == null)
                continue;

            var sightDistance = player.SightDistance;
            var curTileIdx = new Vector2(player.currentTile.tileIdx.x, player.currentTile.tileIdx.z);

            if (!calculateSightDics[idx].ContainsKey(curTileIdx))
            {
                for (var i = -sightDistance; i <= sightDistance; ++i)
                {
                    if (curTileIdx.y + i < 0)
                        continue;

                    for (var j = -sightDistance; j <= sightDistance; ++j)
                    {
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
                InitObstacle(idx, CharacterType.Player);
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


    private void InitObstacle(int idx, CharacterType characterType)
    {
        var tileIdx = Vector3.zero;
        GameObject character = null;
        var sightDist = 0;
        switch (characterType)
        {
            case CharacterType.Player:
                tileIdx = playerableChars[idx].tileIdx;
                character = playerableChars[idx].gameObject;
                sightDist = character.GetComponent<PlayerableChar>().SightDistance;
                break;
            case CharacterType.Monster:
                tileIdx = monsters[idx].tileIdx;
                character = monsters[idx].gameObject;
                sightDist = character.GetComponent<MonsterChar>().monsterStats.monster.sightRange;
                break;
        }
        if (!character.activeSelf)
            return;

        var maxX = sightDist;
        var maxY = sightDist;
        var startTileIdx = new Vector2(tileIdx.x, tileIdx.z);

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
                    CastRayTile(endTileIdx, startTileIdx, sightDist, characterType);
            }
        }
    }

    private void CastRayTile(Vector2 start, Vector2 end, int sightDist, CharacterType characterType)
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
                switch (characterType)
                {
                    case CharacterType.Player:
                        elem.isInSight = true;
                        break;
                    case CharacterType.Monster:
                        elem.isInMonsterSight = true;
                        break;
                }
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

        UpdateFrontDirection(curTileIdx, playerIdx, player.direction);

        UpdateObj();
    }

    private void UpdateFrontDirection(Vector3 curTileIdx, int playerIdx, DirectionType direction)
    {
        var advanceCenter = 0;
        for (var i = 1; i < MaxFrontTile; ++i)
        {
            var maxSide = 0;
            if (i < 3)
                maxSide = 2;
            else if (i < MaxSideFrontTile)
                maxSide = i;
            else if (i == MaxFrontTile - 2)
                maxSide = 5;
            else if (i == MaxFrontTile - 1)
                maxSide = 3;

            var centerIdx = Vector2.zero;
            switch (direction)
            {
                case DirectionType.Top:
                    centerIdx = new Vector2(curTileIdx.x, curTileIdx.z + i);
                    break;
                case DirectionType.Bot:
                    centerIdx = new Vector2(curTileIdx.x, curTileIdx.z - i);
                    break;
                case DirectionType.Left:
                    centerIdx = new Vector2(curTileIdx.x - i, curTileIdx.z);
                    break;
                case DirectionType.Right:
                    centerIdx = new Vector2(curTileIdx.x + i, curTileIdx.z);
                    break;
                case DirectionType.None:
                    return;
            }

            var isCorrectX = -1 < centerIdx.x && centerIdx.x < TileMgr.MAX_X_IDX;
            var isCorrectZ = -1 < centerIdx.y && centerIdx.y < TileMgr.MAX_Z_IDX;

            if (!(isCorrectX && isCorrectZ))
                break;
            advanceCenter = i;

            RegistFrontSight(centerIdx, playerIdx, maxSide);

            for (var j = 1; j < maxSide; ++j)
            {
                var sideTileIdx1 = Vector2.zero;
                var sideTileIdx2 = Vector2.zero;
                switch (direction)
                {
                    case DirectionType.Top:
                        sideTileIdx1 = new Vector2(curTileIdx.x - j, curTileIdx.z + i);
                        sideTileIdx2 = new Vector2(curTileIdx.x + j, curTileIdx.z + i);
                        break;
                    case DirectionType.Bot:
                        sideTileIdx1 = new Vector2(curTileIdx.x - j, curTileIdx.z - i);
                        sideTileIdx2 = new Vector2(curTileIdx.x + j, curTileIdx.z - i);
                        break;
                    case DirectionType.Left:
                        sideTileIdx1 = new Vector2(curTileIdx.x - i, curTileIdx.z - j);
                        sideTileIdx2 = new Vector2(curTileIdx.x - i, curTileIdx.z + j);
                        break;
                    case DirectionType.Right:
                        sideTileIdx1 = new Vector2(curTileIdx.x + i, curTileIdx.z - j);
                        sideTileIdx2 = new Vector2(curTileIdx.x + i, curTileIdx.z + j);
                        break;
                }

                RegistFrontSight(sideTileIdx1, playerIdx, maxSide);
                RegistFrontSight(sideTileIdx2, playerIdx, maxSide);
            }
        }

        var startTileIdx = new Vector2(curTileIdx.x, curTileIdx.z);
        var endSide = 0;
        if (advanceCenter < 2)
            endSide = 1;
        else
            endSide = advanceCenter - 1;

        for (var i = -endSide; i <= endSide; ++i)
        {
            var endTileIdx = Vector2.zero;
            switch (direction)
            {
                case DirectionType.Top:
                    endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + i, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + advanceCenter, 0, MAX_Z_IDX));
                    break;
                case DirectionType.Bot:
                    endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + i, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y - advanceCenter, 0, MAX_Z_IDX));
                    break;
                case DirectionType.Left:
                    endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x - advanceCenter, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + i, 0, MAX_Z_IDX));
                    break;
                case DirectionType.Right:
                    endTileIdx = new Vector2(Mathf.Clamp(startTileIdx.x + advanceCenter, 0, MAX_X_IDX), Mathf.Clamp(startTileIdx.y + i, 0, MAX_Z_IDX));
                    break;
            }
            CastRayTile(endTileIdx, startTileIdx, MaxFrontTile, CharacterType.Player);
        }
    }

    private void RegistFrontSight(Vector2 registTileIdx, int playerIdx, int accuracy)
    {
        if (totalSightDics.ContainsKey(registTileIdx))
        {
            foreach (var sightTile in totalSightDics[registTileIdx])
            {
                sightTile.tileBase.accuracy = accuracy;
                frontSightList[playerIdx].Add(sightTile);
            }
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
    }

    public bool GetMonsterInPlayerSight(GameObject gameObj)
    {
        foreach (var sight in sightList)
        {
            foreach (var tile in sight)
            {
                if (tile.tileBase.charObj == gameObj)
                    return true;
            }
        }

        foreach (var frontSight in frontSightList)
        {
            foreach (var tile in frontSight)
            {
                if (tile.tileBase.charObj == gameObj)
                    return true;
            }
        }

        return false;
    }

    public List<MonsterChar> GetMonsterInPlayerSight(PlayerableChar player)
    {
        var playerIdx = -1;

        for (var idx = 0; idx < playerableChars.Count; ++idx)
        {
            if (playerableChars[idx] == player)
            {
                playerIdx = idx;
                break;
            }    
        }

        var monsterList = new List<MonsterChar>();

        foreach (var tile in sightList[playerIdx])
        {
            if (tile.tileBase.charObj != null)
            {
                var monster = tile.tileBase.charObj.GetComponent<MonsterChar>();
                if (monster != null)
                    monsterList.Add(monster);
            }
        }

        foreach (var tile in frontSightList[playerIdx])
        {
            if (tile.tileBase.charObj != null)
            {
                var monster = tile.tileBase.charObj.GetComponent<MonsterChar>();
                if (monster != null)
                {
                    if (monsterList.Exists(check => check != monster))
                        monsterList.Add(monster);
                }
            }
        }

        return monsterList;
    }

    public PlayerableChar GetPlayerInMonsterSight(int idx)
    {
        var monster = monsters[idx];
        var monsterSight = monsterSightList[idx];

        foreach (var sight in monsterSight)
        {
            if (sight.isInMonsterSight && sight.tileBase.charObj != null)
            {
                foreach (var player in playerableChars)
                {
                    if (sight.tileBase.charObj == player.gameObject)
                        return player;
                }
            }
        }

        return null;
    }

    public void InitMonsterSight(int monsterIdx)
    {
        var monsterSight = monsterSightList[monsterIdx];
        var monster = monsters[monsterIdx];
        var sightDistance = monster.monsterStats.monster.sightRange;
        var curTileIdx = new Vector2(monster.tileIdx.x, monster.tileIdx.z);

        foreach (var sight in monsterSight)
        {
            sight.MonsterInit();
        }

        monsterSight.Clear();

        for (var i = -sightDistance; i <= sightDistance; ++i)
        {
            if (curTileIdx.y + i < 0)
                continue;

            for (var j = -sightDistance; j <= sightDistance; ++j)
            {
                if (curTileIdx.x + j < 0)
                    continue;

                var checkIdx = new Vector2(curTileIdx.x + j, curTileIdx.y + i);
                if (Mathf.Abs(i) + Mathf.Abs(j) > sightDistance)
                    continue;

                if (totalSightDics.ContainsKey(checkIdx))
                {
                    foreach (var sightTile in totalSightDics[checkIdx])
                    {
                        monsterSight.Add(sightTile);
                    }
                }
            }
        }
        InitObstacle(monsterIdx, CharacterType.Monster);
    }
    
    public bool GetSightDisplay(TileBase tile)
    {
        var newTileIdx = new Vector3(tile.tileIdx.x, tile.tileIdx.z);
        if (totalSightDics.ContainsKey(newTileIdx))
        {
            foreach (var sight in totalSightDics[newTileIdx])
            {
                if (sight.isInSight)
                    return true;
            }

            return false;
        }
        else
            return false;
    }
}