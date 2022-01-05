using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPlayerSight : MonoBehaviour
{
    public enum PrintType
    {
        Mesh,
        Sprite
    }

    public PrintType printType;

    private MeshRenderer meshRenderer;
    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        switch (printType)
        {
            case PrintType.Mesh:
                meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.enabled = false;
                break;
            case PrintType.Sprite:
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.enabled = false;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSight"))
        {
            switch (printType)
            {
                case PrintType.Mesh:
                    if (meshRenderer == null) other.GetComponent<MeshRenderer>().enabled = true;
                    meshRenderer.enabled = true;
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
            switch (printType)
            {
                case PrintType.Mesh:
                    if (meshRenderer == null) other.GetComponent<MeshRenderer>().enabled = true;
                    meshRenderer.enabled = true;
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
                    meshRenderer.enabled = false;
                    break;
                case PrintType.Sprite:
                    spriteRenderer.enabled = false;
                    break;
            }
        }
    }
}
