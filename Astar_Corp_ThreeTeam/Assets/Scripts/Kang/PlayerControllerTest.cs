using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerTest : MonoBehaviour
{
    private float startTime;
    private bool isGameover;

    void Start()
    {
        EventBusMgr.Subscribe(EventType.Move, Move);
        EventBusMgr.Subscribe(EventType.Gameover, Gameover);
    }

    void Update()
    {
        if (isGameover)
        {
            if (startTime + 3f < Time.time)
                EventBusMgr.Publish(EventType.Gameover);
        }
    }

    public void Move(params object[] paramArr)
    {
        var rigid = gameObject.GetComponent<Rigidbody>();
        var speed = paramArr[0];
        rigid.useGravity = false;
        rigid.velocity = Vector3.forward * (int)speed;
        Debug.Log(paramArr[1].ToString());
    }

    public void Gameover(object empty)
    {
        var rigid = GetComponent<Rigidbody>();

        rigid.velocity = Vector3.zero;
    }

    public void MoveType()
    {
        EventBusMgr.Publish(EventType.Move, new object[] { 3, "¾ßÈ£" });
        startTime = Time.time;
        isGameover = true;
    }
}