using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive
}

[System.Serializable]
public class SkillMgr
{
    public List<ActiveSkill> activeSkills;
    public List<PassiveSkill> passiveSkills;

    public SkillMgr()
    {
        activeSkills = new List<ActiveSkill>();
        passiveSkills = new List<PassiveSkill>();
    }

    public List<PassiveSkill> GetPassiveSkills(PassiveCase skillCase)
    {
        var skillList = new List<PassiveSkill>();
        Debug.Log(passiveSkills);
        foreach (var skill in passiveSkills)
        {
            Debug.Log(skill);
            if (skill == null)
                continue;

            if (skill.skillCase == skillCase)
                skillList.Add(skill);
        }
        
        return skillList;
    }

    public void AddSkill(SkillType type, SkillBase skill)
    {
        switch (type)
        {
            case SkillType.Active:
                activeSkills.Add(skill as ActiveSkill);
                break;
            case SkillType.Passive:
                passiveSkills.Add(skill as PassiveSkill);
                break;
        }
    }
}
