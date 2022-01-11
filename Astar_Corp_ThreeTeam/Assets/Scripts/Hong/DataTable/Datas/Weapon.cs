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

    public int accur_Rate_Base;     // �⺻ ���߷�

    public int min_damage;          // �ּ� ������
    public int max_damage;          // �ִ� ������
    public int crit_Damage;         // ũ��Ƽ�� ������ (������ * ���)%

    public int bullet;              // źâ Ŭ����
    public int accur_Rate_Dec;      // ���߷� ����
    /// <summary>
    /// Mp �Ҹ�
    /// </summary>
    public int weight;              // �̵��� �Ҹ�Ǵ� MP ��

    public int firstShot_Ap;        // ���ο� ���, ���� ��� �� �Ҹ� AP
    public int alertShot_Ap;        // ��� �� ù ��� ���� �Ҹ�Ǵ� AP
    public int aimShot_Ap;          // ���ػ�� �� ù ��� ���� �Ҹ�Ǵ� AP
    public int load_Ap;             // ���� �� �Ҹ�Ǵ� AP

    public int range;               // ��Ÿ�
    public int overRange_Penalty;   // ��Ÿ� +nŸ�Ͽ� ���� ���߷� ����
    public int underRange_Penalty;  // ��Ÿ� -nŸ�Ͽ� ���� ���߷� ����

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
