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
    public override void Open()
    {
        base.Open();
        foreach (var btn in buttons)
        {
            btn.SetActive(true);
        }
        directionBtns.SetActive(false);
    }

    public override void Close()
    {
        base.Close();
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
        {
            Close();
        }
    }
}
