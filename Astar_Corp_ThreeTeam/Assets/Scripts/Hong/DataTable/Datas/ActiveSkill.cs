using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : SkillBase
{
    public int AP;
    public int coolDown;
    public int value;
    public int curCoolDown;
    public bool isUsing;
    public PlayerableChar owner;

    public virtual void Invoke() 
    {
        isUsing = true;
        var playerAction = BattleMgr.Instance.battleWindowMgr.GetWindow((int)BattleWindows.PlayerAction - 1) as PlayerActionWindow;
        if (character == "Tanker")
        {
            owner.isTankB1Skill = true;
            owner.AlertMode();
            playerAction.OnActiveDirectionBtns(false, true);
            playerAction.cancelBtn.SetActive(true);
        }
        if (character == "Bombardier")
        {
            owner.HMG_A1_Skill();
            playerAction.OnActiveDirectionBtns(false, true);
            playerAction.cancelBtn.SetActive(true);
        }
    }

    public void CheckCoolDown()
    {
        if (!isUsing)
            return;

        curCoolDown++;
        if (curCoolDown >= coolDown)
        {
            isUsing = false;
            curCoolDown = 0;
        }
    }
}
