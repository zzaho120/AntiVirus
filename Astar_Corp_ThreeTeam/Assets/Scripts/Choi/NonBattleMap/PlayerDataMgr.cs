using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterDetail
{
    public int id;
    public string name;
    public int hp;
    public int offensePower;
    public int willPower;
    public int stamina;
    public List<string> antivirus = new List<string>();
}

public class PlayerDataMgr : MonoBehaviour
{
    public List<Antibody> antibodyList = new List<Antibody>();
    public List<AttackDefinition> weaponList = new List<AttackDefinition>();
    public Dictionary<string, CharacterDetail> characterInfos = new Dictionary<string, CharacterDetail>();

    public Dictionary<int, string> currentSquad = new Dictionary<int, string>();
    //List<int> currentSquadIndex = new List<int>();
    //List<string> currentSquad = new List<string>();

    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();

    //플레이어 데이터.
    public PlayerSaveData saveData = new PlayerSaveData();
    string filePath;

   ScriptableMgr scriptableMgr;

    private void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        
        characterList = scriptableMgr.characterList;

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

                //캐릭터 값 생성.
                int j = 0;
                foreach (var element in characterList)
                {
                    Debug.Log($"name : {element.Value.name}");
                   
                    saveData.ids.Add(element.Value.name);
                    saveData.names.Add(element.Value.name);
                    saveData.hps.Add(Random.Range(element.Value.min_Hp, element.Value.max_Hp));
                    saveData.offensePowers.Add(element.Value.damage);
                    saveData.willPowers.Add(Random.Range(element.Value.min_Willpower, element.Value.max_Willpower));
                    saveData.staminas.Add(Random.Range(element.Value.min_Stamina, element.Value.max_Stamina));
                    
                    //B,E,I,P,T순.
                    if (Random.Range(0, 2) == 0) saveData.antivirus.Add(null);
                    else saveData.antivirus.Add($"B{Random.Range(1,4)}");
                    if (Random.Range(0, 2) == 0) saveData.antivirus.Add(null);
                    else saveData.antivirus.Add($"E{Random.Range(1, 4)}");
                    if (Random.Range(0, 2) == 0) saveData.antivirus.Add(null);
                    else saveData.antivirus.Add($"I{Random.Range(1, 4)}");
                    if (Random.Range(0, 2) == 0) saveData.antivirus.Add(null);
                    else saveData.antivirus.Add($"P{Random.Range(1, 4)}");
                    if (Random.Range(0, 2) == 0) saveData.antivirus.Add(null);
                    else saveData.antivirus.Add($"T{Random.Range(1, 4)}");

                    CharacterDetail info = new CharacterDetail();
                    info.id = j;
                    info.name = saveData.names[j];
                    info.hp = saveData.hps[j];
                    info.offensePower = saveData.offensePowers[j];
                    info.willPower = saveData.willPowers[j];
                    info.stamina = saveData.staminas[j];

                    characterInfos.Add(info.name, info);

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
                        currentSquad.Add(i, value);
                }

                    saveData = PlayerSaveLoadSystem.Load(filePath);
               
                for (int i=0; i< saveData.names.Count; i++)
                {
                    CharacterDetail info = new CharacterDetail();
                    info.id = i;
                    info.name = saveData.names[i];
                    info.hp = saveData.hps[i];
                    info.offensePower = saveData.offensePowers[i];
                    info.willPower = saveData.willPowers[i];
                    info.stamina = saveData.staminas[i];

                    for (int j = 0; j < 5; j++)
                    {
                        info.antivirus.Add(saveData.antivirus[i*5+j]);
                    }

                    characterInfos.Add(info.name, info);
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
