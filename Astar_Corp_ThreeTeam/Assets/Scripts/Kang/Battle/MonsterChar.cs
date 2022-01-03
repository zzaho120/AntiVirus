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
        monsterStats.monster = (Monster)Instantiate(Resources.Load("Choi/Datas/Monsters/Prototype Test 1"));
        monsterStats.Init();
    }
}