using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;

    private Transform tr;

    private CameraMovement cm;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CameraMovement>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!cm.isWorldMapMode)
        {
            float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y,
                target.eulerAngles.y, smoothRotate * Time.deltaTime);

            Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

            tr.position = target.position - (rot * Vector3.forward * dist) +
                (Vector3.up * height);

            tr.LookAt(target);
        }
    }
}
