using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectedCharTest : MonoBehaviour
{
    public List<PlayerData> characterList = new List<PlayerData>();
    private Button[] buttons;

    public GameObject squadUI;
    PlayerDataMgr playerDataMgr;
    Dictionary<int, string> currentSquad = new Dictionary<int, string>();
    GameObject[] warning = new GameObject[4];
    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        currentSquad = playerDataMgr.currentSquad;

       foreach(var element in currentSquad)
        {
            buttons[element.Key].GetComponentInChildren<Text>().text = element.Value;
        }

        for (int i = 0; i < 4; i++)
        {
            var child = squadUI.transform.GetChild(i).gameObject;
            warning[i] = child.transform.GetChild(1).gameObject;
        }

        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    //Debug.Log(" 누가 먼저 ");

        //    buttons[0].GetComponentInChildren<Text>().text = characterList[0].characterStats.Name;
        //    buttons[1].GetComponentInChildren<Text>().text = characterList[1].characterStats.Name;
        //    buttons[2].GetComponentInChildren<Text>().text = characterList[2].characterStats.Name;
        //    buttons[3].GetComponentInChildren<Text>().text = characterList[3].characterStats.Name;
        //}
    }

    public void TurnOffWarning()
    {
        for (int i = 0; i < warning.Length; i++)
        {
            warning[i].SetActive(false);
        }
    }

    public void TurnOnWarning(int i)
    {
        if (playerDataMgr.currentSquad.ContainsKey(i))
            warning[i].SetActive(true);
    }
}

