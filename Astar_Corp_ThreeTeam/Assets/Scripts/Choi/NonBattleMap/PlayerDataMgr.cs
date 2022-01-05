using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterDetail
{
    public int saveId;
    public string characterId;
    public string name;
    public int hp;
    public int sensitivity;
    public int concentration;
    public int willPower;

    public int gaugeE;
    public int gaugeB;
    public int gaugeP;
    public int gaugeI;
    public int gaugeT;

    public int levelE;
    public int levelB;
    public int levelP;
    public int levelI;
    public int levelT;

    public List<string> antivirus = new List<string>();
    public string mainWeapon;
    public int mainWeaponNum;//강화수치등.
    public string subWeapon;
    public int subWeaponNum;

    public List<string> activeSkills = new List<string>();
    public List<string> passiveSkills = new List<string>();
}

public class PlayerDataMgr : MonoBehaviour
{
    //플레이어 데이터.
    public PlayerSaveData saveData = new PlayerSaveData();
    string filePath;

    //SO데이터.
    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();
    public Dictionary<string, Antibody> antibodyList = new Dictionary<string, Antibody>();
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();
    public Dictionary<string, Weapon> equippableList = new Dictionary<string, Weapon>();
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();
    ScriptableMgr scriptableMgr;

    //아이템 데이터.
    public Dictionary<string, Weapon> currentEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> currentEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> currentConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> currentConsumablesNum = new Dictionary<string, int>();

    //캐릭터 데이터.
    //세이브 데이터 관리하기 쉽도록.(세이브 데이터 삭제하기 위한 id도 별도로 있음)
    public Dictionary<int, CharacterDetail> characterInfos = new Dictionary<int, CharacterDetail>(); 
    //게임상 관리하기 쉽도록.
    public Dictionary<int, CharacterStats> characterStats = new Dictionary<int, CharacterStats>();
    public Dictionary<int, CharacterStats> currentSquad = new Dictionary<int, CharacterStats>();//현재 캐릭터들.
    public Dictionary<int, CharacterStats> battleSquad = new Dictionary<int, CharacterStats>();//전투에 나갈 캐릭터들.

    private void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        
        characterList = scriptableMgr.characterList;
        antibodyList = scriptableMgr.antibodyList;
        consumableList = scriptableMgr.consumableList;
        equippableList = scriptableMgr.equippableList;
        monsterList = scriptableMgr.monsterList;
        virusList = scriptableMgr.virusList;
        activeSkillList = scriptableMgr.activeSkillList;
        passiveSkillList = scriptableMgr.passiveSkillList;

        filePath = @$"{Application.persistentDataPath}\PlayerData.json";
        if (saveData.id == null)
        {
            saveData.id = new List<string>();
            saveData.name = new List<string>();
            saveData.hp = new List<int>();
            saveData.sensitivity = new List<int>();
            saveData.concentration = new List<int>();
            saveData.willPower = new List<int>();

            saveData.antivirus = new List<string>();
            saveData.mainWeapon = new List<string>();
            saveData.mainWeaponNum = new List<int>();
            saveData.subWeapon = new List<string>();
            saveData.subWeaponNum = new List<int>();
            saveData.activeSkillList = new List<string>();
            saveData.passiveSkillList = new List<string>();
            saveData.equippableList = new List<string>();
            saveData.equippableNumList = new List<int>();
            saveData.consumableList = new List<string>();
            saveData.consumableNumList = new List<int>();
           
            saveData.gaugeE = new List<int>();
            saveData.gaugeB = new List<int>();
            saveData.gaugeP = new List<int>();
            saveData.gaugeI = new List<int>();
            saveData.gaugeT = new List<int>();

            saveData.levelE = new List<int>();
            saveData.levelB = new List<int>();
            saveData.levelP = new List<int>();
            saveData.levelI = new List<int>();
            saveData.levelT = new List<int>();
        }

