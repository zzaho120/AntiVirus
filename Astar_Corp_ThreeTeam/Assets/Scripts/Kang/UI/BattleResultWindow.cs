using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultWindow : GenericWindow
{
    public GameObject lootPanel;
    public GameObject squadPanel;
    public override void Open()
    {
        base.Open();

        squadPanel.SetActive(true);
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
