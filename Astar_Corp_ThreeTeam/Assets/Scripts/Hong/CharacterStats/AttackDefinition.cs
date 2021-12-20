using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStats.asset", menuName = "Stats/Attack")]
public class AttackDefinition : ScriptableObject
{
    public float minDamage;
    public float maxDamage;
    public float range;

    // ���� ������ ���߿� ���� �� ���� ���ȵ�
    public float criticalChance;
    public float criticalDamage;
    //public float coolDown;

    public AttackStats CreateAttack(CharacterStats player)
    {
        // ������ ����
        var damage = player.baseDamage;
        damage += Random.Range(minDamage, maxDamage);

        // ũ��Ƽ�� ��ġ ����
        var isCritical = Random.value < criticalChance;
        if (isCritical)
            damage *= criticalDamage;

        return new AttackStats((int)damage, isCritical);
    }

    public AttackStats CreateAttack(Character player)
    {
        // ������ ����
        var damage = player.damage;
        damage += Random.Range(minDamage, maxDamage);

        // ũ��Ƽ�� ��ġ ����
        var isCritical = Random.value < criticalChance;
        if (isCritical)
            damage *= criticalDamage;

        return new AttackStats((int)damage, isCritical);
    }
}