        var obj = FindObjectsOfType<PlayerDataMgr>(); 
        if (obj.Length == 1) 
        {    
            DontDestroyOnLoad(gameObject);

            //처음 실행.
            if (!PlayerPrefs.HasKey("Squad0"))
            {
                for (int i = 0; i < 4; i++)
                {
                    string str = $"Squad{i}";
                    PlayerPrefs.SetString(str, null);
                }

                //아이템 설정.
                foreach (var element in equippableList)
                {
                    saveData.equippableList.Add(element.Key);
                    int num = 1;
                    saveData.equippableNumList.Add(num);
                    currentEquippables.Add(element.Key, equippableList[element.Key]);
                    currentEquippablesNum.Add(element.Key, num);
                }
                foreach (var element in consumableList)
                {
                    saveData.consumableList.Add(element.Key);
                    int num = Random.Range(1, 10);
                    saveData.consumableNumList.Add(num);
                    currentConsumables.Add(element.Key, consumableList[element.Key]);
                    currentConsumablesNum.Add(element.Key, num);
                }

                //캐릭터 값 생성.
                for (int i = 0; i < 4; i++)
                {
                    Character character = new Character();
                    character.name = string.Empty;
                    int num = i;
                    AddCharacter(character, num);
                }
            }
            //이어하기.
            else
            {
                saveData = PlayerSaveLoadSystem.Load(filePath);

                //아이템 설정.
                int k = 0;
                foreach (var element in saveData.equippableList)
                {
                    currentEquippables.Add(element, equippableList[element]);
                    int num = k;
                    currentEquippablesNum.Add(element, saveData.equippableNumList[num]);
                    k++;
                }
                k = 0;
                foreach (var element in saveData.consumableList)
                {
                    currentConsumables.Add(element, consumableList[element]);
                    int num = k;
                    currentConsumablesNum.Add(element, saveData.consumableNumList[num]);
                    k++;
                }
                
                //캐릭터 설정.
                for (int i=0; i< saveData.name.Count; i++)
                {
                    //저장된 데이터 관리하기 쉽도록.
                    CharacterDetail info = new CharacterDetail();
                    info.saveId = i;
                    info.characterId = saveData.id[i];
                    info.name = saveData.name[i];
                    info.hp = saveData.hp[i];
                    info.sensitivity = saveData.sensitivity[i];
                    info.concentration = saveData.concentration[i];
                    info.willPower = saveData.willPower[i];
                
                    for (int j = 0; j < 5; j++)
                    {
                        info.antivirus.Add(saveData.antivirus[i*5+j]);
                    }
                    info.mainWeapon = saveData.mainWeapon[i];
                    info.mainWeaponNum = saveData.mainWeaponNum[i];
                    info.subWeapon = saveData.subWeapon[i];
                    info.subWeaponNum = saveData.subWeaponNum[i];

                    int activeSkillNum = activeSkillList.Count;
                    for (int j = 0; j < activeSkillNum; j++) { info.antivirus.Add(saveData.activeSkillList[i * activeSkillNum + j]); }
                    int passiveSkillNum = passiveSkillList.Count;
                    for (int j = 0; j < passiveSkillNum; j++) { info.antivirus.Add(saveData.passiveSkillList[i * passiveSkillNum + j]); }
                    
                    characterInfos.Add(i, info);

                    //게임상 관리하기 쉽도록.
                    CharacterStats stat = new CharacterStats();
                    stat.currentHp = info.hp;
                    stat.sensivity = info.sensitivity;
                    stat.concentration = info.concentration;
                    stat.willpower = info.willPower;
                    if (saveData.id[i] == string.Empty)
                    {
                        Character character = new Character();
                        character.name = string.Empty;
                        stat.character = character;
                    }
                    else stat.character = characterList[info.characterId];
                    stat.character.id = info.characterId;
                    
                    foreach (var antivirus in info.antivirus)
                    {
                        if (antivirus == null) continue;
                        stat.antibody.Add(antibodyList[antivirus]);
                    }
                    stat.weapon = new WeaponStats();
                    stat.weapon.mainWeapon = (info.mainWeapon == null) ? null : equippableList[info.mainWeapon];
                    stat.weapon.subWeapon = (info.subWeapon == null) ? null : equippableList[info.subWeapon];

                    foreach (var activeSkill in info.activeSkills)
                    {
                        if (!activeSkillList.ContainsKey(activeSkill)) continue;
                        stat.skills.activeSkills.Add(activeSkillList[activeSkill]);
                    }
                    foreach (var passiveSkill in info.passiveSkills)
                    {
                        if (!passiveSkillList.ContainsKey(passiveSkill)) continue;
                        stat.skills.passiveSkills.Add(passiveSkillList[passiveSkill]);
                    }

                    characterStats.Add(i, stat);
                }
                characterInfos.OrderBy(x => x.Key);

                string squadNum = "SquadNum";
                int totalSquadNum = (PlayerPrefs.HasKey(squadNum)) ? PlayerPrefs.GetInt(squadNum) : 4;
                for (int i = 0; i < totalSquadNum; i++)
                {
                    string str = $"Squad{i}";
                    string value = PlayerPrefs.GetString(str);
                    Debug.Log($"{i} : {value}");
                    if (!string.IsNullOrEmpty(value))
                    {
                        currentSquad.Add(i, characterStats[i]);
                    }
                    else
                    {
                        CharacterStats stats = new CharacterStats();
                        Character character = new Character();
                        stats.character = character;
                        stats.character.name = string.Empty;
                        currentSquad.Add(i, stats);
                    }
                }
            }
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }

