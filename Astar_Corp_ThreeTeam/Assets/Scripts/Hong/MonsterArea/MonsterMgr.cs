using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMgr : MonoBehaviour
{
    public LaboratoryInfo labInfo;
    private List<MonsterStats> monsterStatList;

    void Start()
    {
        var monsterStats = GetComponentsInChildren<MonsterStats>();
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
