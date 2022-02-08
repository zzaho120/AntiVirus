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
    public GameObject turnEndBtn;
    public PlayerableChar curChar;
    public GameObject directionBtns;
    public bool inited;

    public BattleSkillBtn activeBtn;
    public GameObject content;

    private List<GameObject> activeList = new List<GameObject>();
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

        var childCount = content.transform.childCount;
        for (var idx = 0; idx < childCount; ++idx)
        {
            Destroy(content.transform.GetChild(idx).gameObject);
        }
    }

    public void CloseTurnEnd(object empty)
    {
        Close();
    }

    public void AddActiveList()
    {
        if (curChar != null)
        {
            var activeList = curChar.characterStats.skillMgr.activeSkills;

            foreach (var skill in activeList)
            {
                var go = Instantiate(activeBtn.gameObject, content.transform);
                go.gameObject.name = skill.skillName;

                var active = go.GetComponent<BattleSkillBtn>();
                active.owner = curChar;
                active.skill = skill;
            }
        }
    }

    public void AddBuffList()
    {
        if (curChar != null)
        {
            var buffList = curChar.characterStats.buffMgr.buffList;

            //foreach (var buff in buffList)
            //{
            //    var go = Instantiate(buffBtn, content.transform);
            //    go.name = buff.stat.ToString();
            //}
        }
    }

    public void OnClickMoveBtn()
    {
        curChar.MoveMode(); 
        foreach (var btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
        moveBtn.SetActive(false);
        reloadBtn.SetActive(false);
        turnEndBtn.SetActive(false);
    }

    public void OnClickAttackBtn()
    {
        curChar.AttackMode();
        OnActiveDirectionBtns(false, true);
        cancelBtn.SetActive(true);
    }

    public void OnClickDirectionBtn(int direction)
    {
        curChar.SetDirection((DirectionType)(1 << direction));
        BattleMgr.Instance.sightMgr.UpdateFrontSight(curChar);
        Close();

        var isFullApMove = curChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        if ((curChar.status == CharacterState.Alert || curChar.AP <= 0) && !isFullApMove)
            curChar.EndPlayer();
        else if (curChar.status == CharacterState.Move)
            curChar.WaitPlayer();

        cancelBtn.SetActive(true);
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

        moveBtn.SetActive(enableBtns);
        reloadBtn.SetActive(enableBtns);
        turnEndBtn.SetActive(enableBtns);
        cancelBtn.SetActive(enableBtns);
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
