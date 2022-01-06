using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultWindow : GenericWindow
{
    //�ӽ� �ؽ�Ʈâ
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] stats;

    private void Start()
    {
        BattleMgr battleMgr = BattleMgr.Instance;
        BattlePlayerMgr playerData = battleMgr.playerMgr;

        // �ӽ� ĳ�� ���� �ҷ�����
        for (int i = 0; i < playerData.playerableChars.Count; i++)
        {
            nameText[i].text = playerData.playerableChars[i].characterStats.Name;
            stats[i].text = "HP : " + playerData.playerableChars[i].characterStats.currentHp.ToString();
        }
    }
}
