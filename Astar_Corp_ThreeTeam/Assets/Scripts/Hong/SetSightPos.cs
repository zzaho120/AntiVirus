using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSightPos : MonoBehaviour
{
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = Camera.main.ScreenToViewportPoint(transform.position);

        Vector3 temp = (camera.transform.position - transform.position).normalized;
    }

    private Vector3 GetSightPosition()
    {
        Vector3 pos = Vector3.zero;

        return pos;
    }
}
