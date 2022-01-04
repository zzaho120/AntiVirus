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
        foreach (var btn in buttons)
        {
            btn.SetActive(true);
        }
        directionBtns.SetActive(false);

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
    public void OnClickMoveBtn()
    {
        curChar.MoveMode();
        Close();
    }

    public void OnClickAttackBtn()
    {
        curChar.AttackMode();
        foreach (var btn in buttons)
        {
            btn.SetActive(false);
        }
        directionBtns.SetActive(true);
    }

    public void OnClickDirectionBtn(int direction)
    {
        curChar.direction = (DirectionType)(1 << direction);
        BattleMgr.Instance.sightMgr.UpdateFrontSight(curChar);
        Close();
        Debug.Log(curChar.direction);

        if (curChar.status == PlayerStatus.Alert)
            EventBusMgr.Publish(EventType.EndTurn);
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
        EventBusMgr.Publish(EventType.EndTurn);
    }

    public void OnClickAlertBtn()
    {
        curChar.AlertMode();
        foreach (var btn in buttons)
        {
            btn.SetActive(false);
        }
        directionBtns.SetActive(true);
    }
}