    public void AddCharacter(Character character, int num)
    {
        string str = "SquadNum";
        int totalSquadNum = (PlayerPrefs.HasKey(str)) ? PlayerPrefs.GetInt(str) : 0;

        Debug.Log($"totalSquadNum : {totalSquadNum}");

        if (num > totalSquadNum-1)
        {
            if (character.name != string.Empty)
            {
                saveData.id.Add(character.id);
                saveData.name.Add(character.name);
                saveData.hp.Add(Random.Range(character.min_Hp, character.max_Hp));
                saveData.sensitivity.Add(Random.Range(character.min_Sensitivity, character.max_Sensitivity));
                saveData.concentration.Add(Random.Range(character.min_Concentration, character.max_Concentration));
                saveData.willPower.Add(Random.Range(character.min_Willpower, character.max_Willpower));
            }
            else
            {
                string emptyStr = string.Empty;
                int emptyInt = -1;
                saveData.id.Add(emptyStr);
                saveData.name.Add(emptyStr);
                saveData.hp.Add(emptyInt);
                saveData.sensitivity.Add(emptyInt);
                saveData.concentration.Add(emptyInt);
                saveData.willPower.Add(emptyInt);
            }

            saveData.gaugeE.Add(0);
            saveData.gaugeB.Add(0);
            saveData.gaugeP.Add(0);
            saveData.gaugeI.Add(0);
            saveData.gaugeT.Add(0);

            saveData.levelE.Add(1);
            saveData.levelB.Add(1);
            saveData.levelP.Add(1);
            saveData.levelI.Add(1);
            saveData.levelT.Add(1);

            for (int k = 0; k < 5; k++) { saveData.antivirus.Add(null); }
            saveData.mainWeapon.Add(null);
            saveData.mainWeaponNum.Add(0);
            saveData.subWeapon.Add(null);
            saveData.subWeaponNum.Add(0);
            for (int k = 0; k < 5; k++) { saveData.activeSkillList.Add(null); }
            for (int k = 0; k < 5; k++) { saveData.passiveSkillList.Add(null); }

            //저장된 데이터 관리하기 쉽도록.
            CharacterDetail info = new CharacterDetail();
            info.saveId = num;
            info.characterId = saveData.id[num];
            info.name = saveData.name[num];
            info.hp = saveData.hp[num];
            info.sensitivity = saveData.sensitivity[num];
            info.concentration = saveData.concentration[num];
            info.willPower = saveData.willPower[num];

            info.gaugeE = saveData.gaugeE[num];
            info.gaugeB = saveData.gaugeB[num];
            info.gaugeP = saveData.gaugeP[num];
            info.gaugeI = saveData.gaugeI[num];
            info.gaugeT = saveData.gaugeT[num];

            info.levelE = saveData.levelE[num];
            info.levelB = saveData.levelB[num];
            info.levelP = saveData.levelP[num];
            info.levelI = saveData.levelI[num];
            info.levelT = saveData.levelT[num];

            for (int k = 0; k < 5; k++) { info.antivirus.Add(saveData.antivirus[num * 5 + k]); }
            info.mainWeapon = saveData.mainWeapon[num];
            info.mainWeaponNum = saveData.mainWeaponNum[num];
            info.subWeapon = saveData.subWeapon[num];
            info.subWeaponNum = saveData.subWeaponNum[num];

            int activeSkillNum = activeSkillList.Count;
            for (int k = 0; k < activeSkillNum; k++) { info.antivirus.Add(saveData.activeSkillList[num * activeSkillNum + k]); }
            int passiveSkillNum = passiveSkillList.Count;
            for (int k = 0; k < passiveSkillNum; k++) { info.antivirus.Add(saveData.passiveSkillList[num * passiveSkillNum + k]); }
            characterInfos.Add(num, info);

            //게임상 관리하기 쉽도록.
            CharacterStats stat = new CharacterStats();
            stat.currentHp = info.hp;
            stat.maxHp = character.max_Hp;
            stat.sensivity = info.sensitivity;
            stat.concentration = info.concentration;
            stat.willpower = info.willPower;

            stat.character = character;
            stat.character.id = info.characterId;

            stat.virusPanalty["E"].gauge = saveData.gaugeE[num];
            stat.virusPanalty["B"].gauge = saveData.gaugeB[num];
            stat.virusPanalty["P"].gauge = saveData.gaugeP[num];
            stat.virusPanalty["I"].gauge = saveData.gaugeI[num];
            stat.virusPanalty["T"].gauge = saveData.gaugeT[num];

            stat.virusPanalty["E"].level = saveData.levelE[num];
            stat.virusPanalty["B"].level = saveData.levelB[num];
            stat.virusPanalty["P"].level = saveData.levelP[num];
            stat.virusPanalty["I"].level = saveData.levelI[num];
            stat.virusPanalty["T"].level = saveData.levelT[num];

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = (info.mainWeapon == null) ? null : equippableList[info.mainWeapon];
            stat.weapon.subWeapon = (info.subWeapon == null) ? null : equippableList[info.subWeapon];

            foreach (var activeSkill in info.activeSkills)
            {
                if (!activeSkillList.ContainsKey(activeSkill)) continue;
                stat.skills.activeSkills.Add(activeSkillList[activeSkill]);
            }
            foreach (var passiveSkill in info.passiveSkills)
            {
                if (!passiveSkillList.ContainsKey(passiveSkill)) continue;
                stat.skills.passiveSkills.Add(passiveSkillList[passiveSkill]);
            }
            characterStats.Add(num, stat);

            currentSquad.Add(num, stat);
            string squadNum = "SquadNum";
            int currentNum = PlayerPrefs.HasKey(squadNum)? PlayerPrefs.GetInt(squadNum) : 0;
            PlayerPrefs.SetInt(squadNum, currentNum + 1);
        }
        else //기존거 변경.
        {
            if (character.name != string.Empty)
            {
                saveData.id[num] = character.id;
                saveData.name[num] = character.name;
                saveData.hp[num]= Random.Range(character.min_Hp, character.max_Hp);
                saveData.sensitivity[num] = Random.Range(character.min_Sensitivity, character.max_Sensitivity);
                saveData.concentration[num] = Random.Range(character.min_Concentration, character.max_Concentration);
                saveData.willPower[num] = Random.Range(character.min_Willpower, character.max_Willpower);
            }
            else
            {
                string emptyStr = string.Empty;
                int emptyInt = -1;
                saveData.id[num] = emptyStr;
                saveData.name[num] = emptyStr;
                saveData.hp[num] = emptyInt;
                saveData.sensitivity[num] = emptyInt;
                saveData.concentration[num] = emptyInt;
                saveData.willPower[num] = emptyInt;
            }

            saveData.gaugeE[num] = 0;
            saveData.gaugeB[num] = 0;
            saveData.gaugeP[num] = 0;
            saveData.gaugeI[num] = 0;
            saveData.gaugeT[num] = 0;

            saveData.levelE[num] = 1;
            saveData.levelB[num] = 1;
            saveData.levelP[num] = 1;
            saveData.levelI[num] = 1;
            saveData.levelT[num] = 1;

            for (int k = 0; k < 5; k++) { saveData.antivirus[num*5+k] = null; }
            saveData.mainWeapon[num] = null;
            saveData.mainWeaponNum[num] = 0;
            saveData.subWeapon[num] = null;
            saveData.subWeaponNum[num] = 0;
            for (int k = 0; k < 5; k++) { saveData.activeSkillList[num*5 +k] = null; }
            for (int k = 0; k < 5; k++) { saveData.passiveSkillList[num*5 +k] = null; }

            //저장된 데이터 관리하기 쉽도록.
            CharacterDetail info = new CharacterDetail();
            info.saveId = num;
            info.characterId = saveData.id[num];
            info.name = saveData.name[num];
            info.hp = saveData.hp[num];
            info.sensitivity = saveData.sensitivity[num];
            info.concentration = saveData.concentration[num];
            info.willPower = saveData.willPower[num];

            info.gaugeE = saveData.gaugeE[num];
            info.gaugeB = saveData.gaugeB[num];
            info.gaugeP = saveData.gaugeP[num];
            info.gaugeI = saveData.gaugeI[num];
            info.gaugeT = saveData.gaugeT[num];

            info.levelE = saveData.levelE[num];
            info.levelB = saveData.levelB[num];
            info.levelP = saveData.levelP[num];
            info.levelI = saveData.levelI[num];
            info.levelT = saveData.levelT[num];

            for (int k = 0; k < 5; k++) { info.antivirus.Add(saveData.antivirus[num * 5 + k]); }
            info.mainWeapon = saveData.mainWeapon[num];
            info.mainWeaponNum = saveData.mainWeaponNum[num];
            info.subWeapon = saveData.subWeapon[num];
            info.subWeaponNum = saveData.subWeaponNum[num];

            int activeSkillNum = activeSkillList.Count;
            for (int k = 0; k < activeSkillNum; k++) { info.antivirus.Add(saveData.activeSkillList[num * activeSkillNum + k]); }
            int passiveSkillNum = passiveSkillList.Count;
            for (int k = 0; k < passiveSkillNum; k++) { info.antivirus.Add(saveData.passiveSkillList[num * passiveSkillNum + k]); }
            characterInfos[num] = info;

            //게임상 관리하기 쉽도록.
            CharacterStats stat = new CharacterStats();
            stat.currentHp = info.hp;
            stat.maxHp = character.max_Hp;
            stat.sensivity = info.sensitivity;
            stat.concentration = info.concentration;
            stat.willpower = info.willPower;

            stat.character = character;
            stat.character.id = info.characterId;

            stat.virusPanalty["E"].gauge = saveData.gaugeE[num];
            stat.virusPanalty["B"].gauge = saveData.gaugeB[num];
            stat.virusPanalty["P"].gauge = saveData.gaugeP[num];
            stat.virusPanalty["I"].gauge = saveData.gaugeI[num];
            stat.virusPanalty["T"].gauge = saveData.gaugeT[num];

            stat.virusPanalty["E"].level = saveData.levelE[num];
            stat.virusPanalty["B"].level = saveData.levelB[num];
            stat.virusPanalty["P"].level = saveData.levelP[num];
            stat.virusPanalty["I"].level = saveData.levelI[num];
            stat.virusPanalty["T"].level = saveData.levelT[num];

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = (info.mainWeapon == null) ? null : equippableList[info.mainWeapon];
            stat.weapon.subWeapon = (info.subWeapon == null) ? null : equippableList[info.subWeapon];

            foreach (var activeSkill in info.activeSkills)
            {
                if (!activeSkillList.ContainsKey(activeSkill)) continue;
                stat.skills.activeSkills.Add(activeSkillList[activeSkill]);
            }
            foreach (var passiveSkill in info.passiveSkills)
            {
                if (!passiveSkillList.ContainsKey(passiveSkill)) continue;
                stat.skills.passiveSkills.Add(passiveSkillList[passiveSkill]);
            }
            characterStats[num] = stat;
            currentSquad[num] = stat;
        }

        characterInfos.OrderBy(x => x.Key);
        PlayerSaveLoadSystem.Save(saveData);
    }
}
