using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSightPos : MonoBehaviour
{
    private new Camera camera;
    private GameObject player;
    GameObject cube;
    GameObject playerSights;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        player = GameObject.Find("Player");
        playerSights = GameObject.Find("PlayerSights");

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 point = camera.transform.worldToLocalMatrix.MultiplyPoint();

        int layer = LayerMask.GetMask("Player");

        RaycastHit hitInfo;
        if (Physics.Raycast(camera.transform.localPosition, player.transform.position, out hitInfo, layer))
        {
            Debug.Log("Hit");

            //transform.position = hitInfo.transform.position;
            
            //cube.transform.localScale = new Vector3(3f, 3f, 3f);
            //cube.transform.position = hitInfo.transform.position;

        }
    }
}
