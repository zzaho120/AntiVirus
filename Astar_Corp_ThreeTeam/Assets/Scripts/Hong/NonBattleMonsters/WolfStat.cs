using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfStat : MonoBehaviour
{
    private ScriptableMgr scriptableMgr;
    private WorldMonster wolfStat;
    private string id;

    private void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        id = "NBM_0001";
        wolfStat = scriptableMgr.worldMonsterList[id];
    }

    public void OnTestButtonClick()
    {
        Debug.Log(wolfStat);
    }
}
