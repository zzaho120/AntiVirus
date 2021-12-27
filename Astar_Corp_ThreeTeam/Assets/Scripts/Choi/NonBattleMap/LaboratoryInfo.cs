using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    public InfectedCharTest squadUI;
    public float radiusZone2;
    public float radiusZone3;
    public string virusType;

    public bool isPlayerInZone1;
    public bool isPlayerInZone2;
    public bool isPlayerInZone3;

    GameObject player;
    PlayerController playerController;
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ���Խ��ϴ�.");
            Debug.Log($"���̷��� ���� : {virusType}");

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

                if (distance < radiusZone3)
                    Debug.Log("�÷��̾ ����3�� ���Խ��ϴ�.");
                else if (distance < radiusZone2)
                    Debug.Log("�÷��̾ ����2�� ���Խ��ϴ�.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("�÷��̾ �������ϴ�.");
            player = null;
            playerController = null;
        }
    }
}
