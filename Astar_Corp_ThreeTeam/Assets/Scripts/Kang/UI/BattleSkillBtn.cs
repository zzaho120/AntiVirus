using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkillBtn : MonoBehaviour
{
    public PlayerableChar owner;
    public ActiveSkill skill;

    public void OnClickBtn()
    {
        if (owner.AP >= skill.AP)
        {
            skill.owner = owner;
            skill.Invoke();
        }
        else
        {
        }
    }
}