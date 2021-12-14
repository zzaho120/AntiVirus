using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandBase
{
    public abstract void Execute();
    public abstract void Undo();
    public abstract void Redo();
}