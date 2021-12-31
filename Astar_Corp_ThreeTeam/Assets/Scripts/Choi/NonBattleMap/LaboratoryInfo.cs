using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    public bool isSpareLab;
    public InfectedCharTest squadUI;

    public float radiusZone1;
    public float radiusZone2;
    public float radiusZone3;

    public bool isActiveZone2;
    public bool isAvtiveZone3;

    public string virusType;
    int step;

    GameObject player;
    PlayerController playerController;
    VirusData virusData;
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 들어왔습니다.");
            Debug.Log($"바이러스 종류 : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            virusData = player.GetComponent<VirusData>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            if (playerController.isMove)
            {
                var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

                if (distance < radiusZone3 && isAvtiveZone3)
                {
                    if (step != 3)
                    {
                        virusData.None = false;
                        step = 3;
                        virusData.currentVirus[$"{virusType}"] = step;
                        virusData.Change();
                        Debug.Log("플레이어가 구역3에 들어왔습니다.");

                    }
                }
                else if (distance < radiusZone2 && isActiveZone2)
                {
                    if (step != 2)
                    {
                        virusData.None = false;
                        step = 2;
                        virusData.currentVirus[$"{virusType}"] = step;
                        virusData.Change();
                        Debug.Log("플레이어가 구역2에 들어왔습니다.");
                    }
                }
                else if (distance < radiusZone1)
                {
                    if (step != 1)
                    {
                        virusData.None = false;
                        step = 1;
                        virusData.currentVirus[$"{virusType}"] = step;
                        virusData.Change();
                        Debug.Log("플레이어가 구역1에 들어왔습니다.");
                    }
                }
                else
                {
                    if (step != 0)
                    {
                        virusData.None = true;
                        virusData.currentVirus[$"{virusType}"] = 0;
                    }
                }
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
