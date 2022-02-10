using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnFog : MonoBehaviour
{
    private GameObject fog;
    private MeshRenderer myMesh;

    void Start()
    {
        fog = GameObject.Find("Fog");
        myMesh = GetComponent<MeshRenderer>();

        // 안개 켜져있을때
        if (fog.GetComponent<MeshRenderer>().enabled)
            myMesh.enabled = false;
        // 안개 꺼져있을때
        else
            myMesh.enabled = true;
    }
}
