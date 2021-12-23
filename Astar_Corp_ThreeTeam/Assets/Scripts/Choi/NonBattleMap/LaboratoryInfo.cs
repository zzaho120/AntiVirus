using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    public float radiusZone1;
    public float radiusZone2;
    public float radiusZone3;

    GameObject player;
    PlayerController playerController;
    // Start is called before the first frame update
    //void Start()
    //{
    //    var virusZoon3 = transform.GetChild(1).gameObject;
    //    var virusZoon2 = transform.GetChild(2).gameObject;
    //    var virusZoon1 = transform.GetChild(3).gameObject;

    //    radiusZone3 = virusZoon3.GetComponent<SphereCollider>().radius;
    //    radiusZone2 = virusZoon2.GetComponent<SphereCollider>().radius;
    //    radiusZone1 = virusZoon1.GetComponent<SphereCollider>().radius;

    //    radiusZone3 *= virusZoon3.transform.localScale.x;
    //    radiusZone2 *= virusZoon2.transform.localScale.x;
    //    radiusZone1 *= virusZoon1.transform.localScale.x;

    //    Debug.Log($"radiusZone3 : {radiusZone3}");
    //    Debug.Log($"radiusZone2 : {radiusZone2}");
    //    Debug.Log($"radiusZone1 : {radiusZone1}");

    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾ ���Խ��ϴ�.");
            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();

            var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
            
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
                else if (distance < radiusZone1)
                    Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");
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
