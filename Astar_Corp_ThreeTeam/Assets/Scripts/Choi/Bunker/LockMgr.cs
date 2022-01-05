using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockMgr : MonoBehaviour
{
    public BunkerMgr manger;
    public GameObject lockPopup;
    public GameObject lockPrefab;
    public GameObject unlockCondition;
    List<GameObject> bunkers;
    List<GameObject> lockImgs;

    bool isUnlockConPopup;

    private void Start()
    {
        bunkers = manger.bunkerObjs;

        int randomIndex1 = Random.Range(0, 4);
        int randomIndex2 = Random.Range(4, 9);

        int[] randomIndexArr = { randomIndex1, randomIndex2 };

        foreach (var element in randomIndexArr)
        {
            var go = Instantiate(lockPrefab, lockPopup.transform);
            go.transform.position = Camera.main.WorldToScreenPoint(bunkers[element].transform.position);

            var button = go.AddComponent<Button>();
        }
    }
}
