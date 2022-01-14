using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnRoadSpeed : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;

    // 현재 NavMesh 속도
    public float startSpeed = 8;
    public float speedUp = 20f;

    private void OnTriggerStay(Collider other)
    {
        nonBattleMgr = NonBattleMgr.Instance;

        if (other.CompareTag("Player"))
        {
           // nonBattleMgr.playerController.agent.speed = speedUp;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // nonBattleMgr.playerController.agent.speed = startSpeed;
        }
    }
}
