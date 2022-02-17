using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 dragOrgin;
    private MultiTouch multiTouch;

    // 카메라 영역 제한
    private float mapMinX, mapMaxX, mapMinZ, mapMaxZ;

    [SerializeField]
    private MeshRenderer mapRenderer;

    [HideInInspector]
    public bool isWorldMapMode = false;

    private void Start()
    {
        mainCam = Camera.main;
        multiTouch = GameObject.Find("MultiTouch").GetComponent<MultiTouch>();

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinZ = mapRenderer.transform.position.z - mapRenderer.bounds.size.z / 2f;
        mapMaxZ = mapRenderer.transform.position.z + mapRenderer.bounds.size.z / 2f;
    }

    private void Update()
    {
        if (isWorldMapMode)
            DragCamera();
    }

    private void DragCamera()
    {
        // 마우스 버전
        // 드래그 시작할 때 포지션 저장 (처음 눌렸을 때)
        if (Input.GetMouseButtonDown(0))
            dragOrgin = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // 마우스가 눌려 있는 동안 drag Origin과 new position 사이의 거리 계산
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrgin - mainCam.ScreenToWorldPoint(Input.mousePosition);

            print("origin " + dragOrgin + " newPosition " + mainCam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);

            // 카메라 이동
            //cam.transform.position += difference;
            mainCam.transform.position = ClampCamera(mainCam.transform.position + difference);
        }

        //// 터치 버전
        //// 드래그 시작할 때 포지션 저장 (처음 눌렸을 때)
        //if (multiTouch.Tap)
        //    dragOrgin = cam.ScreenToWorldPoint(Input.mousePosition);
        //
        //// 마우스가 눌려 있는 동안 drag Origin과 new position 사이의 거리 계산
        //if (multiTouch.Tap)
        //{
        //    Vector3 difference = dragOrgin - cam.ScreenToWorldPoint(Input.mousePosition);
        //
        //    print("origin " + dragOrgin + " newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);
        //
        //    // 카메라 이동
        //    cam.transform.position += difference;
        //}
    }

    // 카메라 영역 밖으로 벗어나지 않게
    private Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = mainCam.orthographicSize ;
        float camWidth = mainCam.orthographicSize * mainCam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minZ = mapMinZ + camHeight;
        float maxZ = mapMaxZ - camHeight;

        float newX = Mathf.Clamp(targetPos.x, minX - 10, maxX + 10);
        float newZ = Mathf.Clamp(targetPos.z, minZ - 10, maxZ + 10);

        return new Vector3(newX, targetPos.y, newZ);

    }

    public void SwitchCamMode()
    {
        if (isWorldMapMode)
        {
            isWorldMapMode = false;
            mainCam.orthographic = false;
        }
        else if (!isWorldMapMode)
        {
            isWorldMapMode = true;
            mainCam.orthographic = true;
        }
    }

}
