using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class BattleActionBtn : MonoBehaviour
{
    public GameObject apObj;
    public TextMeshProUGUI btnText;
    public Image image;

    public void SetAP(int ap)
    {
        var apObjCount = apObj.transform.childCount;
        for (var idx = 0; idx < apObjCount; ++idx)
        {
            apObj.transform.GetChild(idx).gameObject.SetActive(false);
        }

        for (var idx = 0; idx < ap; ++idx)
        {
            apObj.transform.GetChild(idx).gameObject.SetActive(true);
        }
    }
}
