using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItem : ScriptableObject
{
    public string id;
    public new string name;

    /// <summary>
    /// �� ���� �־ string������ ����. ���߿� int.Parse Ȥ�� float.Parse�ؼ� ������ּ���
    /// </summary>
    public string grade;
    public string price;
    public string weight;
    public string dropRate;     // �� float������ ��ߵ�
    public string itemQuantity;
}
