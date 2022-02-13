using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    private Vector3 origin;
    private float timer;
    public void Init(Transform target)
    {
        this.target = target;
        origin = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 4;
        transform.position = Vector3.Lerp(origin, target.position + new Vector3(0f, 1f), timer);

        if (timer > 1f)
        {
            timer = 0f;
            var returnToPool = GetComponent<ReturnToPool>();
            returnToPool.Return();
        }
    }
}
