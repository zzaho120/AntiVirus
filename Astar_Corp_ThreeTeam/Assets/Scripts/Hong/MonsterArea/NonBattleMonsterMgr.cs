using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMonsterMgr : MonoBehaviour
{
    public LaboratoryInfo labInfo;
    private List<MonsterStats> monsterStatList;
    private List<WorldMonsterData> monsterDataList;

    void Start()
    {

    }

    private void Update()
    {
        // ½ÇÇè¿ë
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            //if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            //{
            //}
        }
    }
}
