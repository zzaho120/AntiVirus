using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;
    private Vector3 dragOrgin;
    private MultiTouch multiTouch;

    // ī�޶� ���� ����
    private float mapMinX, mapMaxX, mapMinZ, mapMaxZ;
    [SerializeField]
    private MeshRenderer mapRenderer;

    [HideInInspector]
    public bool isWorldMapMode;

    private void Start()
    {
        cam = Camera.main;
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
            dragOrgin = cam.ScreenToWorldPoint(Input.mousePosition);

        // ���콺�� ���� �ִ� ���� drag Origin�� new position ������ �Ÿ� ���
        if (Input.GetMouseButton(0))
        {
            Debug.Log("�ùķ�����");
            Vector3 difference = dragOrgin - cam.ScreenToWorldPoint(Input.mousePosition);

            print("origin " + dragOrgin + " newPosition " + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);

            // ī�޶� �̵�
            //cam.transform.position += difference;
            cam.transform.position = ClampCamera(cam.transform.position + difference);
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

    public void SwitchCamMode()
    {
        if (isWorldMapMode)
        {
            isWorldMapMode = false;
        }
        else if (!isWorldMapMode)
        {
            isWorldMapMode = true;
        }
    }

    // ī�޶� ���� ������ ����� �ʰ�
    private Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize ;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minZ = mapMinZ + camHeight;
        float maxZ = mapMaxZ - camHeight;

        float newX = Mathf.Clamp(targetPos.x, minX - 10, maxX + 10);
        float newZ = Mathf.Clamp(targetPos.z, minZ - 10, maxZ + 10);

        return new Vector3(newX, targetPos.y, newZ);

    }
}
