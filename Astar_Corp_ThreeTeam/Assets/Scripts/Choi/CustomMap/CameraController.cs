using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float scrollSpeed = 10f;

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position += scrollSpeed * new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        }
    }
}
