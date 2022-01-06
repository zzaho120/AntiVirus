using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultWindow : GenericWindow
{
    //임시 텍스트창
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] stats;

    private void Start()
    {
        BattleMgr battleMgr = BattleMgr.Instance;
        BattlePlayerMgr playerData = battleMgr.playerMgr;

        // 임시 캐릭 정보 불러오기
        for (int i = 0; i < playerData.playerableChars.Count; i++)
        {
            nameText[i].text = playerData.playerableChars[i].characterStats.Name;
            stats[i].text = "HP : " + playerData.playerableChars[i].characterStats.currentHp.ToString();
        }
    }
}
