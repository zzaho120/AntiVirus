using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableMgr : Singleton<ScriptableMgr>
{
    #region Dictionary List
    // 캐릭터 스탯 관련
    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();

    // 몬스터 스탯 관련
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();

    // 아이템 관련
    public Dictionary<string, Weapon> equippableList = new Dictionary<string, Weapon>();
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();

    // 스킬 관련
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();

    // 트럭 관련
    public Dictionary<string, Truck> truckList = new Dictionary<string, Truck>();
    #endregion

    public override void Awake()
    {
        base.Awake();

        // 1. Character
        string charSOPath = "Choi/Datas/Characters";
        Character[] characterArr = Resources.LoadAll<Character>(charSOPath);
        foreach(var character in characterArr)
        {
            characterList.Add(character.id, character);
        }

        // 2. Antibody
        // 없어짐...

        // 3. Consumable
        string cousumableSOPath = "Choi/Datas/Consumables";
        Consumable[] cousumableArr = Resources.LoadAll<Consumable>(cousumableSOPath);
        foreach (var cousumable in cousumableArr)
        {
            consumableList.Add(cousumable.id, cousumable);
        }

        // 4. Equippable -> Weapon
        string equippableSOPath = "Choi/Datas/Weapons";
        Weapon[] equippableArr = Resources.LoadAll<Weapon>(equippableSOPath);
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
            virusList.Add($"{virus.name}{virus.penaltyType}", virus);
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

        // 9. Trucks
        string truckSOPath = "Choi/Datas/Trucks";
        Truck[] truckArr = Resources.LoadAll<Truck>(truckSOPath);
        foreach (var truck in truckArr)
        {
            truckList.Add($"{truck.id}", truck);
        }
    }

    /// <summary>
    /// 매개변수 입력 팁; CHAR_0000
    /// </summary>
    public Character GetCharacter(string id)
    {
        return (Character)Instantiate(characterList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; CON_0000
    /// </summary>
    public Consumable GetConsumable(string id)
    {
        return Instantiate(consumableList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; WEP_0000
    /// 장비템 같지만 무기입니다
    /// </summary>
    public Weapon GetEquippable(string id)
    {
        return Instantiate(equippableList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; MON_0000
    /// </summary>
    public Monster GetMonster(string id)
    {
        return Instantiate(monsterList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; VIR_0000
    /// </summary>
    public Virus GetVirus(string id)
    {
        return Instantiate(virusList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; ASK_0000
    /// </summary>
    public ActiveSkill GetActiveSkill(string id)
    {
        return Instantiate(activeSkillList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; PSK_0000
    /// </summary>
    public PassiveSkill GetPassiveSkill(string id)
    {
        return Instantiate(passiveSkillList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; TRU_0000
    /// </summary>
    public Truck GetTruck(string id)
    {
        return Instantiate(truckList[id]);
    }
}
