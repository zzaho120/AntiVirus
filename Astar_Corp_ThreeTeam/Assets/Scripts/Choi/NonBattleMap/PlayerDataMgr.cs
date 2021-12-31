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
    public int offensePower;
    public int willPower;
    public int stamina;
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
    public Dictionary<string, Equippable> equippableList = new Dictionary<string, Equippable>();
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();
    ScriptableMgr scriptableMgr;

    //아이템 데이터.
    public Dictionary<string, Equippable> currentEquippables = new Dictionary<string, Equippable>();
    public Dictionary<string, int> currentEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> currentConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> currentConsumablesNum = new Dictionary<string, int>();

    //캐릭터 데이터.
    //세이브 데이터 관리하기 쉽도록.(세이브 데이터 삭제하기 위한 id도 별도로 있음)
    public Dictionary<string, CharacterDetail> characterInfos = new Dictionary<string, CharacterDetail>(); 
    //게임상 관리하기 쉽도록.
    public Dictionary<string, CharacterStats> characterStats = new Dictionary<string, CharacterStats>();
    public Dictionary<int, CharacterStats> currentSquad = new Dictionary<int, CharacterStats>();

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
        if (saveData.ids == null)
        {
            saveData.ids = new List<string>();
            saveData.names = new List<string>();
            saveData.hps = new List<int>();
            saveData.offensePowers = new List<int>();
            saveData.willPowers = new List<int>();
            saveData.staminas = new List<int>();
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
                int j = 0;
                foreach (var element in characterList)
                {
                    //초기값 설정.
                    saveData.ids.Add(element.Value.id);
                    saveData.names.Add(element.Value.name);
                    saveData.hps.Add(Random.Range(element.Value.min_Hp, element.Value.max_Hp));
                    saveData.offensePowers.Add(element.Value.damage);
                    saveData.willPowers.Add(Random.Range(element.Value.min_Willpower, element.Value.max_Willpower));
                    saveData.staminas.Add(Random.Range(element.Value.min_Stamina, element.Value.max_Stamina));
                    for (int k = 0; k < 5; k++) { saveData.antivirus.Add(null); }
                    saveData.mainWeapon.Add(null);
                    saveData.mainWeaponNum.Add(0);
                    saveData.subWeapon.Add(null);
                    saveData.subWeaponNum.Add(0);
                    for (int k = 0; k < 5; k++){ saveData.activeSkillList.Add(null); }
                    for (int k = 0; k < 5; k++) { saveData.passiveSkillList.Add(null); }

                    //저장된 데이터 관리하기 쉽도록.
                    CharacterDetail info = new CharacterDetail();
                    info.saveId = j;
                    info.characterId = saveData.ids[j];
                    info.name = saveData.names[j];
                    info.hp = saveData.hps[j];
                    info.offensePower = saveData.offensePowers[j];
                    info.willPower = saveData.willPowers[j];
                    info.stamina = saveData.staminas[j];
                    for (int k = 0; k < 5; k++){ info.antivirus.Add(saveData.antivirus[j * 5 + k]); }
                    info.mainWeapon = saveData.mainWeapon[j];
                    info.mainWeaponNum = saveData.mainWeaponNum[j];
                    info.subWeapon = saveData.subWeapon[j];
                    info.subWeaponNum = saveData.subWeaponNum[j];

                    int activeSkillNum = activeSkillList.Count;
                    for (int k = 0; k < activeSkillNum; k++) { info.antivirus.Add(saveData.activeSkillList[j * activeSkillNum + k]); }
                    int passiveSkillNum = passiveSkillList.Count;
                    for (int k = 0; k < passiveSkillNum; k++) { info.antivirus.Add(saveData.passiveSkillList[j * passiveSkillNum + k]); }
                    characterInfos.Add(info.name, info);

                    //게임상 관리하기 쉽도록.
                    CharacterStats stat = new CharacterStats();
                    stat.currentHp = info.hp;
                    stat.maxHp = element.Value.max_Hp;
                    stat.willpower = info.willPower;
                    stat.stamina = info.stamina;
                    stat.character = element.Value;
                    stat.character.id = info.characterId;
                    stat.mainWeapon = (info.mainWeapon == null)? null :equippableList[info.mainWeapon];
                    stat.subWeapon = (info.subWeapon == null)? null : equippableList[info.subWeapon];
                    
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
                    characterStats.Add(info.name, stat);
                    j++;
                }

                characterInfos.OrderBy(x => x.Key);
                PlayerSaveLoadSystem.Save(saveData);
            }
            //이어하기.
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    string str = $"Squad{i}";
                    string value = PlayerPrefs.GetString(str);
                    Debug.Log($"{i} : {value}");
                    if (!string.IsNullOrEmpty(value))
                    {
                        currentSquad.Add(i, characterStats[value]);
                    }
                }

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
                for (int i=0; i< saveData.names.Count; i++)
                {
                    //저장된 데이터 관리하기 쉽도록.
                    CharacterDetail info = new CharacterDetail();
                    info.saveId = i;
                    info.characterId = saveData.ids[i];
                    info.name = saveData.names[i];
                    info.hp = saveData.hps[i];
                    info.offensePower = saveData.offensePowers[i];
                    info.willPower = saveData.willPowers[i];
                    info.stamina = saveData.staminas[i];
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
                    
                    characterInfos.Add(info.name, info);

                    //게임상 관리하기 쉽도록.
                    CharacterStats stat = new CharacterStats();
                    stat.currentHp = info.hp;
                    stat.maxHp = characterList[info.characterId].max_Hp;
                    stat.willpower = info.willPower;
                    stat.stamina = info.stamina;
                    stat.character = characterList[info.characterId];
                    stat.character.id = info.characterId;
                    foreach(var antivirus in info.antivirus)
                    {
                        if (antivirus == null) continue;
                        //if (!antibodyList.ContainsKey(antivirus)) continue;
                        stat.antibody.Add(antibodyList[antivirus]);
                    }
                    stat.mainWeapon = (info.mainWeapon == null) ? null : equippableList[info.mainWeapon];
                    stat.subWeapon = (info.subWeapon == null) ? null : equippableList[info.subWeapon];

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

                    characterStats.Add(info.name, stat);
                }
                characterInfos.OrderBy(x => x.Key);
            }
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }
}
