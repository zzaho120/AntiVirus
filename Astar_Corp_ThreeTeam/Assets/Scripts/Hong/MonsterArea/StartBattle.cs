using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    //private bool isBattle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("���� �߻�");

            // ���߿� ���� �����Ǹ�
            //if (!isBattle)
            //{
            //    Debug.Log("���� �߻�");
            //    isBattle = true;
            //}
            //
            //else
            //    return;
        }
    }

    // Stay �ÿ��� �����߻� ó�� ? 
}
