using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BoardingMgr : MonoBehaviour
{
    public GameObject ListContent;
    public GameObject characterPrefab;
    public Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    public Dictionary<int, CharacterStats> characterInfo = new Dictionary<int, CharacterStats>();
    public Dictionary<int, int> isBoarding = new Dictionary<int, int>();
    public List<GameObject> seats;

    int currentIndex;
    int currentSeatNum;

    PlayerDataMgr playerDataMgr;
    Color originColor;

    private void Start()
    {
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            if (element.Value.character.name == string.Empty) continue;

            var go = Instantiate(characterPrefab, ListContent.transform);
            var button = go.AddComponent<Button>();

            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.character.name;

            int num = i;
            button.onClick.AddListener(delegate { SelectCharacter(num); });
            characters.Add(num, go);
            characterInfo.Add(num, element.Value);
         
            i++;
        }

        i = 0;
        foreach (var element in seats)
        {
            int num = i;
            isBoarding.Add(num, -1);

            i++;
        }

        currentIndex = -1;
        currentSeatNum = -1;

        originColor = new Color(255, 192, 0);
    }

    public void SelectCharacter(int i)
    {
        if (currentSeatNum == -1) return;

        if(currentIndex!=-1)
        characters[currentIndex].GetComponent<Image>().color = originColor;

        currentIndex = i;
        characters[currentIndex].GetComponent<Image>().color = Color.red;
    }

    public void SelectSeat(int i)
    {
        if (currentSeatNum != -1) seats[currentSeatNum].GetComponent<Image>().color = Color.white;

        Debug.Log($"currentSeatNum : {currentSeatNum}");
        Debug.Log($"count : {seats.Count}");

        currentSeatNum = i;
        seats[currentSeatNum].GetComponent<Image>().color = Color.red;
    }

    //Å¾½Â.
    public void GetInTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;

        foreach (var element in isBoarding)
        {
            if (element.Value == currentIndex) return;
        }
        
        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = characterInfo[currentIndex].character.name.Substring(0,3);
        isBoarding[currentSeatNum] = currentIndex;
    }

    public void GetOffTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;

        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = string.Empty;
        isBoarding[currentSeatNum] = -1;
    }
}
