using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerTest : MonoBehaviour
{
    private float startTime;
    private bool isGameover;
    private CommandMgr cmdMgr;

    void Start()
    {
        EventBusMgr.Subscribe(EventType.Move, Move);
        EventBusMgr.Subscribe(EventType.Gameover, Gameover);
        cmdMgr = GameMgr.Instance.commandMgr;
    }

    void Update()
    {
        if (isGameover)
        {
            if (startTime + 3f < Time.time)
                EventBusMgr.Publish(EventType.Gameover);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var cmd = new JumpCommand(gameObject);
            cmdMgr.ExecuteCommand(cmd);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var cmd = new MoveForwardCommand(gameObject);
            cmdMgr.ExecuteCommand(cmd);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            cmdMgr.UndoCommand();

        if (Input.GetKeyDown(KeyCode.Alpha0))
            cmdMgr.RedoCommand();
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