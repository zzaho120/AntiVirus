using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoWindow : GenericWindow
{
    public BattleInfoPanel monsterInfoPanel;
    public BattleInfoPanel playerInfoPanel;
    public GameObject confirmBtn;
    public PlayerableChar curPlayer;
    public MonsterChar curMonster;
    public GameObject cancelBtn;
    private bool inited = false;
    public override void Open()
    {
        base.Open();
        monsterInfoPanel.gameObject.SetActive(false);
        playerInfoPanel.gameObject.SetActive(false);
        confirmBtn.SetActive(false);

        if (!inited)
        {
            inited = true;
            EventBusMgr.Subscribe(EventType.EndPlayer, CloseTurnEnd);
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

    public void EnableMonsterInfo(bool isEnable, MonsterChar monster, WeaponStats weapon)
    {
        monsterInfoPanel.gameObject.SetActive(isEnable);
        monsterInfoPanel.SetInfoMonster(monster, weapon);
        confirmBtn.SetActive(true);
        curMonster = monster;
    }

    public void EnableMonsterInfo(bool isEnable, MonsterChar monster)
    {
        monsterInfoPanel.gameObject.SetActive(isEnable);
        monsterInfoPanel.SetInfoMonster(monster);
        curMonster = monster;
    }
    public void EnablePlayerInfo(bool isEnable, PlayerableChar player)
    {
        playerInfoPanel.gameObject.SetActive(isEnable);
        playerInfoPanel.SetInfoPlayer(player);
        curPlayer = player;
    }

    public void OnClickConfirmBtn()
    {
        var tileIdx = new Vector2(curMonster.tileIdx.x, curMonster.tileIdx.z);
        if (BattleMgr.Instance.sightMgr.GetFrontSight(curPlayer).Exists(x => x.tileBase == curMonster.currentTile))
        {
            curPlayer.ActionAttack(curMonster);
        }

        curPlayer = null;
        curMonster = null;
        Close();
    }
}
