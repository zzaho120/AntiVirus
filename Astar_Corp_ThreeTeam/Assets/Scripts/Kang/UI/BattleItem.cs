using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleItem : MonoBehaviour
{
    public GameObject apObj;
    public TextMeshProUGUI btnText;
    public Consumable item;

    public void Init(Consumable item)
    {
        this.item = item;
        btnText.text = item.storeName;
    }

    public void SetAP()
    {
        var apObjCount = apObj.transform.childCount;
        for (var idx = 0; idx < apObjCount; ++idx)
        {
            apObj.transform.GetChild(idx).gameObject.SetActive(false);
        }

        for (var idx = 0; idx < item.ap; ++idx)
        {
            apObj.transform.GetChild(idx).gameObject.SetActive(true);
        }
    }
}
