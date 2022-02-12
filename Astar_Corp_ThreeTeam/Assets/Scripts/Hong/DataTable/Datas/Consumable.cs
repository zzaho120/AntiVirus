using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : ItemBase
{
    //public string id;
    public string iconId;
    public string prefabsId;
    //public string name;
    public Sprite img;

    public string description;
    public string type;
    
    public int ap;
    public int hpRecovery;
    public int virusGaugeDec;
    public string storeName;
}
