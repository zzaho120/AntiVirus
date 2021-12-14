using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : CommandBase
{
    private GameObject obj;
    private Vector3 originPos;
    private Vector3 jumpVector = Vector3.up * 3f;
    public JumpCommand(GameObject obj)
    {
        this.obj = obj;
        originPos = obj.transform.position;
    }
    public override void Execute()
    {
        obj.transform.position = originPos + jumpVector;
    }

    public override void Redo()
    {
        Execute();
    }

    public override void Undo()
    {
        obj.transform.position = originPos - jumpVector;
    }
}

public class MoveForwardCommand : CommandBase
{
    private GameObject obj;
    private Vector3 originPos;
    private Vector3 moveVector = Vector3.forward * 3f;
    public MoveForwardCommand(GameObject obj)
    {
        this.obj = obj;
        originPos = obj.transform.position;
    }
    public override void Execute()
    {
        obj.transform.position = originPos + moveVector;
    }

    public override void Redo()
    {
        Execute();
    }

    public override void Undo()
    {
        obj.transform.position = originPos - moveVector;
    }
}