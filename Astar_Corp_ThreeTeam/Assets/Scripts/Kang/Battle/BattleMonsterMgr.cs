using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterMgr : MonoBehaviour
{
    public List<MonsterChar> monsters;
    // Start is called before the first frame update
    public void Init()
    {
        monsters.Clear();
        var monsterArr = transform.GetComponentsInChildren<MonsterChar>();
        foreach (var monster in monsterArr)
        {
            monster.Init();
            monsters.Add(monster);
        }
    }
}
