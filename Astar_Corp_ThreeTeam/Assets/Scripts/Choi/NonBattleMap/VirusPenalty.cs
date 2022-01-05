using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusPenalty
{
    //T 바이러스
    //int 게이지
    //int 패널티 레벨

    public int gauge;
    public int level;

    Dictionary<int, int> levelGauge = new Dictionary<int, int>();
    Dictionary<int, int> gaugeIncrease = new Dictionary<int, int>();
    Dictionary<int, int> gaugeReduction = new Dictionary<int, int>();

    public VirusPenalty()
    {
        gauge = 0;
        level = 1;

        for (int i = 1; i < 6; i++) levelGauge.Add(1 * i, 100 * i);

        gaugeIncrease.Add(1, 30);
        gaugeIncrease.Add(2, 30);
        gaugeIncrease.Add(3, 30);
        gaugeIncrease.Add(4, 30);
        gaugeIncrease.Add(5, 60);

        gaugeReduction.Add(1, 20);
        gaugeReduction.Add(2, 20);
        gaugeReduction.Add(3, 20);
        gaugeReduction.Add(4, 20);
        gaugeReduction.Add(5, 40);
    }

    public void Calculation(int level)
    {
        gauge += (gaugeIncrease[level] - gaugeReduction[level]);

        if (level != 5 && gauge >= levelGauge[level])
        {
            gauge -= levelGauge[level];
            this.level++;
        }
        else if (level == 5 && gauge >= levelGauge[level])
        {
            gauge = levelGauge[level];
        }
    }
}
