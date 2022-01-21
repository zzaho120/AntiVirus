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

    public List<PassiveSkill> GetPassiveSkills(PassiveCase skillCase)
    {
        var skillList = new List<PassiveSkill>();

        foreach (var skill in passiveSkills)
        {
            switch (skillCase)
            {
                case PassiveCase.Ready:
                    if (skill.skillCase.Equals("Ready"))
                        skillList.Add(skill);
                    break;
            }
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
