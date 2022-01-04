using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoWindow : GenericWindow
{
    public BattleInfoPanel monsterInfoPanel;
    public BattleInfoPanel playerInfoPanel;
    private bool inited = false;
    public override void Open()
    {
        base.Open();
        monsterInfoPanel.gameObject.SetActive(false);
        playerInfoPanel.gameObject.SetActive(false);

        if (!inited)
        {
            inited = true;
            EventBusMgr.Subscribe(EventType.EndTurn, CloseTurnEnd);
        }
    }

    public override void Close()
    {
        base.Close();
    }

    public void CloseTurnEnd(object empty)
    {
        Close();
    }

    public void EnableMonsterInfo(bool isEnable, MonsterChar monster)
    {
        monsterInfoPanel.gameObject.SetActive(isEnable);
        monsterInfoPanel.SetInfoMonster(monster);

    }
    public void EnablePlayerInfo(bool isEnable, PlayerableChar player)
    {
        playerInfoPanel.gameObject.SetActive(isEnable);
        playerInfoPanel.SetInfoPlayer(player);
    }
}
