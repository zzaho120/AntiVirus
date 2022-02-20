using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [HideInInspector]
    public bool isPause;

    [SerializeField]
    private Image[] image;

    public GameObject player;

    // 0 Play
    // 1 Pause
    // 2 Double Play

    private void Start()
    {
        image[0].color = Color.yellow;
    }

    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0f;

        image[0].color = Color.white;
        image[1].color = Color.yellow;
        image[2].color = Color.white;
    }

    public void Play()
    {
        //isPause = false;
        StartCoroutine(SetPause());
        Time.timeScale = 1f;

        image[0].color = Color.yellow;
        image[1].color = Color.white;
        image[2].color = Color.white;
    }

    public void DoublePlay()
    {
        isPause = false;
        Time.timeScale = 2f;

        image[0].color = Color.white;
        image[1].color = Color.white;
        image[2].color = Color.yellow;
    }

    private IEnumerator SetPause()
    {
        yield return new WaitForSeconds(0.5f);
        isPause = false;
    }

    //public void PauseTime()
    //{
    //    // pause
    //    if (isPause == false)
    //    {
    //        isPause = true;
    //        Time.timeScale = 0f;
    //        image[0].sprite = unpause;
    //    }
    //    // un-pause
    //    else
    //    {
    //        //playerPos.position = newPos.position;
    //        isPause = false;
    //        Time.timeScale = 1f;
    //        image[0].sprite = pause;
    //    }
    //}
    //
    //public void DoubleSpeedTime()
    //{
    //    if (isPause == false)
    //    {
    //        // 2배속
    //        if (isFastSpeed == false)
    //        {
    //            isFastSpeed = true;
    //            Time.timeScale = 2f;
    //            image[1].sprite = x2;
    //        }
    //        // 1배속
    //        else if (isFastSpeed == true)
    //        {
    //            isFastSpeed = false;
    //            Time.timeScale = 1f;
    //            image[1].sprite = x1;
    //        }
    //    }
    //}
}
