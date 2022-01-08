using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNew : MonoBehaviour
{
    //private NonBattleMgr manager;
    public float speed;
    public bool isMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0) isMove = true;
        else isMove = false;

        x *= speed * Time.deltaTime;
        z *= speed * Time.deltaTime;

        transform.Translate(x, 0, z);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log($"{other.gameObject.name}");
    //    if (other.gameObject.name.Equals("Seoul"))
    //    {
    //        manager.currentMapType = MapType.Seoul;
    //        Debug.Log("I'm in Seoul!");
    //    }
    //    else if (other.gameObject.name.Equals("Suncheon"))
    //    {
    //        manager.currentMapType = MapType.Suncheon;
    //        Debug.Log("I'm in Suncheon!");
    //    }
    //    else if (other.gameObject.name.Equals("Daegu"))
    //    {
    //        manager.currentMapType = MapType.Daegu;
    //        Debug.Log("I'm in Daegu!");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    manager.currentMapType = MapType.None;
    //}
}
