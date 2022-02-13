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
        SetAP();
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

    public void OnClickItemConfirm()
    {
        var window = BattleMgr.Instance.battleWindowMgr.GetWindow(0) as BattleBasicWindow;

        if (window.infoPanel.activeSelf)
        {
            window.battleinfoPanel.SetItemInfo(item);
        }
        else
        {
            var player = window.selectedChar;
            if (item.ap <= player.AP)
            {
                window.OnClickItemCancel();
                player.AP -= item.ap;
                if (item.hpRecovery > 0)
                    player.UseConsumeItemForHp(item.hpRecovery);
                else if (item.virusGaugeDec > 0)
                    player.UseConsumeItemForVirus(item.virusGaugeDec);

                window.UpdateUI();
            }
        }
    }
}
