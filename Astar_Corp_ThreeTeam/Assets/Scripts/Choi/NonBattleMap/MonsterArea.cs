using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArea : MonoBehaviour
{
    public bool isBattle;

    int[] randomTime;
    int AppearanceTime;

    float appearanceTimer;
    float timer;

    private void Start()
    {
        isBattle = false;

        appearanceTimer = 0;
        timer = 0;

        randomTime = new int[3];
        randomTime[0] = 5;
        randomTime[1] = 10;
        randomTime[2] = 15;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int randomNum = Random.Range(0, 3);
            AppearanceTime = randomTime[randomNum];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer+=Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                appearanceTimer++;
            }

            if (appearanceTimer > AppearanceTime && !isBattle)
            {
                appearanceTimer = 0;
                isBattle = true;
                Debug.Log("±Þ½À!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer = 0;
            appearanceTimer = 0;
            isBattle = false;
        }
    }
}
