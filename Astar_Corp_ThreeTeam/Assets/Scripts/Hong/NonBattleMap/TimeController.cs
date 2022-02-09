using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private Image[] image;

    //public Sprite pause;
    //public Sprite unpause;
    //
    //public Sprite x1;
    //public Sprite x2;

    [HideInInspector]
    public bool isPause;
    //private bool isFastSpeed;

    void Start()
    {
        image = GetComponentsInChildren<Image>();
        //playerPos = GameObject.FindWithTag("Player").transform;
        //Debug.Log(playerPos.gameObject.name);
    }


    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0f;
    }

    public void Play()
    {
        isPause = false;
        Time.timeScale = 1f;
    }

    public void DoublePlay()
    {
        isPause = false;
        Time.timeScale = 2f;
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
