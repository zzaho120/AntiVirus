using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public static CameraController Instance { get => instance; }

    public Transform followTransform;
    public Transform mainCamera;

    [Header("Value")]
    public float moveSpeed;
    public float touchSpeed;
    public float moveTime;

    public float rotateSpeed;
    public float rotateTime;

    public Vector3 zoomSpeed;

    public Vector3 destPosition;
    public Quaternion destRotate;
    public Vector3 destZoomInOut;

    [Header("Min / Max")]
    public Vector2 zoomInOut;

    private MultiTouch touchMgr;
    private bool isSmoothMove;
    void Start()
    {
        instance = this;
        transform.position = new Vector3(TileMgr.MAX_X_IDX * 0.5f, 0f, TileMgr.MAX_Z_IDX * 0.5f);
        destPosition = transform.position;
        destRotate = transform.rotation;
        destZoomInOut = mainCamera.localPosition;
        touchMgr = BattleMgr.Instance.touchMgr;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform == null || !isSmoothMove)
        {
            MoveKeyboardInput();
            RotateKeyboardInput();
            ZoominOutKeyboardInput();
            TouchInput();
        }
        FollowObject();
    }

    private void MoveKeyboardInput()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var newX = Mathf.Clamp(destPosition.x + horizontal * moveSpeed, 0, TileMgr.MAX_X_IDX);
        var newZ = Mathf.Clamp(destPosition.z + vertical * moveSpeed, 0, TileMgr.MAX_Z_IDX);
        destPosition = new Vector3(newX, 0, newZ);

        transform.position = Vector3.Lerp(transform.position, destPosition, moveTime * Time.deltaTime);
    }

    private void TouchInput()
    {
        var touch = -touchMgr.SwipeDirection;

        var newX = Mathf.Clamp(destPosition.x + touch.x * touchSpeed, 0, TileMgr.MAX_X_IDX);
        var newZ = Mathf.Clamp(destPosition.z + touch.y * touchSpeed, 0, TileMgr.MAX_Z_IDX);
        destPosition = new Vector3(newX, 0, newZ);

        transform.position = Vector3.Lerp(transform.position, destPosition, moveTime * Time.deltaTime);
    }

    private void RotateKeyboardInput()
    {
        var rotateValue = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotateValue = -1f;
        else if (Input.GetKey(KeyCode.E))
            rotateValue = 1f;
        else
            rotateValue = 0f;

        destRotate = destRotate * Quaternion.Euler(new Vector3(0, rotateValue, 0) * rotateSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, destRotate, rotateTime * Time.deltaTime);
    }

    private void ZoominOutKeyboardInput()
    {
        if (Input.GetKey(KeyCode.R))
            destZoomInOut += zoomSpeed;
        else if (Input.GetKey(KeyCode.F))
            destZoomInOut -= zoomSpeed;

        var value = Mathf.Clamp(destZoomInOut.y, zoomInOut.x, zoomInOut.y);

        destZoomInOut = new Vector3(destZoomInOut.x, value, -value + 1);

        mainCamera.localPosition = destZoomInOut;
    }

    public void FollowObject()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
    }

    public void SetCameraTrs(Transform trs)
    {
        destPosition = trs.position;
        StopCoroutine("CoSmoothCameraMove");
        StartCoroutine(CoSmoothCameraMove(destPosition));
    }

    public void SetFollowObject(Transform transform)
    {
        if (transform == null && followTransform != null)
            destPosition = followTransform.position;
        followTransform = transform;
    }

    public IEnumerator CoSmoothCameraMove(Vector3 dest)
    {
        var timer = 0f;
        isSmoothMove = true;
        transform.position = Vector3.Lerp(transform.position, dest, moveTime * Time.deltaTime);

        while (timer < 2f)
        {
            timer += Time.deltaTime * 5;

            transform.position = Vector3.Lerp(transform.position, dest, moveTime * Time.deltaTime);

            yield return null;
        }

        isSmoothMove = false;
    }
}
