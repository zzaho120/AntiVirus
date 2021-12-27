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

    float timer;
    float decreaseTimer; 
    float decreaseTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 들어왔습니다.");
            Debug.Log($"바이러스 종류 : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();

            squadUI.TurnOnWarning(0);
            squadUI.TurnOnWarning(1);
            squadUI.TurnOnWarning(2);
            squadUI.TurnOnWarning(3);
        }

        timer = 0f;
        decreaseTime = 5f;
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
                    Debug.Log("플레이어가 구역1에 들어왔습니다.");

                    squadUI.TurnOnWarning(0);
                    squadUI.TurnOnWarning(1);
                    squadUI.TurnOnWarning(2);
                    squadUI.TurnOnWarning(3);
                }
            }
            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0f;
                decreaseTimer++;
            }
            if (decreaseTimer >= decreaseTime)
            {
                decreaseTimer = 0f;
                if (playerController.character.stemina > 1) playerController.DecreaseStemia(1);
                else if (playerController.character.hp > 1) playerController.DecreaseHp(1);
                else Debug.Log("Die");
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

            squadUI.TurnOffWarning();

            timer = 0f;
            decreaseTimer = 0f;
        }
    }
}
