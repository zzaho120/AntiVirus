using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItem : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite img;

    /// <summary>
    /// �� ���� �־ string������ ����. ���߿� int.Parse Ȥ�� float.Parse�ؼ� ������ּ���
    /// </summary>
    public string grade;
    public string price;
    public string weight;
    public string dropRate;     // �� float������ ��ߵ�
    public string itemQuantity;
    public string storeName;
    public string des; // ����.
}
