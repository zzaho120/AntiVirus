using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataConsumableItem : DataItem
{
    public int count;

    public ItemTableElem tableElem;

    public ItemTableElem ItemTableElem
    {
        get { return tableElem; }
    }
}

