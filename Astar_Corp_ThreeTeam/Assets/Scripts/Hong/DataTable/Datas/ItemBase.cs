using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    // ���� �ִ� ����
    public string id;
    public new string name;

    // �߰� ����
    public int grade;
    public int price;
    public int weight;
    public int value;
    public float dropRate;
    public int itemQuantity;
}

