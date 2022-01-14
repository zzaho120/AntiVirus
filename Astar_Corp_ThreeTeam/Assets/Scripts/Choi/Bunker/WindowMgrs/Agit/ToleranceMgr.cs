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

        var eVirusLevel = stat.virusPanalty["E"].penaltyLevel;
        var bVirusLevel = stat.virusPanalty["B"].penaltyLevel;
        var pVirusLevel = stat.virusPanalty["P"].penaltyLevel;
        var iVirusLevel = stat.virusPanalty["I"].penaltyLevel;
        var tVirusLevel = stat.virusPanalty["T"].penaltyLevel;

        //�׽�Ʈ��.
        //eVirusLevel = 3;
        //bVirusLevel = 4;
        //pVirusLevel = 1;
        //iVirusLevel = 5;
        //tVirusLevel = 0;

        Stats stats = new Stats(eVirusLevel, bVirusLevel, pVirusLevel, iVirusLevel, tVirusLevel);
        uiStatsRadarChart.SetStats(stats);

        //���� ����Txt.
        eVirusTxt.text = $"���� ���� ���� {stat.virusPanalty["E"].penaltyLevel}";
        bVirusTxt.text = $"���� ���� ���� {stat.virusPanalty["B"].penaltyLevel}";
        pVirusTxt.text = $"���� ���� ���� {stat.virusPanalty["P"].penaltyLevel}";
        iVirusTxt.text = $"���� ���� ���� {stat.virusPanalty["I"].penaltyLevel}";
        tVirusTxt.text = $"���� ���� ���� {stat.virusPanalty["T"].penaltyLevel}";

        //���̷��� ��.
        eVirusBar.value = (float)stat.virusPanalty["E"].penaltyGauge / stat.virusPanalty["E"].GetMaxGauge();
        bVirusBar.value = (float)stat.virusPanalty["B"].penaltyGauge / stat.virusPanalty["E"].GetMaxGauge();
        pVirusBar.value = (float)stat.virusPanalty["P"].penaltyGauge / stat.virusPanalty["E"].GetMaxGauge();
        iVirusBar.value = (float)stat.virusPanalty["I"].penaltyGauge / stat.virusPanalty["E"].GetMaxGauge();
        tVirusBar.value = (float)stat.virusPanalty["T"].penaltyGauge / stat.virusPanalty["E"].GetMaxGauge();
    }
}
