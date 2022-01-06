using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattlePopUps : GenericWindow
{
	public override void Open()
	{
		base.Open();
	}

	public void Open(int value)
	{
		base.Open();

		//text = GetComponentsInChildren<Text>();
		//character = GetComponentInParent<InfectedCharTest>();
		//
		//PrintCharacterInfo(value);
	}

	public override void Close()
	{
		base.Close();
	}

	//public void OnNextWindow()
	//{
	//	base.OnNextWindow();
	//}

	//public WindowManager manager;

	public void StartDoubleWindow()
	{
		var windowId = (int)Windows.BunkerWindow - 1;
		var openWindow = manager.Open(windowId, false) as NonBattlePopUps;
	}
}
