using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Seoul,
    Suncheon,
    Daegu,
    None
}

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;

    PlayerControllerNew playerController;
    float timer;
    public MapType currentMapType;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerControllerNew>();
        currentMapType = MapType.Seoul;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isMove)
        {
            timer += Time.deltaTime;

            if (timer > 1)
            {
                timer = 0f;
                if (Random.Range(1, 101) <= 10)
                {
                    Debug.Log($"{currentMapType}");
                    switch (currentMapType)
                    {
                       
                        case MapType.Seoul:
                            Debug.Log("I love Seoul");
                            break;
                        case MapType.Suncheon:
                            Debug.Log("I love Suncheon");
                            break;
                        case MapType.Daegu:
                            Debug.Log("I love Daegu");
                            break;
                    }
                    //Debug.Log("Encountered a monster");
                }
            }
        }
    }
}
