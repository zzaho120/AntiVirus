using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultWindow : GenericWindow
{
    public GameObject lootPanel;
    public GameObject squadPanel;
    public Transform squadContent;
    public GameObject squadListPrefab;
    public override void Open()
    {
        base.Open();

        squadPanel.SetActive(true);
        var players = BattleMgr.Instance.playerMgr.playerableChars;

        for (var idx = 0; idx < players.Count; ++idx)
        {
            var sqaud = Instantiate(squadListPrefab, squadContent);
            var resultInfo = sqaud.GetComponent<BattleResultInfo>();

            var window = BattleMgr.Instance.battleWindowMgr.GetWindow(0) as BattleBasicWindow;
            
            resultInfo.Init(players[idx], window.names[idx]);
            
        }

        lootPanel.SetActive(false);
    }

    public void OnClickSquadNextBtn()
    {
        squadPanel.SetActive(false);
        lootPanel.SetActive(true);
    }

    public void OnClickLootNextBtn()
    {
        SceneManager.LoadScene("WorldMap_Asset");
    }

    public void OnClickLootPrevBtn()
    {
        squadPanel.SetActive(true);
        lootPanel.SetActive(false);
    }
}
