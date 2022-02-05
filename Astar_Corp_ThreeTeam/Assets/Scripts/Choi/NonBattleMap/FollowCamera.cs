using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //public Transform target;
    //public float delayTime;
    //
    //public Vector3 offset;
    //private CameraDrag cm;
    //
    //private void Start()
    //{
    //    cm = GetComponent<CameraDrag>();
    //}
    //
    //private void LateUpdate()
    //{
    //    if (!cm.isWorldMapMode)
    //    {
    //        Vector3 fixedPos = new Vector3(
    //            target.transform.position.x + offset.x,
    //            target.transform.position.y + offset.y,
    //            target.transform.position.z + offset.z);
    //        transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * delayTime);
    //    }
    //}

    public Transform target;
    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;
    
    private Transform player;
    private CameraDrag cm;
    
    void Start()
    {
        cm = GetComponent<CameraDrag>();
        player = GetComponent<Transform>();
    }
    
    void LateUpdate()
    {
        if (!cm.isWorldMapMode)
        {
            float currYAngle = Mathf.LerpAngle(player.eulerAngles.y,
                target.eulerAngles.y, smoothRotate * Time.deltaTime);
            
            Quaternion rot = Quaternion.Euler(0, currYAngle, 0);
            
            player.position = target.position - (rot * Vector3.forward * dist) +
                (Vector3.up * height);
            
            player.LookAt(target);
        }
    }
}
