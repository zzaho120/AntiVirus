using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBase : MonoBehaviour
{
    public TileBase parent;
    public string virusName;
    public int virusLevel;
    public int increasePerTic;

    public void Init(TileBase tile, string name, int level, int increase)
    {
        parent = tile;
        virusName = name;
        virusLevel = level;
        increase = increasePerTic;
    }
}
