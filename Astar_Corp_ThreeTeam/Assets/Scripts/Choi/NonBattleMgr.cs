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
    public List<GameObject> maps;

    public Dictionary<GameObject, List<GameObject>> randomEvents;
    public GameObject randomEventPrefab;

    PlayerController playerController;
    float timer;
    public MapType currentMapType;

    bool isFirst;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        randomEvents = new Dictionary<GameObject, List<GameObject>>();

        foreach (var element in maps)
        {
            //Debug.Log($"{element.name}");
            randomEvents.Add(element, new List<GameObject>());
        }

        foreach (var element in maps)
        {
            var randomNum = Random.Range(0, 4);
            for (int i = 0; i < randomNum; i++)
            {
                var randomX = element.transform.position.x + Random.Range(0, 10);
                var randomZ = element.transform.position.z + Random.Range(0, 10);

                var go = Instantiate(randomEventPrefab, new Vector3(randomX, 0, randomZ), Quaternion.identity);
                go.tag = "RandomEvent";

                randomEvents[element].Add(go);
            }
        }
    }

   

    void CreateEvents()
    {
        foreach (var element in maps)
        {
            //Debug.Log($"{element.name}");
            randomEvents.Add(element, new List<GameObject>());
        }

        foreach (var element in maps)
        {
            var randomNum = Random.Range(0, 4);
            for (int i = 0; i < randomNum; i++)
            {
                var randomX = element.transform.position.x + Random.Range(0, 10);
                var randomZ = element.transform.position.z + Random.Range(0, 10);

                var go = Instantiate(randomEventPrefab, new Vector3(randomX, 0, randomZ), Quaternion.identity);
                go.tag = "RandomEvent";

                randomEvents[element].Add(go);
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{


    //if (playerController.isMove)
    //{
    //    timer += Time.deltaTime;

    //    if (timer > 1)
    //    {
    //        timer = 0f;
    //        if (Random.Range(1, 101) <= 10)
    //        {
    //            Debug.Log($"{currentMapType}");
    //            switch (currentMapType)
    //            {

    //                case MapType.Seoul:
    //                    Debug.Log("I love Seoul");
    //                    break;
    //                case MapType.Suncheon:
    //                    Debug.Log("I love Suncheon");
    //                    break;
    //                case MapType.Daegu:
    //                    Debug.Log("I love Daegu");
    //                    break;
    //            }
    //            //Debug.Log("Encountered a monster");
    //        }
    //    }
    //}
    //}
}
