using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public string id;
    public string iconId;
    public string prefabId;

    public string type;
    public string kind;
    public new string name;

    public int accur_Rate_Base;
    //public float accur_Rate_Alert;

    public int min_damage;
    public int max_damage;
    public int crit_Damage;

    public int bullet;
    public int accur_Rate_Dec;
    /// <summary>
    /// Mp �Ҹ�
    /// </summary>
    public int weight;

    public int firstShot_Ap;
    public int alertShot_Ap;
    public int aimShot_Ap;
    public int load_Ap;

    public int range;
    public int overRange_Penalty;
    public int underRange_Penalty;

    //private Sprite iconSprite;

    //public Sprite IconSprite
    //{
    //    get { return iconSprite; }
    //}

    // Attack ��ġ ������
    //public AttackStats CreateAttack(Character player)
    //{
    //    // ������ ����
    //    // ���� - ���߿� ������ �����غ���
    //    var damage = player.damage;
    //    damage += Random.Range(damage - 10, damage + 10);
    //
    //    // ũ��Ƽ�� ��ġ ����
    //    var isCritical = Random.value < critRate;
    //    if (isCritical)
    //        damage *= critDamage;
    //
    //    return new AttackStats((int)damage, isCritical);
    //}
}
