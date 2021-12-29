using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableMgr : Singleton<ScriptableMgr>
{
    #region Dictionary List
    // 캐릭터 스탯 관련
    public Dictionary<string, Antibody> antibodyList = new Dictionary<string, Antibody>(); 
    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();

    // 몬스터 스탯 관련
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();

    // 아이템 관련
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();
    public Dictionary<string, Equippable> equippableList = new Dictionary<string, Equippable>();

    // 스킬 관련
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();
    #endregion

    void Start()
    {
        // 1. Character
        string charSOPath = "Choi/Datas/Characters";
        Character[] characterArr = Resources.LoadAll<Character>(charSOPath);
        foreach(var character in characterArr)
        {
            characterList.Add(character.id, character);
        }

        // 2. Antibody
        string antibodySOPath = "Choi/Datas/Antibodys";
        Antibody[] antibodyArr = Resources.LoadAll<Antibody>(antibodySOPath);
        foreach(var antibody in antibodyArr)
        {
            antibodyList.Add($"{antibody.name}{antibody.level}", antibody);
        }

        // 3. Consumable
        string cousumableSOPath = "Choi/Datas/Consumables";
        Consumable[] cousumableArr = Resources.LoadAll<Consumable>(cousumableSOPath);
        foreach (var cousumable in cousumableArr)
        {
            consumableList.Add(cousumable.id, cousumable);
        }

        // 4. Equippable
        string equippableSOPath = "Choi/Datas/Equippables";
        Equippable[] equippableArr = Resources.LoadAll<Equippable>(equippableSOPath);
        foreach (var equippable in equippableArr)
        {
            equippableList.Add(equippable.id, equippable);
        }

        // 5. Monster
        string monsterSOPath = "Choi/Datas/Monsters";
        Monster[] monsterArr = Resources.LoadAll<Monster>(monsterSOPath);
        foreach (var monster in monsterArr)
        {
            monsterList.Add(monster.id, monster);
        }

        // 6. Virus
        string virusSOPath = "Choi/Datas/Viruses";
        Virus[] virusArr = Resources.LoadAll<Virus>(virusSOPath);
        foreach (var virus in virusArr)
        {
            virusList.Add($"{virus.name}{virus.level}", virus);
        }

        // 7. Active Skills
        string activeSkillSOPath = "Choi/Datas/Skills/ActiveSkills";
        ActiveSkill[] activeSkillArr = Resources.LoadAll<ActiveSkill>(activeSkillSOPath);
        foreach (var skill in activeSkillArr)
        {
            activeSkillList.Add($"{skill.id}", skill);
        }

        // 8. Passive Skills
        string passiveSkillSOPath = "Choi/Datas/Skills/PassiveSkills";
        PassiveSkill[] passiveSkillArr = Resources.LoadAll<PassiveSkill>(passiveSkillSOPath);
        foreach (var skill in passiveSkillArr)
        {
            passiveSkillList.Add($"{skill.id}", skill);
        }
    }

    /// <summary>
    /// 매개변수 입력 시, {Name}{level} 형식으로 입력 ex) E1, P3, T2 ...
    /// </summary>
    public Antibody GetAntibody(string id)
    {
        return Instantiate(antibodyList[id]);
    }

    public Character GetCharacter(string id)
    {
        return (Character)Instantiate(characterList[id]);
    }

    public Consumable GetConsumable(string id)
    {
        return Instantiate(consumableList[id]);
    }

    public Equippable GetEquippable(string id)
    {
        return Instantiate(equippableList[id]);
    }

    public Monster GetMonster(string id)
    {
        return Instantiate(monsterList[id]);
    }

    /// <summary>
    /// 매개변수 입력 시, {Name}{level} 형식으로 입력 ex) E1, P3, T2 ...
    /// </summary>
    public Virus GetVirus(string id)
    {
        return Instantiate(virusList[id]);
    }

    public ActiveSkill GetActiveSkill(string id)
    {
        return Instantiate(activeSkillList[id]);
    }

    public PassiveSkill GetPassiveSkill(string id)
    {
        return Instantiate(passiveSkillList[id]);
    }

    //private void Update()
    //{
    //    //var testChar = GetCharacter("1");
    //    //Debug.Log(testChar.name);
    //
    //    var testVirus = GetVirus("E2");
    //    var testVirus = GetVirus("B5");
    //    Debug.Log($"{testVirus.name} Hp : {testVirus.hp}");
    //}

    //public T GetData<T>(string id)
    //{
    //
    //    return (Character)Instantiate(characterList[id]);
    //}
}
