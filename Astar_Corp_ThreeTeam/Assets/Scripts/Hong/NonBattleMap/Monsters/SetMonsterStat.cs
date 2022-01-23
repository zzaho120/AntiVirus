using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetMonsterStat : MonoBehaviour
{
    private ScriptableMgr scriptableMgr;
    private MonsterStats monsterStats;
    private string id, nonBattleId;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //
    //    var center = transform.position;
    //    var radius = transform.lossyScale.x;
    //    Gizmos.DrawSphere(center, radius);
    //}


    public void Init()
    {
        scriptableMgr = ScriptableMgr.Instance;
        monsterStats = GetComponent<MonsterStats>();

        GetMonster();
        SetMonster();
    }

    private void GetMonster()
    {
        // ID 설정
        if (gameObject.name == MonsterName.Wolf)
        {
            id = "MON_0001";
            nonBattleId = "NBM_0001";
        }
        else if (gameObject.name == MonsterName.Boar)
        {
            id = "MON_0002";
            nonBattleId = "NBM_0002";
        }
        else if (gameObject.name == MonsterName.Bear)
        {
            id = "MON_0003";
            nonBattleId = "NBM_0003";
        }
        else if (gameObject.name == MonsterName.Tiger)
        {
            id = "MON_0004";
            nonBattleId = "NBM_0004";
        }
        else if (gameObject.name == MonsterName.Jaguar)
        {
            id = "MON_0005";
            nonBattleId = "NBM_0005";
        }
        else if (gameObject.name == MonsterName.Fox)
        {
            id = "MON_0006";
            nonBattleId = "NBM_0006";
        }
        else if (gameObject.name == MonsterName.Spider)
        {
            id = "MON_0007";
            nonBattleId = "NBM_0007";
        }
        monsterStats.monster = scriptableMgr.GetMonster(id);
        monsterStats.nonBattleMonster = scriptableMgr.GetWorldMonster(nonBattleId);
    }

    private void SetMonster()
    {
        var center = transform.position;
        var radius = transform.lossyScale.x;
        var areaMask = LayerMask.GetMask("VirusZone");
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, areaMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<LaboratoryInfo>())
            {
                // 바이러스 타입 설정
                var labInfo = hitColliders[i].GetComponent<LaboratoryInfo>();
                monsterStats.virus = scriptableMgr.GetVirus(labInfo.virusType);

                //// Area2
                //if (labInfo.isActiveZone2 && !labInfo.isActiveZone3)
                //{
                //    monsterStats.virusLevel = Random.Range(1, 2 + 1);
                //}
                //// Area3
                //else if (labInfo.isActiveZone2 && labInfo.isActiveZone3)
                //{
                //    monsterStats.virusLevel = Random.Range(1, 3 + 1);
                //}
                //// Area1
                //else
                //{
                //    monsterStats.virusLevel = 1;
                //}
                ////break;
            }
        }
        if (hitColliders.Length >= 3)
        {
            monsterStats.virusLevel = 3;
        }
        else if (hitColliders.Length == 2)
        {
            monsterStats.virusLevel = 2;
        }
        else if (hitColliders.Length <= 1)
        {
            monsterStats.virusLevel = 1;
        }
        else
        {
            monsterStats.virusLevel = 1;
        }
    }
}
