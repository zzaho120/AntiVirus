using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterStats))]
public class WorldMonsterChar : MonoBehaviour
{
    public MonsterStats monsterStat;
    public WorldMonsterFSM fsm;
    public Sprite monsterImg;

    //여기서 몹영역 크기 설정해줘야 될듯
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
