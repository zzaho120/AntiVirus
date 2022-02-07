using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    [HideInInspector]
    public bool isPlayerIn;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hi");
        Debug.Log(collision.collider.tag);

        if (collision.gameObject.CompareTag("VirusZonePhase3") || collision.gameObject.CompareTag("VirusZonePhase2") || collision.gameObject.CompareTag("VirusZonePhase1"))
        {
            Debug.Log("Player In");
            isPlayerIn = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("VirusZonePhase3") || collision.gameObject.CompareTag("VirusZonePhase2") || collision.gameObject.CompareTag("VirusZonePhase1"))
        {
            Debug.Log("Player Out");
            isPlayerIn = false;
        }
    }
}
