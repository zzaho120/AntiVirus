using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : ScriptableObject
{
    public string id;
    public string iconId;
    public string prefabsId;
    public string name;
    public string description;
    public string type;
    public int weight;
    public int cost;
    public int hp;
    public int mp;
    public int str;
    public float duration;
}
