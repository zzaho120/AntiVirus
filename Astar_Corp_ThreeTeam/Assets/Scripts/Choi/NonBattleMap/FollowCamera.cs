using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
 
    public Vector3 pos; // 위치
    public Vector3 rot; // 회전

    Vector3 targetPos;
    public Vector3 additionalPos;

    public bool isWorldMode;

    private void Start()
    {
        targetPos = transform.position ; //transform.position + additionalPos;
    }

    private void LateUpdate()
    {
        if (!isWorldMode)
        {
            transform.position = target.position + pos;
            //transform.position = Vector3.Lerp(transform.position, target.position + pos, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Euler(rot);
            //transform.LookAt(target);
        }
        else
        {
            targetPos = target.position + additionalPos;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3f);
        }
        
    }

    public void SwitchCam()
    {
        if (!isWorldMode)
            isWorldMode = true;
        else
            isWorldMode = false;
    }

    //public Transform target;
    //public float dist = 10.0f;
    //public float height = 25.0f;
    //public float smoothRotate = 5.0f;
    //
    //private Transform player;
    //private CameraDrag cm;
    //
    //void Start()
    //{
    //    cm = GetComponent<CameraDrag>();
    //    player = GetComponent<Transform>();
    //}
    //
    //void LateUpdate()
    //{
    //    if (!cm.isWorldMapMode)
    //    {
    //        float currYAngle = Mathf.LerpAngle(player.eulerAngles.y,
    //            target.eulerAngles.y, smoothRotate * Time.deltaTime);
    //        
    //        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);
    //        
    //        player.position = target.position - (rot * Vector3.forward * dist) +
    //            (Vector3.up * height);
    //        
    //        player.LookAt(target);
    //    }
    //}
}
