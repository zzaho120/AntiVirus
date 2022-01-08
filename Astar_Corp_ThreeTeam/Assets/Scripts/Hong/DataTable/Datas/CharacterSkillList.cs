using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSkillList
{
    // ����? ���߿� ����Ʈ�� �ٲ�� �Ҽ���?

    public List<ActiveSkill> activeSkills;
    public List<PassiveSkill> passiveSkills;

    private ScriptableMgr scriptableMgr;
    
    public CharacterSkillList(string skillId)
    {
        scriptableMgr = ScriptableMgr.Instance;

        // �ӽ� ��ų �������� (�׽�Ʈ��)
        //activeSkills[0] = scriptableMgr.GetActiveSkill(skillId);
    }
}