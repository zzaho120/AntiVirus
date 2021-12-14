using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMgr
{
    // Invoker + undo + redo
    private Stack<CommandBase> undoList = new Stack<CommandBase>();
    private Stack<CommandBase> redoList = new Stack<CommandBase>();

    public void Init()
    {
        undoList.Clear();
        redoList.Clear();
    }

    public void ExecuteCommand(CommandBase command)
    {
        command.Execute();

        undoList.Push(command);
        redoList.Clear();
    }

    public void UndoCommand()
    {
        if (undoList.Count > 0)
        {
            var undoCmd = undoList.Pop();
            undoCmd.Undo();
            redoList.Push(undoCmd);
            Debug.Log($"RedoList Count : {undoList.Count}");
        }
        else
        {
            Debug.LogWarning("undo ����Ʈ�� Ŀ�ǵ尡 �����ϴ�.");
        }
    }

    public void RedoCommand()
    {
        if (redoList.Count > 0)
        {
            var redoCmd = redoList.Pop();
            redoCmd.Redo();
            undoList.Push(redoCmd);
            Debug.Log($"RedoList Count : {redoList.Count}");
        }
        else
        {
            Debug.LogWarning("redo ����Ʈ�� Ŀ�ǵ尡 �����ϴ�.");
        }
    }
}