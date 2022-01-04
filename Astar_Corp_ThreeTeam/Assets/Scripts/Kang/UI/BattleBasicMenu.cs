using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBasicMenu : GenericWindow
{
    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void OnClickStartTurn()
    {
        EventBusMgr.Publish(EventType.StartTurn);
    }
}
