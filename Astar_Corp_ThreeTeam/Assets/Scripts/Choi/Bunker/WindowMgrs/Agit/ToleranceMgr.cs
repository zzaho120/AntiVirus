using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToleranceMgr : MonoBehaviour
{
    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;

    public PlayerDataMgr playerDataMgr;
    public GameObject toleranceWin;

    public Text eVirusTxt;
    public Text bVirusTxt;
    public Text pVirusTxt;
    public Text iVirusTxt;
    public Text tVirusTxt;

    public Slider eVirusBar;
    public Slider bVirusBar;
    public Slider pVirusBar;
    public Slider iVirusBar;
    public Slider tVirusBar;

    public int currentIndex;

    public void Refresh()
    {
        if (currentIndex == -1) return;
        var stat = playerDataMgr.currentSquad[currentIndex];

        var eVirusLevel = stat.virusPenalty["E"].penaltyLevel;
        var bVirusLevel = stat.virusPenalty["B"].penaltyLevel;
        var pVirusLevel = stat.virusPenalty["P"].penaltyLevel;
        var iVirusLevel = stat.virusPenalty["I"].penaltyLevel;
        var tVirusLevel = stat.virusPenalty["T"].penaltyLevel;

        //테스트용.
        //eVirusLevel = 3;
        //bVirusLevel = 4;
        //pVirusLevel = 1;
        //iVirusLevel = 5;
        //tVirusLevel = 0;

        Stats stats = new Stats(eVirusLevel, bVirusLevel, pVirusLevel, iVirusLevel, tVirusLevel);
        uiStatsRadarChart.SetStats(stats);

        //현재 레벨Txt.
        eVirusTxt.text = $"현재 내성 레벨 {stat.virusPenalty["E"].penaltyLevel}";
        bVirusTxt.text = $"현재 내성 레벨 {stat.virusPenalty["B"].penaltyLevel}";
        pVirusTxt.text = $"현재 내성 레벨 {stat.virusPenalty["P"].penaltyLevel}";
        iVirusTxt.text = $"현재 내성 레벨 {stat.virusPenalty["I"].penaltyLevel}";
        tVirusTxt.text = $"현재 내성 레벨 {stat.virusPenalty["T"].penaltyLevel}";

        //바이러스 바.
        eVirusBar.value = (float)stat.virusPenalty["E"].penaltyGauge / stat.virusPenalty["E"].GetMaxGauge();
        bVirusBar.value = (float)stat.virusPenalty["B"].penaltyGauge / stat.virusPenalty["E"].GetMaxGauge();
        pVirusBar.value = (float)stat.virusPenalty["P"].penaltyGauge / stat.virusPenalty["E"].GetMaxGauge();
        iVirusBar.value = (float)stat.virusPenalty["I"].penaltyGauge / stat.virusPenalty["E"].GetMaxGauge();
        tVirusBar.value = (float)stat.virusPenalty["T"].penaltyGauge / stat.virusPenalty["E"].GetMaxGauge();
    }
}
