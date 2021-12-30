using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionWindow : GenericWindow
{
    public List<Button> buttons;
    public PlayerableChar curChar;
    public override void Open()
    {
        base.Open();
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
        BattleMgr.Instance.sightMgr.AddFrontSight(curChar);
        Close();
    }

    public void OnClickDirectionBtn(int direction)
    {
        curChar.direction = (DirectionType)(1 << direction);
        Debug.Log(curChar.direction);
    }
}
