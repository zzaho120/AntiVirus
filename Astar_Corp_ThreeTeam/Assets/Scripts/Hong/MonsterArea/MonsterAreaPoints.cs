using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAreaPoints : MonoBehaviour
{
    private BoxCollider boxCollider;
    private GameObject wayPoint;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();    
    }

    private void Update()
    {
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = Return_RandomPosition();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = new Vector3 (transform.position.x, transform.position.y + 10f, transform.position.z);
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = boxCollider.bounds.size.x;
        float range_Z = boxCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
