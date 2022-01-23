using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMonsterArea : MonoBehaviour
{
    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.enabled = true;
        }
    }
}
