using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SquadMgr : MonoBehaviour
{
    public GameObject characterListUI;

    //ĳ���� ����Ʈ.
    public GameObject characterListPrefab;
    public GameObject CharacterLists;
    public List<GameObject> CharacterList;
    Dictionary<int, Character> characterInfos  = new Dictionary<int, Character>();

    //������ ����Ʈ.
    public GameObject SquadParents;
    public GameObject SquadPrefab;
    public List<GameObject> SquadGOs;

    //public GameObject SquadLists;
    //public List<GameObject> SquadList;
    Dictionary<int, string> SquadData;
    int totalSquadNum = 8;

    GameObject currentSelected;
    int currentIndex;

    public PlayerDataMgr playerDataMgr;
    // Start is called before the first frame update
    void Start()
    {
        SquadData = new Dictionary<int, string>();

        string str = "SquadNum";
        totalSquadNum = (PlayerPrefs.HasKey(str))? PlayerPrefs.GetInt(str) : 4;

        var squadParents = SquadParents.transform;
        for (int i = 0; i < totalSquadNum; i++)
        {
            var go = Instantiate(SquadPrefab, squadParents);
            var button = go.AddComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { ClickSquad(num); });
            SquadGOs.Add(go);
        }

        var currentSquad = playerDataMgr.currentSquad;
        Debug.Log($"currentSquad.Count : {currentSquad.Count}");
        foreach (var element in currentSquad)
        {
            Debug.Log($"element.Key : {element.Key}");

            var child = SquadParents.transform.GetChild(element.Key).gameObject;
            var squadName = child.transform.GetChild(0).gameObject.GetComponent<Text>();
            squadName.text = element.Value.character.name;
            SquadData.Add(element.Key, element.Value.character.name);
        }

        //ĳ���� ����Ʈ â ����.
        int k = 0;
        foreach (var element in playerDataMgr.characterList)
        {
            characterInfos.Add(k, element.Value);
            k++;
        }

        CharacterList = new List<GameObject>();
        var characters = CharacterLists.transform;
        for (int i = 0; i < characterInfos.Count; i++)
        {
            var go = Instantiate(characterListPrefab, characters);
            var button = go.GetComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { ClickCharacter(num); });

            CharacterList.Add(go);
        }

        int j = 0;
        foreach (var element in characterInfos)
        {
            var child = CharacterList[j].transform.GetChild(0).gameObject;

            var characterName = child.GetComponent<Text>();
            characterName.text = element.Value.name;
            j++;
        }

        //������ ����Ʈ ����.
        //SquadList = new List<GameObject>();
        //var squadLists = SquadLists.transform;
        //for (int i = 0; i < squadLists.childCount; i++)
        //{
        //    SquadList.Add(squadLists.GetChild(i).gameObject);
        //}
    }

    public void ClickSquad(int i)
    {
        currentSelected = SquadGOs[i];
        currentIndex = i;

        characterListUI.SetActive(true);
    }

    public void ClickCharacter(int i)
    {
        if (currentSelected == null) return;

        var child = currentSelected.transform.GetChild(0).gameObject;
        var characterName = child.GetComponent<Text>();

        ////�ٸ� ���Կ� �ִٸ� �ٸ� ������ �������.
        //if (squadData.ContainsValue(characterInfos[i].name))
        //{
        //    var key = squadData.FirstOrDefault(x => x.Value == characterInfos[i].name).Key;
        //    var previous = SquadList[key].transform.GetChild(0).gameObject;
        //    var previousName = previous.GetComponent<Text>();
        //    previousName.text = string.Empty;

        //    squadData.Remove(key);
        //    playerDataMgr.currentSquad.Remove(key);
        //    string str = $"Squad{key}";
        //    PlayerPrefs.SetString(str, null);
        //}

        //���� ���Կ� ���� �ִٸ� ����.
        if (SquadData.ContainsKey(currentIndex))
        {
            SquadData.Remove(currentIndex);
            playerDataMgr.currentSquad.Remove(currentIndex);
        }

        characterName.text = characterInfos[i].name;
        SquadData.Add(currentIndex, characterName.text);

        Debug.Log($"currentIndex : {currentIndex}");

        playerDataMgr.AddCharacter(characterInfos[i], currentIndex);
        
        string squadStr = $"Squad{currentIndex}";
        PlayerPrefs.SetString(squadStr, characterName.text);

        characterListUI.SetActive(false);
    }

    public void AddSquad()
    {
        var squadParents = SquadParents.transform;
        var go = Instantiate(SquadPrefab, squadParents);
        var button = go.AddComponent<Button>();
       
        totalSquadNum += 1;
        int num = totalSquadNum;
        button.onClick.AddListener(delegate { ClickSquad(num-1); });
        SquadGOs.Add(go);

        SquadData.Add(num - 1, null);

        Character character = new Character();
        character.name = string.Empty;

        playerDataMgr.AddCharacter(character, num-1);
       
        string squadStr = $"Squad{num-1}";
        PlayerPrefs.SetString(squadStr,null);
    }
}
