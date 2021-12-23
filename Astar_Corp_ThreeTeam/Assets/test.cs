using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public MultiTouch multiTouch;
    public MeshRenderer ren;
    public Camera cam;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public TextMeshProUGUI text7;
    private float maxZoom = 60f;
    private float minZoom = 100f;
    private void Update()
    {
        SlideTest();
        ZoomTest();
        RotateTest();


        text4.text = multiTouch.curTouchPos.ToString();
        text5.text = multiTouch.curSecTouchPos.ToString();
        text6.text = multiTouch.primaryDeltaPos.ToString();
        text7.text = multiTouch.secondDeltaPos.ToString();
    }

    private void SlideTest()
    {
        if (multiTouch.SwipeDirection.x < 0)
            text.text = "left";
        else if (multiTouch.SwipeDirection.x > 0)
            text.text = "right";
        else if (multiTouch.SwipeDirection.y < 0)
            text.text = "down";
        else if (multiTouch.SwipeDirection.y > 0)
            text.text = "up";


    }

    private void ZoomTest()
    {
        if (multiTouch.ZoomInOut < 0f)
        {
            text2.text = $"ZoomOut {multiTouch.ZoomInOut}";
        }
        else if (multiTouch.ZoomInOut > 0f)
        {
            text2.text = $"ZoomIn {multiTouch.ZoomInOut}";
        }

        if (multiTouch.ZoomInOut != 0f)
        {
            Camera.main.transform.position += new Vector3(0f, 0f, multiTouch.ZoomInOut);
        }
    }

    private void RotateTest()
    {
        if (multiTouch.RotateAngle != 0f)
        {
            transform.Rotate(0f, 0f, multiTouch.RotateAngle);
        }

        if (multiTouch.RotateAngle < 0f)
        {
            text3.text = $"rotate left {multiTouch.RotateAngle}";
        }
        else if (multiTouch.RotateAngle > 0f)
        {
            text3.text = $"rotate right {multiTouch.RotateAngle}";
        }
    }
}
