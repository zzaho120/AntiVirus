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

    public int accurRateBase;       // �⺻ ���߷�

    public int minDamage;           // �ּ� ������
    public int maxDamage;           // �ִ� ������
    public int critRate;            // ũ��Ƽ�� Ȯ��
    public int critDamage;          // ũ��Ƽ�� ������ (������ * ���)%

    public int bullet;              // źâ Ŭ����
    public int mpPerAp;             // 1 AP�� ���� MP

    public int firstShotAp;         // ��� ù�� �� �Ҹ� AP
    public int otherShotAp;         // ù�� ���� �Ҹ�Ǵ� AP
    public int loadAp;              // ���� �� �Ҹ�Ǵ� AP

    public int minRange;            // �ּ� ��Ÿ�
    public int maxRange;            // �ִ� ��Ÿ�
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
