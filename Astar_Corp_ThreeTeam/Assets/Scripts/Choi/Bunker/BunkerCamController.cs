using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum Mode
{ 
    None,
    Touch,
    Mouse
}

public class BunkerCamController : MonoBehaviour
{
    public BunkerMgr bunkerMgr;
    public MultiTouch multiTouch;
    public Vector3 centerPos;

    float duration = 2; // This will be your time in seconds.
    float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.

    public GameObject currentObject;
    Camera camera;

    public bool isZoomIn;
    IEnumerator coroutine;
    Vector3 positionToLook;
    
    public bool isCurrentEmpty;

    public Action OpenWindow;
    public Action CloseWindow;

    public Mode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentMode = Mode.None;
        isCurrentEmpty = true;

        centerPos = new Vector3(3.53f, 1f, -0.8213f);
    }

    // Update is called once per frame
    void Update()
    {
        if (multiTouch.Tap) currentMode = Mode.Touch;
        else if( Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) currentMode = Mode.Mouse;

        if (currentMode != Mode.None)
        {
            //Debug.Log("µé¾î¿Ó½¿");
            //Debug.Log($"isZoomIn : {isZoomIn}");
            //Debug.Log($"isCurrentEmpty : {isCurrentEmpty}");
            if (isZoomIn && isCurrentEmpty)//ºó¹æ ÁÜ¾Æ¿ô.
            {
                RaycastHit hit;
                Ray ray;
                //ÅÍÄ¡.
                if (currentMode == Mode.Touch)
                {
                    ray = camera.ScreenPointToRay(multiTouch.curTouchPos);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //ºñ¾îÀÖÀ»¶§.
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null
                            && hit.collider.gameObject.GetComponent<GardenRoom>() == null
                            && hit.collider.gameObject.GetComponent<OperatingRoom>() == null
                            && hit.collider.gameObject.GetComponent<StoreRoom>() == null)
                        {

                            isZoomIn = false;
                            CloseWindow();
                            positionToLook = centerPos;
                            currentObject = null;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomOut();
                            StartCoroutine(coroutine);
                        }
                        
                    }
                }
                //¸¶¿ì½º.
                else if (currentMode == Mode.Mouse)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //ºñ¾îÀÖÀ»¶§.
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null
                            && hit.collider.gameObject.GetComponent<GardenRoom>() == null
                            && hit.collider.gameObject.GetComponent<OperatingRoom>() == null
                            && hit.collider.gameObject.GetComponent<StoreRoom>() == null)
                        {

                            isZoomIn = false;
                            CloseWindow();
                            positionToLook = centerPos;
                            currentObject = null;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomOut();
                            StartCoroutine(coroutine);
                        }
                        else if (hit.collider.gameObject.GetComponent<GardenRoom>() != null)
                        { 
                        
                        }
                    }
                }

            }

            else if (!isZoomIn)//ÁÜÀÎ.
            {
                RaycastHit hit;
                Ray ray;
                //ÅÍÄ¡.
                if (currentMode == Mode.Touch)
                {
                    ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                        {
                            Invoke("CompleteZoomIn", 0.2f);
                            positionToLook = hit.collider.transform.position;
                            currentObject = hit.collider.transform.gameObject;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomIn();
                            StartCoroutine(coroutine);
                        }
                    }
                }
                //¸¶¿ì½º.
                else if (currentMode == Mode.Mouse)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                        {
                            Invoke("CompleteZoomIn", 0.2f);
                            positionToLook = hit.collider.transform.position;
                            currentObject = hit.collider.transform.gameObject;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomIn();
                            StartCoroutine(coroutine);
                        }
                    }
                }
            }
        }
        currentMode = Mode.None;
    }

    public void CompleteZoomIn()
    {
        isZoomIn = true;

        if(!isCurrentEmpty) OpenWindow();
    }

    IEnumerator ZoomIn()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.

        while (progress < 1)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 0.4f, progress);
            //Quaternion lookOnLook = Quaternion.LookRotation(positionToLook - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);
            var x = Mathf.Lerp(transform.position.x, currentObject.transform.position.x, progress);
            var y = Mathf.Lerp(transform.position.y, currentObject.transform.position.y, progress);
            transform.position = new Vector3(x, y, transform.position.z);

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
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5f, progress);
            //Quaternion lookOnLook = Quaternion.LookRotation(positionToLook - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);
            var x = Mathf.Lerp(transform.position.x, centerPos.x, progress);
            var y = Mathf.Lerp(transform.position.y, centerPos.y, progress);
            transform.position = new Vector3(x, y, transform.position.z);

            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }

    public void Exit()
    {
        isZoomIn = false;
        positionToLook = centerPos;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ZoomOut();
        StartCoroutine(coroutine);

        CloseWindow();
    }
}
