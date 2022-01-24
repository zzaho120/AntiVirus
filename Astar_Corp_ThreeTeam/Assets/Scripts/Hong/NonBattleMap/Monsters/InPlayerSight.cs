using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPlayerSight : MonoBehaviour
{
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.collider.name);
    //}

    public enum PrintType
    {
        Mesh,
        Sprite
    }
    
    public PrintType printType;
    
    //private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer[] meshRenderers;
    private SpriteRenderer spriteRenderer;
    
    void OnEnable()
    {
        switch (printType)
        {
            case PrintType.Mesh:
                //meshRenderer = GetComponentInChildren<MeshRenderer>();
                //meshRenderer.enabled = false;
                meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var renderer in meshRenderers)
                {
                    //renderer.enabled = false;
                }
                break;
            case PrintType.Sprite:
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.enabled = false;
                break;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
    
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            //Debug.Log("In Sight");
    
            switch (printType)
            {
                case PrintType.Mesh:
                    meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var renderer in meshRenderers)
                    {
                        renderer.enabled = true;
                        //Debug.Log("Renderer Enabled");
                    }
                    //if (meshRenderer == null) other.GetComponent<MeshRenderer>().enabled = true;
                    //meshRenderer.enabled = true;
                    break;
                case PrintType.Sprite:
                    spriteRenderer.enabled = true;
                    break;
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            //Debug.Log("In Sight");
    
            switch (printType)
            {
                case PrintType.Mesh:
                    meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var renderer in meshRenderers)
                    {
                        renderer.enabled = true;
                        //Debug.Log("Renderer Enabled");
                    }
                    //if (meshRenderer == null) other.GetComponent<MeshRenderer>().enabled = true;
                    //meshRenderer.enabled = true;
                    break;
                case PrintType.Sprite:
                    spriteRenderer.enabled = true;
                    break;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            switch (printType)
            {
                case PrintType.Mesh:
                    foreach (var renderer in meshRenderers)
                    {
                        renderer.enabled = false;
                    }
                    //meshRenderer.enabled = false;
                    break;
                case PrintType.Sprite:
                    spriteRenderer.enabled = false;
                    break;
            }
        }
    }
}
