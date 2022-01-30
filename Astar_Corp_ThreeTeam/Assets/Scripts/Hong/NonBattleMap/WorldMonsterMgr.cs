using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMonsterMgr : MonoBehaviour
{
    public List<WorldMonsterChar> monsters;

    //public void Init()
    //{
    //    foreach (var monster in monsters)
    //    {
    //        monster.Init();
    //    }
    //}   
    
    public void MonsterUpdate()
    {
        foreach (var monster in monsters)
        {
            monster.MonsterUpdate();
        }
    }

    public void ReturnMonster(WorldMonsterChar monster)
    {
        monsters.Remove(monster);
        var returnToPool = monster.GetComponent<ReturnToPool>();
        returnToPool.Return();
    }

    public void AddMonster(WorldMonsterChar monster)
    {
        monsters.Add(monster);
    }

}
