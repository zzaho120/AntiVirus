using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRoad : MonoBehaviour
{
    public LayerMask layer;

    // Update is called once per frame
    void Update()
    {
        MyCollisions();
    }

    void MyCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(
            GetComponentInChildren<CapsuleCollider>().center + transform.parent.position,
            GetComponentInChildren<CapsuleCollider>(). / 2,
            Quaternion.identity,
            layer);

        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit : " + hitColliders[i].name);
            i++;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            GetComponent<BoxCollider>().center + transform.parent.position,
            GetComponent<BoxCollider>().size);
    }
}
