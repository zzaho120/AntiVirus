using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterMgr : MonoBehaviour
{
    public List<BattleMonsterBase> monsters;
    // Start is called before the first frame update
    public void Init()
    {
        monsters.Clear();
    }
}
