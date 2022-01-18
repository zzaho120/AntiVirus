using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockMgr : MonoBehaviour
{
    public BunkerCamController camController;
    public BunkerMgr manger;
    public GameObject lockPopup;
    public GameObject lockPrefab;
    public GameObject unlockCondition;
    List<GameObject> bunkers;
    List<GameObject> lockImgs = new List<GameObject>();

    bool isUnlockConPopup;
    int[] randomIndexArr;
    private void Start()
    {
    //    bunkers = manger.bunkerObjs;

    //    int randomIndex1 = Random.Range(0, 4);
    //    int randomIndex2 = Random.Range(4, 9);

    //    randomIndexArr = new int[2];
    //    randomIndexArr[0] = randomIndex1;
    //    randomIndexArr[1] = randomIndex2;

    //    foreach (var element in randomIndexArr)
    //    {
    //        var go = Instantiate(lockPrefab, lockPopup.transform);
    //        go.transform.position = Camera.main.WorldToScreenPoint(bunkers[element].transform.position);

    //        var button = go.AddComponent<Button>();
    //        button.onClick.AddListener(delegate { OpenConditionPopup(); });

    //        lockImgs.Add(go);
    //    }
    }

    public void OpenLockImg()
    {
        foreach (var element in lockImgs)
        {
            element.SetActive(true);
        }
    }

    public void CloseLockImg()
    {
        foreach (var element in randomIndexArr)
        {
            bunkers[element].GetComponent<BoxCollider>().enabled = false;
        }
            foreach (var element in lockImgs)
        {
            element.SetActive(false);
        }
    }

    public void OpenConditionPopup()
    {
        unlockCondition.SetActive(true);
    }

    public void CloseConditionPopup()
    {
        unlockCondition.SetActive(false);
    }
}
