using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 dragOrgin;
    private MultiTouch multiTouch;

    // ī�޶� ���� ����
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
        // ���콺 ����
        // �巡�� ������ �� ������ ���� (ó�� ������ ��)
        if (Input.GetMouseButtonDown(0))
            dragOrgin = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // ���콺�� ���� �ִ� ���� drag Origin�� new position ������ �Ÿ� ���
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrgin - mainCam.ScreenToWorldPoint(Input.mousePosition);

            print("origin " + dragOrgin + " newPosition " + mainCam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);

            // ī�޶� �̵�
            //cam.transform.position += difference;
            mainCam.transform.position = ClampCamera(mainCam.transform.position + difference);
        }

        //// ��ġ ����
        //// �巡�� ������ �� ������ ���� (ó�� ������ ��)
        //if (multiTouch.Tap)
        //    dragOrgin = cam.ScreenToWorldPoint(Input.mousePosition);
        //
        //// ���콺�� ���� �ִ� ���� drag Origin�� new position ������ �Ÿ� ���
        //if (multiTouch.Tap)
        //{
        //    Vector3 difference = dragOrgin - cam.ScreenToWorldPoint(Input.mousePosition);
        //
        //    print("origin " + dragOrgin + " newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);
        //
        //    // ī�޶� �̵�
        //    cam.transform.position += difference;
        //}
    }

    // ī�޶� ���� ������ ����� �ʰ�
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
