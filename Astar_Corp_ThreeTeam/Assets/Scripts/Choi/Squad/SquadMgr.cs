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

    public GameObject SquadLists;
    public List<GameObject> SquadList;
    Dictionary<int, string> squadData;
   
    GameObject currentSelected;
    int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 리스트 관리.
        //CharacterList = new List<GameObject>();
        //var characters = CharacterLists.transform;
        //for (int i = 0; i < characters.childCount; i++)
        //{
        //    //var child = characters.transform.GetChild(i).gameObject;
        //    //child.SetActive(true);

        //    //CharacterList.Add(child);
        //}

        characterData = new List<string>();
        characterData.Add("강주수");
        characterData.Add("최지은");
        characterData.Add("홍수진");
        characterData.Add("김미정");
        characterData.Add("김형일");
        characterData.Add("송태명");
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
        }

        //현재 슬롯에 값이 있다면 변경.
        if (squadData.ContainsKey(currentIndex))
        {
            squadData.Remove(currentIndex);
        }
        
        characterName.text = characterData[i];
        squadData.Add(currentIndex, characterName.text);
        
        characterListUI.SetActive(false);
    }
}
