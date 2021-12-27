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

    private void Awake()
    {
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
}
