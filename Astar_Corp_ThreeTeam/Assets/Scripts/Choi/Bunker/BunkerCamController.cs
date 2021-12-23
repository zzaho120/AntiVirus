using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerCamController : MonoBehaviour
{
    public MultiTouch multiTouch;
  
    float duration = 5; // This will be your time in seconds.
    float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.

    GameObject currentObject;
    Camera camera;

    public bool isZoomIn;
    IEnumerator coroutine;
    Vector3 positionToLook;
    Vector3 centerPos;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        centerPos = new Vector3(2.412f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (multiTouch.Tap == true)
        {
            if (isZoomIn)
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                    {
                        isZoomIn = false;
                        positionToLook = centerPos;

                        if (coroutine != null) StopCoroutine(coroutine);
                        coroutine = ZoomOut();
                        StartCoroutine(coroutine);
                    }
                }
            }

            else
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                    {
                        Invoke("CompleteZoomIn", 1f);
                        positionToLook = hit.collider.transform.position;

                        if (coroutine != null) StopCoroutine(coroutine);
                        coroutine = ZoomIn();
                        StartCoroutine(coroutine);
                    }
                }
            }
        }
    }

    public void CompleteZoomIn()
    {
        isZoomIn = true;
    }

    IEnumerator ZoomIn()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.

        float t = 0f;
        while (progress < 0.2)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 15f, progress);
            Quaternion lookOnLook = Quaternion.LookRotation(positionToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);

            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }

    IEnumerator ZoomOut()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 0.2)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60f, progress);
            Quaternion lookOnLook = Quaternion.LookRotation(positionToLook - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);
           
            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }
}
