using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamController : MonoBehaviour
{
    public MultiTouch multiTouch;

    Camera camera;
    float duration = 5; // This will be your time in seconds.
    float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
    float currentSize = 5;
    bool isZoomIn;

    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (multiTouch.ZoomInOut > 0f)
        {
            //ZoomIn.
            isZoomIn = true;
            Debug.Log("ZoomIn");

            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = ZoomIn();
            StartCoroutine(coroutine);
        }
        else if (multiTouch.ZoomInOut < 0f)
        {
            //ZoomOut.
            isZoomIn = false;
            Debug.Log("ZoomOut");

            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = ZoomOut();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator ZoomIn()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 13, progress);
            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }

    IEnumerator ZoomOut()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 40, progress);
            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }
}
