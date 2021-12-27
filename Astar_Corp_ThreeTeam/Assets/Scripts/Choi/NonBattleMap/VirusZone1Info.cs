using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZone1Info : MonoBehaviour
{
    public InfectedCharTest squadUI;
    public Vector3 laboratoryPos;
    public float virusZone2Radius;
    public string virusType;

    GameObject player;
    PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ���Խ��ϴ�.");
            Debug.Log($"���̷��� ���� : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();

            squadUI.TurnOnWarning(0);
            squadUI.TurnOnWarning(1);
            squadUI.TurnOnWarning(2);
            squadUI.TurnOnWarning(3);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            if (playerController.isMove)
            {
                var distance = Vector2.Distance(new Vector2(laboratoryPos.x, laboratoryPos.z), new Vector2(player.transform.position.x, player.transform.position.z));

                if (distance > virusZone2Radius)
                {
                    Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");

                    squadUI.TurnOnWarning(0);
                    squadUI.TurnOnWarning(1);
                    squadUI.TurnOnWarning(2);
                    squadUI.TurnOnWarning(3);
                }
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

            squadUI.TurnOffWarning();
        }
    }
}
