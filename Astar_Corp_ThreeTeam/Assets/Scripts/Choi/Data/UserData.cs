using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemCountPair
{
    public string itemId;
    public int count;

    public ItemCountPair(string id, int c)
    {
        itemId = id;
        count = c;
    }
}

public class UserData
{
    public int id;
    public string nickname;

    public List<string> characterList = new List<string>();
    //public List<int> levelList = new List<int>();

    public List<string> equippableList = new List<string>();
    public List<string> consumableNameList = new List<string>();
    public List<int> consumableNumList = new List<int>();
    //public List<ItemCountPair> consumableItemList = new List<ItemCountPair>();
}
