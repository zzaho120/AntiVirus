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

    //is Zone SetActive?
    public bool isActiveZone2;
    public bool isActiveZone3;

    public string virusType;
    string[] virusTypes = { "E", "B", "P", "I", "T" };
    int step;

    GameObject player;
    PlayerController playerController;
    VirusData virusData;
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ���Խ��ϴ�.");
            Debug.Log($"���̷��� ���� : {virusType}");

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

                if (distance < radiusZone3 && isActiveZone3)
                {
                    if (step != 3)
                    {
                        virusData.None = false;
                        step = 3;
                        virusData.currentVirus[$"{virusType}"] = step;
                        virusData.Change();
                        Debug.Log("�÷��̾ ����3�� ���Խ��ϴ�.");

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
                        Debug.Log("�÷��̾ ����2�� ���Խ��ϴ�.");
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
                        Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");
                    }
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

            virusData.currentVirus[$"{virusType}"] = 0;
            step = 0;
            virusData.Change();

            bool isInZone = false;
            foreach (var element in virusTypes)
            {
                if (virusData.currentVirus[element] != 0)
                {
                    isInZone = true;
                    break;
                }
            }

            if (isInZone == false)
            {
                virusData.None = true;
                virusData.Init();
            }
        }
    }
}
