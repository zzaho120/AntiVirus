using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMonster : ScriptableObject
{
    public string id;
    public string prefabId;
    public new string name;

    public int speed;           // �̵��ӵ�
    public int suddenAtkRate;   // ��� Ȯ�� (2�ʿ� �ѹ� �˻�)
    public int sightRange;      // ���� �Ÿ�
    public int areaRange;       // ���� ũ�� (������)
    public int areaMaxNum;      // ���� �� �ִ� �� ��
    public int createTime;      // 1���� �� ��Ÿ��
    public int battleMinNum;    // �浹 �� ���� �ּ� ���� ��
    public int battleMaxNum;    // �浹 �� ���� �ִ� ���� ��
}
