using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour
{
    public GameObject doorObj;
    public bool isOpenDoor;
    public DirectionType type;


    public void OnOpenDoor()
    {
        isOpenDoor = !isOpenDoor;
    }

    public void EnableDisplay(bool isEnabled)
    {
        var ren = doorObj.GetComponent<MeshRenderer>();
        ren.enabled = isEnabled;
    }

    public void EnableDisplay()
    {
        var ren = doorObj.GetComponent<MeshRenderer>();
        ren.enabled = !isOpenDoor;
    }
}