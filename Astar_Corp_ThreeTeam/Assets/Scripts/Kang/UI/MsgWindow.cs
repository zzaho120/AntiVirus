using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MsgWindow : GenericWindow
{
    public TextMeshProUGUI msgText;

    public override void Open()
    {
        base.Open();
        Invoke("Close", 2f);
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetMsgText(string msg)
    {
        msgText.text = msg;
    }
}
