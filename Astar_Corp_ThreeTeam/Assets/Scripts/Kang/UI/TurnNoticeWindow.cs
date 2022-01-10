using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnNoticeWindow : GenericWindow
{
    public TextMeshProUGUI noticeText;
    private float maxTime = 2f;

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void NoticeTurn(BattleTurn turn)
    {
        switch (turn)
        {
            case BattleTurn.Player:
                noticeText.text = "Player Turn";
                break;
            case BattleTurn.Enemy:
                noticeText.text = "Enemy Turn";
                break;
        }
        Invoke("Close", maxTime);
    }
}
