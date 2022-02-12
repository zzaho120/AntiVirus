using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableMgr : Singleton<ScriptableMgr>
{
    #region Dictionary List
    // 캐릭터 스탯 관련
    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();

    // 캐릭터 레벨 경험치 관련
    public Dictionary<string, CharacterExp> characterExpList = new Dictionary<string, CharacterExp>();

    public Dictionary<string, MissionExp> missionExpList = new Dictionary<string, MissionExp>();

    // 몬스터 스탯 관련
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();

    // 아이템 관련
    public Dictionary<string, Weapon> equippableList = new Dictionary<string, Weapon>();
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();
    public Dictionary<string, OtherItem> otherItemList = new Dictionary<string, OtherItem>();

    // 스킬 관련
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();

    // 트럭 관련
    public Dictionary<string, Truck> truckList = new Dictionary<string, Truck>();

    // 비전투씬 몬스터
    public Dictionary<string, WorldMonster> worldMonsterList = new Dictionary<string, WorldMonster>();

    //벙커 관련
    public Dictionary<string, Bunker> bunkerList = new Dictionary<string, Bunker>();

    //가방 관련
    public Dictionary<string, Inventory> bagList = new Dictionary<string, Inventory>();

    //캐릭터 이름 관련
    public Dictionary<string, Name> nameList = new Dictionary<string, Name>();

    // 내성 최대 경험치 관련
    public Dictionary<string, ResistExp> resistExpList = new Dictionary<string, ResistExp>();
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
            virusList.Add($"{virus.name}", virus);
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

        // 10. Bunker
        string bunkerSOPath = "Choi/Datas/Bunkers";
        Bunker[] bunkerArr = Resources.LoadAll<Bunker>(bunkerSOPath);
        foreach (var bunker in bunkerArr)
        {
            bunkerList.Add($"{bunker.id}", bunker);
        }

        // 11. Bag
        string bagSOPath = "Choi/Datas/Inventorys";
        Inventory[] bagArr = Resources.LoadAll<Inventory>(bagSOPath);
        foreach (var bag in bagArr)
        {
            bagList.Add($"{bag.id}", bag);
        }

        // 12. Bag
        string nbmSOPath = "Choi/Datas/WorldMonsters";
        WorldMonster[] worldMonsterArr = Resources.LoadAll<WorldMonster>(nbmSOPath);
        foreach (var monster in worldMonsterArr)
        {
            worldMonsterList.Add($"{monster.id}", monster);
        }

        // 13. OtherItems
        string otherItemsSOPath = "Choi/Datas/OtherItems";
        OtherItem[] otherItemsArr = Resources.LoadAll<OtherItem>(otherItemsSOPath);
        foreach (var otherItem in otherItemsArr)
        {
            otherItemList.Add($"{otherItem.id}", otherItem);
        }

        // 14. Character Exp
        string characterExpSOPath = "Choi/Datas/CharacterExp";
        CharacterExp[] characterExpArr = Resources.LoadAll<CharacterExp>(characterExpSOPath);
        foreach (var characterExp in characterExpArr)
        {
            characterExpList.Add($"{characterExp.id}", characterExp);
        }

        // 14. Mission Exp
        string missionExpSOPath = "Choi/Datas/MissionExp";
        MissionExp[] missionExpArr = Resources.LoadAll<MissionExp>(missionExpSOPath);
        foreach (var missionExp in missionExpArr)
        {
            missionExpList.Add($"{missionExp.id}", missionExp);
        }

        // 15. Character Name
        string characterNameSOPath = "Choi/Datas/Names";
        Name[] nameArr = Resources.LoadAll<Name>(characterNameSOPath);
        foreach (var name in nameArr)
        {
            nameList.Add($"{name.id}", name);
        }

        // 16. resist Exp
        string resistExpSOPath = "Choi/Datas/ResistExp";
        ResistExp[] resistExpArr = Resources.LoadAll<ResistExp>(resistExpSOPath);
        foreach (var resistExp in resistExpArr)
        {
            resistExpList.Add($"{resistExp.id}", resistExp);
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
    /// 매개변수 입력 팁; 바이러스 이름 (E, B, P, I, T)
    /// </summary>
    public Virus GetVirus(string name)
    {
        return Instantiate(virusList[name]);
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

    /// <summary>
    /// 매개변수 입력 팁; NBM_0000
    /// </summary>
    public WorldMonster GetWorldMonster(string id)
    {
        return Instantiate(worldMonsterList[id]);
    }


    /// <summary>
    /// 매개변수 입력 팁; EXP_0000
    /// </summary>
    public CharacterExp GetCharacterExp(string id)
    {
        return Instantiate(characterExpList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; MEP_0000
    /// </summary>
    public MissionExp GetMissionExp(string id)
    {
        return Instantiate(missionExpList[id]);
    }

    /// <summary>
    /// 매개변수 입력 팁; RES_0
    /// </summary>
    public ResistExp GetResistExp(string id)
    {
        return Instantiate(resistExpList[id]);
    }
}
