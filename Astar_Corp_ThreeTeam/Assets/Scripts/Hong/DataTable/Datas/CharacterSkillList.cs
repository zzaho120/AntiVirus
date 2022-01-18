using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSkillList
{
    public List<ActiveSkill> activeSkills;
    public List<PassiveSkill> passiveSkills;

    private ScriptableMgr scriptableMgr;

    public CharacterSkillList() { }

    public CharacterSkillList(string skillId)
    {
        scriptableMgr = ScriptableMgr.Instance;

        // 임시 스킬 가져오기 (테스트용)
        //activeSkills[0] = scriptableMgr.GetActiveSkill(skillId);
    }
}
