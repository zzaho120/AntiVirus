using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerController : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight")) // && !playerController.isBunkerClikable)
        {
            playerController.isBunkerClikable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight")) // && playerController.isBunkerClikable)
        {
            playerController.isBunkerClikable = false;
        }
    }
}
