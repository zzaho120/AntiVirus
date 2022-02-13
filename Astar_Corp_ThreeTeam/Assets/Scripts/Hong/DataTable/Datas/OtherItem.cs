using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItem : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite img;

    /// <summary>
    /// 빈 값이 있어서 string형으로 썼음. 나중에 int.Parse 혹은 float.Parse해서 사용해주세요
    /// </summary>
    public string grade;
    public string price;
    public string weight;
    public string dropRate;     // 얘 float형으로 써야됨
    public string itemQuantity;
    public string storeName;
    public string des; // 설명.
}
