using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSightPos : MonoBehaviour
{
    GameObject player;
    GameObject playerSights;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(Camera.main.transform.localPosition, player.transform.localPosition);
    //}

    void Start()
    {
        player = GameObject.Find("Player");
        playerSights = GameObject.Find("PlayerSights");
    }

    void LateUpdate()
    {
        int layer = LayerMask.GetMask("Mask");

        // ∏Ò«• ∫§≈Õ - Ω√¿€ ∫§≈Õ
      //  var dir = (playerSights.transform.localPosition - Camera.main.transform.position);//.normalized;
        var dir = (player.transform.position - Camera.main.transform.position);//.normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, dir, out hitInfo, 1000f, layer))
        {
            //Debug.Log(hitInfo.collider.name);
            transform.position = hitInfo.point;
        }
    }
}
