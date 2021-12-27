using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataMgr : MonoBehaviour
{
    public List<Character> characterList = new List<Character>();
    public List<Antibody> antibodyList = new List<Antibody>();
    public List<AttackDefinition> weaponList = new List<AttackDefinition>();

    public Dictionary<int, string> currentSquad = new Dictionary<int, string>();
    //List<int> currentSquadIndex = new List<int>();
    //List<string> currentSquad = new List<string>();

    //플레이어 데이터.
    public PlayerSaveData saveData = new PlayerSaveData();
    string filePath;
    
    private void Awake()
    {
        filePath = @$"{Application.persistentDataPath}\PlayerData.json";

        var obj = FindObjectsOfType<PlayerDataMgr>(); 
        if (obj.Length == 1) 
        {    
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("Squad0"))
            {
                for (int i = 0; i < 4; i++)
                {
                    string str = $"Squad{i}";
                    PlayerPrefs.SetString(str, null);
                }

                //PlayerSaveLoadSystem.Save(saveData);
            }
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
            }
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }
    //private void Update()
    //{
    //    if (isFirst)
    //    {
    //        isFirst = false;
    //        return;
    //    }

    //    if (!PlayerPrefs.HasKey("Squad0"))
    //    {
    //        //캐릭터 값 생성.
    //        foreach (var element in characterList)
    //        {
    //            saveData.ids.Add(element.name);
    //            saveData.names.Add(element.name);
    //            saveData.hps.Add(Random.Range(element.min_Hp, element.max_Hp));
    //            saveData.offensePowers.Add(element.damage);
    //            saveData.willPowers.Add(Random.Range(element.min_Willpower, element.max_Willpower));
    //            saveData.staminas.Add(Random.Range(element.min_Stamina, element.max_Stamina));
    //        }
    //        PlayerSaveLoadSystem.Save(saveData);
    //    }
    //    else
    //    {
    //        //캐릭터 값 불러오기.
    //        saveData = PlayerSaveLoadSystem.Load(filePath);
    //    }
    //}
}
