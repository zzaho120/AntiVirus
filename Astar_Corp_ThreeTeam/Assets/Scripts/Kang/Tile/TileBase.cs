using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    private TileMgr mgr;

    [Header("Value")]
    public Vector3 tileIdx;
    public GameObject tileObj;

    [Header("List")]
    public List<TileBase> adjNodes;
    public List<GameObject> objList;

    public void Init(TileMgr tileMgr)
    {
        mgr = tileMgr;

        InitIdx();
        InitWall();
    }

    private void InitIdx()
    {
        tileIdx = transform.position;
        gameObject.name = $"Tile ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
    }

    private void InitWall()
    {
        foreach (var obj in objList)
        {
            var wall = obj.GetComponent<WallBase>();
            if (wall != null)
            {
                wall.Init();

                if (wall.tileIdx.y == tileIdx.y) // °°Àº Ãþ
                    wall.parentFloor = this;
                else if (wall.tileIdx.y == tileIdx.y - 1) // À­ Ãþ
                    wall.parentSeiling = this;
            }
        }
    }

    public bool CheckObj(DirectionType type)
    {
        foreach (var obj in objList)
        {
            var wall = obj.GetComponent<WallBase>();
            if(wall != null)
            {
                if (type == wall.type)
                    return true;
            }

            var door = obj.GetComponent<DoorBase>();
            if(door != null)
            {
                if (type == door.type && !door.isOpenDoor)
                    return true;
            }
        }
        return false;
    }

    public void OpenDoor(bool isContainAdjTile)
    {
        foreach (var obj in objList)
        {
            var door = obj.GetComponent<DoorBase>();

            if (door != null)
                door.OnOpenDoor();
        }

        if (isContainAdjTile)
        {
            OpenDoor4Dir(DirectionType.Top);
            OpenDoor4Dir(DirectionType.Bot);
            OpenDoor4Dir(DirectionType.Left);
            OpenDoor4Dir(DirectionType.Right);
        }
    }

    public void OpenDoor4Dir(DirectionType dir)
    {
        var tileDics = BattleMgr.Instance.tileMgr.tileDics;

        Vector3 dirVector3 = Vector3.zero;
        switch (dir)
        {
            case DirectionType.Top:
                dirVector3 = new Vector3(tileIdx.x, tileIdx.y, tileIdx.z + 1);
                break;
            case DirectionType.Bot:
                dirVector3 = new Vector3(tileIdx.x, tileIdx.y, tileIdx.z - 1);
                break;
            case DirectionType.Left:
                dirVector3 = new Vector3(tileIdx.x - 1, tileIdx.y, tileIdx.z);
                break;
            case DirectionType.Right:
                dirVector3 = new Vector3(tileIdx.x + 1, tileIdx.y, tileIdx.z);
                break;
        }


        if (tileDics.ContainsKey(dirVector3))
        {
            var otherTile = tileDics[dirVector3];
            otherTile.OpenDoor(false);

            for (int idx = 0; idx < adjNodes.Count; ++idx)
            {
                if (adjNodes[idx].tileIdx == dirVector3)
                {
                    adjNodes.RemoveAt(idx);
                    break;
                }
            }
            for (int idx = 0; idx < otherTile.adjNodes.Count; ++idx)
            {
                if (otherTile.adjNodes[idx].tileIdx == tileIdx)
                {
                    otherTile.adjNodes.RemoveAt(idx);
                    break;
                }
            }

            switch (dir)
            {
                case DirectionType.Top:
                    BattleMgr.Instance.tileMgr.CheckAdjTile(this, dirVector3, DirectionType.Top, DirectionType.Bot);
                    BattleMgr.Instance.tileMgr.CheckAdjTile(otherTile, tileIdx, DirectionType.Bot, DirectionType.Top);
                    break;
                case DirectionType.Bot:
                    BattleMgr.Instance.tileMgr.CheckAdjTile(this, dirVector3, DirectionType.Bot, DirectionType.Top);
                    BattleMgr.Instance.tileMgr.CheckAdjTile(otherTile, tileIdx, DirectionType.Top, DirectionType.Top);
                    break;
                case DirectionType.Left:
                    BattleMgr.Instance.tileMgr.CheckAdjTile(this, dirVector3, DirectionType.Left, DirectionType.Right);
                    BattleMgr.Instance.tileMgr.CheckAdjTile(otherTile, tileIdx, DirectionType.Right, DirectionType.Left);
                    break;
                case DirectionType.Right:
                    BattleMgr.Instance.tileMgr.CheckAdjTile(this, dirVector3, DirectionType.Right, DirectionType.Left);
                    BattleMgr.Instance.tileMgr.CheckAdjTile(otherTile, tileIdx, DirectionType.Left, DirectionType.Right);
                    break;
            }
        }
    }

    public void EnableDisplay(bool isEnabled)
    {
        var ren = tileObj.GetComponent<MeshRenderer>();
        ren.enabled = isEnabled;
    }
}