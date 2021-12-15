using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [Header("Value")]
    public Vector3 tileIdx;

    [Header("List")]
    public List<TileBase> adjNodes;
    public List<GameObject> objList;

    public void Init()
    {
    }
}