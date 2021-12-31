using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "AttackStats.asset", menuName = "Stats/Attack")]
public class AttackDefinition : ScriptableObject
{
    public int minDamage;
    public int maxDamage;
    public float range;

    // 아직 없지만 나중에 쓰일 것 같은 스탯들
    public float criticalChance;
    public int criticalDamage;
    //public float coolDown;


    //public AttackStats CreateAttack(Character player)
    //{
    //    // 데미지 조정
    //    var damage = player.min_Hp_Rise;
    //    damage += Random.Range(minDamage, maxDamage);
    //
    //    // 크리티컬 수치 조정
    //    var isCritical = Random.value < criticalChance;
    //    if (isCritical)
    //        damage *= criticalDamage;
    //
    //    return new AttackStats((int)damage, isCritical);
    //}
}
