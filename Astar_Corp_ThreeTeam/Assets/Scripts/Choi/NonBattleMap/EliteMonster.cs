using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : MonoBehaviour
{
    MeshRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            if(render == null) render = GetComponent<MeshRenderer>();
            if (render.enabled == false)
                render.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            if (render == null) render = GetComponent<MeshRenderer>();
            if (render.enabled == true)
                render.enabled = false;
        }
    }
}
