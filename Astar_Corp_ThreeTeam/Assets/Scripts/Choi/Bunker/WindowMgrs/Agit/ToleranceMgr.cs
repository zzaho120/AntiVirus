using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class ToleranceMgr : MonoBehaviour
{
    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;

    public PlayerDataMgr playerDataMgr;
    public GameObject toleranceWin;

    public Text levelTxt;
    public Text nameTxt;
    public Text expTxt;

    [Header("������")]
    public Text hpTxt;
    public Text healthTxt;
    public Text weightTxt;
    public Text sensitiveTxt;
    public Text concentrationTxt;
    public Text mentalityTxt;
    public Text accuracyTxt;
    public Text criticalTxt;
    public Text evasionTxt;

    [Header("���̷�����")]
    public Slider eVirusBar;
    public Slider bVirusBar;
    public Slider pVirusBar;
    public Slider iVirusBar;
    public Slider tVirusBar;
    public Text eVirusTxt;
    public Text bVirusTxt;
    public Text pVirusTxt;
    public Text iVirusTxt;
    public Text tVirusTxt;

    [Header("������")]
    public Slider eResistanceBar;
    public Slider bResistanceBar;
    public Slider pResistanceBar;
    public Slider iResistanceBar;
    public Slider tResistanceBar;
    public Text eResistanceTxt;
    public Text bResistanceTxt;
    public Text pResistanceTxt;
    public Text iResistanceTxt;
    public Text tResistanceTxt;

    public int currentIndex;

    public void Refresh()
    {
        if (currentIndex == -1) return;
        var stat = playerDataMgr.currentSquad[currentIndex];

        //var eVirusLevel = stat.virusPenalty["E"].penaltyLevel;
        //var bVirusLevel = stat.virusPenalty["B"].penaltyLevel;
        //var pVirusLevel = stat.virusPenalty["P"].penaltyLevel;
        //var iVirusLevel = stat.virusPenalty["I"].penaltyLevel;
        //var tVirusLevel = stat.virusPenalty["T"].penaltyLevel;

        //Stats stats = new Stats(eVirusLevel, bVirusLevel, pVirusLevel, iVirusLevel, tVirusLevel);
        //uiStatsRadarChart.SetStats(stats);
        levelTxt.text = $"{stat.level}";
        nameTxt.text = $"{stat.character.name}";
        expTxt.text = $"XP: {stat.currentExp}/\n       {stat.totalExp}";

        //�� ����.
        hpTxt.text = $"{stat.currentHp} / {stat.MaxHp}";
        healthTxt.text = $"{stat.currentHp}";
        weightTxt.text = $"{stat.Weight}";
        sensitiveTxt.text = $"{stat.sensivity}";
        concentrationTxt.text = $"{stat.concentration}";
        mentalityTxt.text = $"{stat.willpower}";
        accuracyTxt.text = $"{stat.accuracy}";
        criticalTxt.text = $"{stat.critRate}";
        evasionTxt.text = $"{stat.avoidRate}";

    //��.
        eVirusBar.maxValue = stat.virusPenalty["E"].GetMaxGauge();
        eVirusBar.value = stat.virusPenalty["E"].penaltyGauge;
        eVirusTxt.text = 
            $"LV{stat.virusPenalty["E"].penaltyLevel} ({stat.virusPenalty["E"].penaltyGauge}/{stat.virusPenalty["E"].GetMaxGauge()})";

        eResistanceBar.maxValue = stat.virusPenalty["E"].GetMaxReductionGauge();
        eResistanceBar.value = stat.virusPenalty["E"].resistGauge;
        eResistanceTxt.text =
            $"LV{stat.virusPenalty["E"].resistLevel} ({stat.virusPenalty["E"].resistGauge}/{stat.virusPenalty["E"].GetMaxReductionGauge()})";

        bVirusBar.maxValue = stat.virusPenalty["B"].GetMaxGauge();
        bVirusBar.value = stat.virusPenalty["B"].penaltyGauge; 
        bVirusTxt.text =
            $"LV{stat.virusPenalty["B"].penaltyLevel} ({stat.virusPenalty["B"].penaltyGauge}/{stat.virusPenalty["B"].GetMaxGauge()})";

        bResistanceBar.maxValue = stat.virusPenalty["B"].GetMaxReductionGauge();
        bResistanceBar.value = stat.virusPenalty["B"].resistGauge;
        bResistanceTxt.text =
           $"LV{stat.virusPenalty["B"].resistLevel} ({stat.virusPenalty["B"].resistGauge}/{stat.virusPenalty["B"].GetMaxReductionGauge()})";

        pVirusBar.maxValue = stat.virusPenalty["P"].GetMaxGauge();
        pVirusBar.value = stat.virusPenalty["P"].penaltyGauge;
        pVirusTxt.text =
           $"LV{stat.virusPenalty["P"].penaltyLevel} ({stat.virusPenalty["P"].penaltyGauge}/{stat.virusPenalty["P"].GetMaxGauge()})";

        pResistanceBar.maxValue = stat.virusPenalty["P"].GetMaxReductionGauge();
        pResistanceBar.value = stat.virusPenalty["P"].resistGauge;
        pResistanceTxt.text =
           $"LV{stat.virusPenalty["P"].resistLevel} ({stat.virusPenalty["P"].resistGauge}/{stat.virusPenalty["P"].GetMaxReductionGauge()})";

        iVirusBar.maxValue = stat.virusPenalty["I"].GetMaxGauge();
        iVirusBar.value = stat.virusPenalty["I"].penaltyGauge;
        iVirusTxt.text =
          $"LV{stat.virusPenalty["I"].penaltyLevel} ({stat.virusPenalty["I"].penaltyGauge}/{stat.virusPenalty["I"].GetMaxGauge()})";

        iResistanceBar.maxValue = stat.virusPenalty["I"].GetMaxReductionGauge();
        iResistanceBar.value = stat.virusPenalty["I"].resistGauge;
        iResistanceTxt.text =
          $"LV{stat.virusPenalty["I"].resistLevel} ({stat.virusPenalty["I"].resistGauge}/{stat.virusPenalty["I"].GetMaxReductionGauge()})";

        tVirusBar.maxValue = stat.virusPenalty["T"].GetMaxGauge();
        tVirusBar.value = stat.virusPenalty["T"].penaltyGauge;
        tVirusTxt.text =
         $"LV{stat.virusPenalty["T"].penaltyLevel} ({stat.virusPenalty["T"].penaltyGauge}/{stat.virusPenalty["T"].GetMaxGauge()})";

        tResistanceBar.maxValue = stat.virusPenalty["T"].GetMaxReductionGauge();
        tResistanceBar.value = stat.virusPenalty["T"].resistGauge;
        tResistanceTxt.text =
          $"LV{stat.virusPenalty["T"].resistLevel} ({stat.virusPenalty["T"].resistGauge}/{stat.virusPenalty["T"].GetMaxReductionGauge()})";
    }
}
