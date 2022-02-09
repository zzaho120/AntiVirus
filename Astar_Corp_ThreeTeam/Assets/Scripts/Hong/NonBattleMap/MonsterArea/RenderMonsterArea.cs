using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMonsterArea : MonoBehaviour
{
    private new MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (renderer != null)
                renderer.enabled = true;
        }
    }
}
