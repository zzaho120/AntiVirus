using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    public InfectedCharTest squadUI;

    public float radiusZone1;
    public float radiusZone2;
    public float radiusZone3;

    public bool isActiveZone2;
    public bool isAvtiveZone3;

    public string virusType;

    GameObject player;
    PlayerController playerController;
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 들어왔습니다.");
            Debug.Log($"바이러스 종류 : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            if (playerController.isMove)
            {
                var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
                //Debug.Log($"distance : {distance}");

                if (distance < radiusZone3 && isAvtiveZone3)
                    Debug.Log("플레이어가 구역3에 들어왔습니다.");
                else if (distance < radiusZone2 && isActiveZone2)
                    Debug.Log("플레이어가 구역2에 들어왔습니다.");
                else if(distance < radiusZone1)
                    Debug.Log("플레이어가 구역1에 들어왔습니다.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("플레이어가 나갔습니다.");
            player = null;
            playerController = null;
        }
    }
}
