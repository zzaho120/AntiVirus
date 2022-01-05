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
            Debug.Log("전투 발생");

            // 나중에 전투 구현되면
            //if (!isBattle)
            //{
            //    Debug.Log("전투 발생");
            //    isBattle = true;
            //}
            //
            //else
            //    return;
        }
    }

    // Stay 시에도 전투발생 처리 ? 
}
