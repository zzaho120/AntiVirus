using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 체력, 기력, 의지력

public class TestStatWindow : MonoBehaviour
{
    private Text[] text;
    public PlayerData character;

    private void Start()
    {
        text = GetComponentsInChildren<Text>();
        character = character.GetComponent<PlayerData>();

        text[0].text = character.characterInfo.Name;
        //text[1].text = ("Level : " + character.characterInfo.level + "\n" +
        //                "HP : " + character.characterInfo.currentHp + "\n" +
        //                "Stamina : " + character.characterInfo.stamina + "\n" +
        //                "Willpower : " + character.characterInfo.Willpower).ToString();
    }

    private void Update()
    {
        text[0].text = character.characterInfo.Name;
        text[1].text = ("Level : " + character.characterInfo.level + "\n" +
                        "HP : " + character.characterInfo.currentHp + "\n" +
                        "Stamina : " + string.Format("{0:0.#}", character.characterInfo.stamina) + "\n" +
                        "Willpower : " + character.characterInfo.willpower).ToString();
    }
}
