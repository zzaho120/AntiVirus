using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : ScriptableObject
{
    public string id;
    public string iconId;
    public string prefabId;
    public string name;
    public string description;
    public string type;
    public int damage;
    public int critRate;
    public int critDamage;
    public int cost;
    public int weight;
    public int str;
    public int con;
    public int intellet;
    public int luck;
    //private Sprite iconSprite;

    //public Sprite IconSprite
    //{
    //    get { return iconSprite; }
    //}
}
