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
    public List<Material> materials;
    public bool isWall;
    public int accuracy;
    public int moveAP;

    [Header("List")]
    public List<TileBase> adjNodes;

    [Header("RunTime")]
    public GameObject charObj;
    public TileBase wallTile;

    public TileBase(TileBase copy)
    {
        tileIdx = copy.tileIdx;
        tileObj = copy.tileObj;
        materials = copy.materials;
        isWall = copy.isWall;
    }
    public void Init(TileMgr tileMgr)
    {
        mgr = tileMgr;

        InitIdx();
    }

    private void InitIdx()
    {
        tileIdx = transform.position;
        gameObject.name = $"Tile ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
    }

    public void EnableDisplay(bool isEnabled)
    {
        var ren = tileObj.GetComponent<MeshRenderer>();

        if (isEnabled)
        {
            ren.material = materials[0];
            ren.material.color = Color.white;
        }
        else
        {
            ren.material = materials[1];
            ren.material.color = Color.white;
        }

        if (charObj != null)
            charObj.GetComponent<MeshRenderer>().enabled = isEnabled;
    }

    public void SetHighlight()
    {
        if (charObj != null && charObj.tag == "BattleMonster")
        {
            var ren = tileObj.GetComponent<MeshRenderer>();

            ren.material.color = Color.red;
        }
    }
}