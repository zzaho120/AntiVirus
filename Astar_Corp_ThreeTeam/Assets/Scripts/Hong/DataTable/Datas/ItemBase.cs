using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    // 원래 있던 정보
    public string id;
    public new string name;

    // 추가 정보
    public int grade;
    public int price;
    public int weight;
    public int value;
    public float dropRate;
    public int itemQuantity;
}

