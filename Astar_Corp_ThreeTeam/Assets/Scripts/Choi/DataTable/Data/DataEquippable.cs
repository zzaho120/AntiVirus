using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEquippable : DataItem
{
    public int id;

    public EquippableTableElem tableElem;

    public DataCharacter owner;

    public void SetOwener(DataCharacter newOwner)
    {
        owner = newOwner;
    }

    public EquippableTableElem ItemTableElem
    {
        get
        {
            return itemTableElem as EquippableTableElem;
        }
    }
}
