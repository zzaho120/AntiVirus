using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Value")]
    public float moveSpeed;
    public float rotateSpeed;
    public Vector3 destPosition;
    public Quaternion destRotate;


    void Start()
    {
        destPosition = transform.position;
        destRotate = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveKeyboardInput();
        RotateKeyboardInput();
    }

    private void MoveKeyboardInput()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        destPosition = destPosition + new Vector3(horizontal, 0, vertical);

        transform.position = Vector3.Lerp(transform.position, destPosition, moveSpeed * Time.deltaTime);
    }

    private void RotateKeyboardInput()
    {
    }
}
