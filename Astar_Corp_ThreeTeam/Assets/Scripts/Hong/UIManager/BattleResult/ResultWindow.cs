using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultWindow : GenericWindow
{
    //�ӽ� �ؽ�Ʈâ
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] stats;

    BattleMgr battleMgr;
    BattlePlayerMgr playerData;

    public void UpdateCharInfo()
    {
        battleMgr = BattleMgr.Instance;
        playerData = battleMgr.playerMgr;

        Debug.Log(playerData.playerableChars.Count);

        // �ӽ� ĳ�� ���� �ҷ�����
        for (int i = 0; i < playerData.playerableChars.Count; i++)
        {
            nameText[i].text = playerData.playerableChars[i].characterStats.Name;
            stats[i].text = "HP : " + playerData.playerableChars[i].characterStats.currentHp.ToString();
        }
    }

    public void SaveSquadInfo()
    {

        StartCoroutine(CoLoadBunker());
    }

    private IEnumerator CoLoadBunker()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Bunker");
    }
}
