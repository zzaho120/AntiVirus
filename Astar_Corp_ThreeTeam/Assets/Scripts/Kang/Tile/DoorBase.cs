using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour
{
    public GameObject door;
    public bool isOpenDoor;
    public DirectionType type;

    private Renderer ren;
    private void Start()
    {
        ren = door.GetComponent<Renderer>();
    }

    public void OnOpenDoor()
    {
        isOpenDoor = !isOpenDoor;
        if (isOpenDoor)
            ren.enabled = false;
        else
            ren.enabled = true;
    }
}