using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 체력, 기력, 의지력

public class TestStatWindow : MonoBehaviour
{
    private Text[] text;
    public PlayerData playerData;

    private void Start()
    {
        text = GetComponentsInChildren<Text>();
        //playerData = playerData.GetComponent<PlayerData>();

        //text[0].text = playerData.characterStats.Name;
        //text[1].text = ("Level : " + character.characterInfo.level + "\n" +
        //                "HP : " + character.characterInfo.currentHp + "\n" +
        //                "Stamina : " + character.characterInfo.stamina + "\n" +
        //                "Willpower : " + character.characterInfo.Willpower).ToString();
    }

    private void Update()
    {
        text[0].text = playerData.characterStats.Name;
        text[1].text = ("Level : " + playerData.characterStats.level + "\n" +
                        "HP : " + playerData.characterStats.currentHp + "\n" +
                        "Stamina : " + string.Format("{0:0.#}", playerData.characterStats.stamina) + "\n" +
                        "Willpower : " + playerData.characterStats.willpower).ToString();
    }
}
