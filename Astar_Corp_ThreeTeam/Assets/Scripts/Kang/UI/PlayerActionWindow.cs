using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionWindow : GenericWindow
{
    public List<GameObject> buttons;
    public PlayerableChar curChar;
    public GameObject directionBtns;
    public bool isDetailMenu;
    public bool inited;
    public override void Open()
    {
        base.Open();
        OnActiveDirectionBtns(true, false);

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
    }

    public void OnClickDirectionBtn(int direction)
    {
        curChar.direction = (DirectionType)(1 << direction);
        BattleMgr.Instance.sightMgr.UpdateFrontSight(curChar);
        Close();

        if (curChar.status == PlayerStatus.Alert || curChar.status == PlayerStatus.Wait)
            curChar.EndPlayer();
    }

    public void OnClickCancelBtn()
    {
        if (isDetailMenu)
        {
            foreach (var btn in buttons)
            {
                btn.SetActive(true);
            }
        }
        else
            Close();
    }

    public void OnClickTurnEndBtn()
    {
        curChar.EndPlayer();
    }

    public void OnClickAlertBtn()
    {
        curChar.AlertMode();
        OnActiveDirectionBtns(false, true);
    }

    public void OnActiveDirectionBtns(bool enableBtns, bool enableDir)
    {
        foreach (var btn in buttons)
        {
            btn.SetActive(enableBtns);
        }
        directionBtns.SetActive(enableDir);
    }
}
