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
        ren.enabled = isEnabled;
    }
}