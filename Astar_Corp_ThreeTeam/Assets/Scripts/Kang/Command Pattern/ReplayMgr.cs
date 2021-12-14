using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReplayMgr : MonoBehaviour
{
    private SortedList<float, CommandBase> recordedCmds =
           new SortedList<float, CommandBase>();

    private bool isReplaying;
    private float replayTime;
    private float recordingTime;

    public void Update()
    {
        replayTime += Time.deltaTime;

        if (isReplaying)
        {
            if (recordedCmds.Any())
            {
                if (recordedCmds.Keys[0] < replayTime)
                {
                    Debug.Log("Replay Time: " + replayTime);
                    Debug.Log("Replay Command: " +
                              recordedCmds.Values[0]);

                    recordedCmds.Values[0].Execute();
                    recordedCmds.RemoveAt(0);
                }
            }
            else
            {
                Debug.Log("Replay End");
                isReplaying = false;
            }
        }
        else
            recordingTime += Time.deltaTime;
    }

    public void Init()
    {
        recordedCmds.Clear();

        isReplaying = false;
        recordingTime = 0f;
        replayTime = 0f;
    }

    public void Add(CommandBase command)
    {
        recordedCmds.Add(recordingTime, command);

        Debug.Log("Recorded Time: " + recordingTime);
        Debug.Log("Recorded Command: " + command);
    }

    public void Replay()
    {
        isReplaying = true;
        replayTime = 0f;

        if (recordedCmds.Count <= 0)
            Debug.LogError("No commands to replay!");

        recordedCmds.Reverse();
        Debug.Log("Replay Start");
    }

    public void ReplayPause()
    {
        isReplaying = !isReplaying;
    }
}