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

        // �Ȱ� ����������
        if (fog.GetComponent<MeshRenderer>().enabled)
            myMesh.enabled = false;
        // �Ȱ� ����������
        else
            myMesh.enabled = true;
    }
}
