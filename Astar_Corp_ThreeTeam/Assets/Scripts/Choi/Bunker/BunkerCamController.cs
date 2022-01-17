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
    public LockMgr lockMgr;
    public MultiTouch multiTouch;
    public Vector3 centerPos;

    float duration = 1; // This will be your time in seconds.
    float smoothness = 0.007f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.

    public GameObject currentObject;
    public Camera camera;

    public bool isZoomIn;
    IEnumerator coroutine;
    Vector3 positionToLook;

    public bool isCurrentEmpty;

    public Action OpenWindow;
    public Action CloseWindow;
    public Action<Mode, GameObject> SetBunkerKind;

    public Mode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentMode = Mode.None;
        isCurrentEmpty = true;

        centerPos = new Vector3(3.5f, 1.06f, -7.81f);
    }

    // Update is called once per frame
    void Update()
    {
        if (multiTouch.Tap) currentMode = Mode.Touch;
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) currentMode = Mode.Mouse;

        if (currentMode != Mode.None)
        {
            if (isZoomIn && isCurrentEmpty)//빈방 줌아웃.
            {
                RaycastHit hit;
                Ray ray;

                //터치.
                if (currentMode == Mode.Touch)
                {
                    ray = camera.ScreenPointToRay(multiTouch.curTouchPos);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //비어있을때.
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null
                            && hit.collider.gameObject.GetComponent<BunkerBase>().bunkerName.Equals("None"))
                        {
                            CloseWindow();
                            Invoke("CompleteZoomOut", 1f);
                            positionToLook = centerPos;
                            currentObject = null;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomOut();
                            StartCoroutine(coroutine);
                        }

                    }
                }
                //마우스.
                else if (currentMode == Mode.Mouse)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //비어있을때.
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null
                        && hit.collider.gameObject.GetComponent<BunkerBase>().bunkerName.Equals("None"))
                        {
                            CloseWindow();
                            Invoke("CompleteZoomOut", 1f);
                            positionToLook = centerPos;
                            currentObject = null;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomOut();
                            StartCoroutine(coroutine);
                        }
                    }
                }
            }

            else if (!isZoomIn)//줌인.
            {
                RaycastHit hit;
                Ray ray;
                //터치.
                if (currentMode == Mode.Touch)
                {
                    ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                        {
                            Debug.Log("안녕");

                            SetBunkerKind(currentMode, hit.collider.gameObject);
                            if (hit.collider.gameObject.GetComponent<BunkerBase>().bunkerName.Equals("None")) isCurrentEmpty = true;
                            else isCurrentEmpty = false;

                            Invoke("CompleteZoomIn", 1f);
                            positionToLook = hit.collider.gameObject.transform.position;
                            currentObject = hit.collider.gameObject.transform.gameObject;

                            if (coroutine != null) StopCoroutine(coroutine);
                            coroutine = ZoomIn();
                            StartCoroutine(coroutine);
                        }
                    }
                }
                //마우스.
                else if (currentMode == Mode.Mouse)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.GetComponent<BunkerBase>() != null)
                        {
                            SetBunkerKind(currentMode, hit.collider.gameObject);
                            if (hit.collider.gameObject.GetComponent<BunkerBase>().bunkerName.Equals("None")) isCurrentEmpty = true;
                            else isCurrentEmpty = false;

                            Invoke("CompleteZoomIn", 1f);
                            positionToLook = hit.collider.gameObject.transform.position;
                            currentObject = hit.collider.gameObject.transform.gameObject;

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

        if (!isCurrentEmpty) OpenWindow();
    }

    public void CompleteZoomOut()
    {
        isZoomIn = false;
    }

    IEnumerator ZoomIn()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.

        lockMgr.CloseLockImg();
        while (progress < .5)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 0.4f, progress);
            var x = Mathf.Lerp(transform.position.x, currentObject.transform.position.x, progress);
            var y = Mathf.Lerp(transform.position.y, currentObject.transform.position.y, progress);
            transform.position = new Vector3(x, y, transform.position.z);

            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }

    IEnumerator ZoomOut()
    {
        lockMgr.OpenLockImg();
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < .5)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5f, progress);
            var x = Mathf.Lerp(transform.position.x, centerPos.x, progress);
            var y = Mathf.Lerp(transform.position.y, centerPos.y, progress);
            transform.position = new Vector3(x, y, transform.position.z);

            progress += increment;

            yield return new WaitForSeconds(smoothness);
        }
    }

    public void Exit()
    {
        if (bunkerMgr.destroyButton.activeSelf) bunkerMgr.destroyButton.SetActive(false);

        isZoomIn = false;
        positionToLook = centerPos;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ZoomOut();
        StartCoroutine(coroutine);

        CloseWindow();
    }
}
