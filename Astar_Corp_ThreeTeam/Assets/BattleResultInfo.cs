using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultInfo : MonoBehaviour
{
    public Text levelText;
    public Slider expSlider;
    public Text expText;
    public Image characterIcon;
    public Text className;
    public Text characterName;
    public Text hp;
    public Slider hpBar;
    public List<GameObject> virusList;
    public List<Text> resistList;
    public void Init(PlayerableChar player, string name)
    {
        var stats = player.characterStats;
        levelText.text = $"LV{stats.level}";
        expSlider.maxValue = stats.totalExp;
        expSlider.value = stats.currentExp;
        expText.text = $"{stats.currentExp}/{stats.totalExp}";
        characterIcon.sprite = stats.character.icon;

        string classStr = string.Empty;
        switch (stats.character.name)
        {
            case "Bombardier":
                classStr = "중화기병";
                break;
            case "Scout":
                classStr = "정찰병";
                break;
            case "Sniper":
                classStr = "저격병";
                break;
            case "Healer":
                classStr = "보조병";
                break;
            case "Tanker":
                classStr = "돌격병";
                break;
        }


        className.text = classStr;
        characterName.text = name;
        hp.text = $"{stats.currentHp}/{stats.MaxHp}";
        hpBar.maxValue = stats.MaxHp;
        hpBar.value = stats.currentHp;

        string[] virusName = new string[5];
        virusName[0] = "E";
        virusName[1] = "B";
        virusName[2] = "P";
        virusName[3] = "I";
        virusName[4] = "T";

        for (var idx = 0; idx < virusName.Length; ++idx)
        {
            virusList[idx].SetActive(stats.virusPenalty[virusName[idx]].penaltyLevel >= 1);
        }

        for (var idx = 0; idx < virusName.Length; ++idx)
        {
            resistList[idx].text
                = $"Lv {stats.virusPenalty[virusName[idx]].resistLevel}";
        }
    }
}
