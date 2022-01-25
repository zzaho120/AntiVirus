using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMapCam : MonoBehaviour
{
    public float moveSpeed;
    public Transform cam;

    Vector2 prevPos = Vector2.zero;
    float prevDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            if (prevPos == Vector2.zero)
            {
                prevPos = Input.GetTouch(0).position;
            }
        }
    }

    public void ExitDrag()
    {

    }
}
