using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionWindow : GenericWindow
{
    public List<GameObject> buttons;
    public GameObject moveBtn;
    public GameObject reloadBtn;
    public GameObject cancelBtn;
    public PlayerableChar curChar;
    public GameObject directionBtns;
    public bool inited;
    public override void Open()
    {
        base.Open();
        OnActiveDirectionBtns(true, false);

        if (BattleMgr.Instance.turnCount == 0)
        {
            foreach (var btn in buttons)
            {
                btn.gameObject.SetActive(false);
            }
        }


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

    public void OnClickMoveBtn()
    {
        curChar.MoveMode();
        Close();
    }

    public void OnClickAttackBtn()
    {
        curChar.AttackMode();
        OnActiveDirectionBtns(false, true);
        cancelBtn.SetActive(true);
    }

    public void OnClickDirectionBtn(int direction)
    {
        curChar.direction = (DirectionType)(1 << direction);
        BattleMgr.Instance.sightMgr.UpdateFrontSight(curChar);
        Close();

        if (curChar.status == PlayerState.Alert || curChar.AP <= 0)
            curChar.EndPlayer();
        else if (curChar.status == PlayerState.Move)
            curChar.WaitPlayer();
    }

    public void OnClickCancelBtn()
    {
        curChar.SetNonSelected();
    }

    public void OnClickTurnEndBtn()
    {
        curChar.EndPlayer();
    }

    public void OnClickAlertBtn()
    {
        curChar.AlertMode();
        OnActiveDirectionBtns(false, true);
        cancelBtn.SetActive(true);
    }

    public void OnActiveDirectionBtns(bool enableBtns, bool enableDir)
    {
        foreach (var btn in buttons)
        {
            btn.SetActive(enableBtns);
        }
        directionBtns.SetActive(enableDir);
    }

    public void OnClickReloadBtn()
    {
        curChar.ReloadWeapon();
        curChar.SetNonSelected();
    }

    public void EnableReloadBtn()
    {
        var weapon = curChar.characterStats.weapon;
        if (weapon.WeaponBullet < weapon.curWeapon.bullet)
            reloadBtn.SetActive(true);
        else
            reloadBtn.SetActive(false);
    }
}
