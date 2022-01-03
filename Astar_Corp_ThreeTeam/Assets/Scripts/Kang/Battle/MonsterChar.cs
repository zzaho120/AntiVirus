using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChar : BattleChar
{
    [Header("Character")]
    public MonsterStats monsterStats;

    public override void Init()
    {
        base.Init();
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/1"));
        monsterStats.Init();
    }

    public void GetDamage(int dmg)
    {
        var hp = monsterStats.currentHp;
        hp -= dmg;
        monsterStats.currentHp = Mathf.Clamp(hp, 0, hp);

        Debug.Log($"{monsterStats.gameObject.name}은 {dmg} 데미지를 입어 {monsterStats.currentHp}가 되었다.");

        if (monsterStats.currentHp == 0)
            Destroy(gameObject);
    }
}