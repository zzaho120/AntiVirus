using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SquadMgr : MonoBehaviour
{
    public GameObject characterListUI;

    public GameObject characterListPrefab;
    public GameObject CharacterLists;
    public List<GameObject> CharacterList;
    List<string> characterData;
    List<Character> characterList;

    public GameObject SquadLists;
    public List<GameObject> SquadList;
    Dictionary<int, string> squadData;
   
    GameObject currentSelected;
    int currentIndex;

    PlayerDataMgr playerDataMgr;
    // Start is called before the first frame update
    void Start()
    {
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        var characterList = playerDataMgr.characterList;

        var currentSquad = playerDataMgr.currentSquad;
        
        foreach (var element in currentSquad)
        {
            var child = SquadLists.transform.GetChild(element.Key).gameObject;
            var squadName = child.transform.GetChild(0).gameObject.GetComponent<Text>();
            squadName.text = element.Value;
        }

        //캐릭터 데이터 가져옴.
        characterData = new List<string>();
        foreach (var element in characterList)
        {
            characterData.Add(element.name);
        }

        characterData = characterData.OrderBy(x => x).ToList<string>();
        
        CharacterList = new List<GameObject>();
        var characters = CharacterLists.transform;
        for (int i = 0; i < characterData.Count; i++)
        {
            var go = Instantiate(characterListPrefab, characters);
            var button = go.GetComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { ClickCharacter(num); });

            CharacterList.Add(go);
            
        }

        int j = 0;
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var child = CharacterList[i].transform.GetChild(0).gameObject;

            var characterName = child.GetComponent<Text>();
            characterName.text = characterData[j];
            j++;
        }

        //스쿼드 리스트 관리.
        SquadList = new List<GameObject>();
        var squadLists = SquadLists.transform;
        for (int i = 0; i < squadLists.childCount; i++)
        {
            SquadList.Add(squadLists.GetChild(i).gameObject);
        }

        squadData = new Dictionary<int, string>();
    }

    public void ClickSquad(int i)
    {
        currentSelected = SquadList[i];
        currentIndex = i;

        characterListUI.SetActive(true);
    }

    public void ClickCharacter(int i)
    {
        if (currentSelected == null) return;

        var child = currentSelected.transform.GetChild(0).gameObject;
        var characterName = child.GetComponent<Text>();

        //다른 슬롯에 있다면 다른 슬롯은 비워버림.
        if (squadData.ContainsValue(characterData[i]))
        {
            var key = squadData.FirstOrDefault(x => x.Value == characterData[i]).Key;
            var previous = SquadList[key].transform.GetChild(0).gameObject;
            var previousName = previous.GetComponent<Text>();
            previousName.text = string.Empty;

            squadData.Remove(key);
            playerDataMgr.currentSquad.Remove(key);
            string str = $"Squad{key}";
            PlayerPrefs.SetString(str, null);
        }

        //현재 슬롯에 값이 있다면 변경.
        if (squadData.ContainsKey(currentIndex))
        {
            squadData.Remove(currentIndex);
            playerDataMgr.currentSquad.Remove(currentIndex);
        }

        characterName.text = characterData[i];
        squadData.Add(currentIndex, characterName.text);
        playerDataMgr.currentSquad.Add(currentIndex, characterName.text);
        
        string squadStr = $"Squad{currentIndex}";
        PlayerPrefs.SetString(squadStr, characterName.text);

        characterListUI.SetActive(false);
    }
}
