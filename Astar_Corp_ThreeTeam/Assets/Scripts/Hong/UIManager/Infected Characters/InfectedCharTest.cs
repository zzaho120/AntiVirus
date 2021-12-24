using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectedCharTest : MonoBehaviour
{
    public List<PlayerData> characterList = new List<PlayerData>();

    private Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            //Debug.Log(" 누가 먼저 ");
            
            buttons[0].GetComponentInChildren<Text>().text = characterList[0].characterStats.Name;
            buttons[1].GetComponentInChildren<Text>().text = characterList[1].characterStats.Name;
            buttons[2].GetComponentInChildren<Text>().text = characterList[2].characterStats.Name;
            buttons[3].GetComponentInChildren<Text>().text = characterList[3].characterStats.Name;
        }
    }
}

