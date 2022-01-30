using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterStats))]
public class WorldMonsterChar : MonoBehaviour
{
    public MonsterStats monsterStat;
    public WorldMonsterFSM fsm;

    //public void Init()
    public void OnEnable()
    {
        monsterStat = GetComponent<MonsterStats>();
        StartCoroutine(GetFSM());
    }   

    public void MonsterUpdate()
    {
        if (fsm != null)
            fsm.Update();
    }

    private IEnumerator GetFSM()
    {
        yield return new WaitForSeconds(1f);

        fsm = new WorldMonsterFSM();
        fsm.Init(this);
    }
}
