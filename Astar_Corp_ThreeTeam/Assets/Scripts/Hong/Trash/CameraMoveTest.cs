using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTest : MonoBehaviour
{
    public Transform target;

    public float distance = 10f;    // 카메라, 플레이어 간 거리
    public float rotation = 4f;     // 카메라 각도 - 수치 높일수록 위쪽 봄
    public float height = 6f;       // 카메라 높이
    private float speed = 5f;

    private void Update()
    {
        Vector3 temp = target.position - target.forward.normalized * distance;
        temp = new Vector3(temp.x, temp.y + height, temp.z);
        transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime * speed); //선형보간
    }
}
