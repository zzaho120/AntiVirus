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
    //List<string> characterData;
    //List<Character> characterList;
    Dictionary<int, CharacterDetail> characterInfos  = new Dictionary<int, CharacterDetail>();

    public GameObject SquadLists;
    public List<GameObject> SquadList;
    Dictionary<int, string> squadData;
   
    GameObject currentSelected;
    int currentIndex;

    PlayerDataMgr playerDataMgr;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script : SquadMgr");

        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        //var characterList = playerDataMgr.characterList;

        int k = 0;
        foreach (var element in playerDataMgr.characterInfos)
        {
            characterInfos.Add(k, element.Value);
            k++;
        }

        var currentSquad = playerDataMgr.currentSquad;
        foreach (var element in currentSquad)
        {
            var child = SquadLists.transform.GetChild(element.Key).gameObject;
            var squadName = child.transform.GetChild(0).gameObject.GetComponent<Text>();
            squadName.text = element.Value.character.name;
        }

        //캐릭터 리스트 창 생성.
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
        if (squadData.ContainsValue(characterInfos[i].name))
        {
            var key = squadData.FirstOrDefault(x => x.Value == characterInfos[i].name).Key;
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

        characterName.text = characterInfos[i].name;
        squadData.Add(currentIndex, characterName.text);
        playerDataMgr.currentSquad.Add(currentIndex, playerDataMgr.characterStats[characterName.text]);
        
        string squadStr = $"Squad{currentIndex}";
        PlayerPrefs.SetString(squadStr, characterName.text);

        characterListUI.SetActive(false);
    }
}
