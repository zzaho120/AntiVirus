using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTest : MonoBehaviour
{
    public Transform target;

    public float distance = 10f;    // ī�޶�, �÷��̾� �� �Ÿ�
    public float rotation = 4f;     // ī�޶� ���� - ��ġ ���ϼ��� ���� ��
    public float height = 6f;       // ī�޶� ����
    private float speed = 5f;

    private void Update()
    {
        Vector3 temp = target.position - target.forward.normalized * distance;
        temp = new Vector3(temp.x, temp.y + height, temp.z);
        transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime * speed); //��������
    }
}
