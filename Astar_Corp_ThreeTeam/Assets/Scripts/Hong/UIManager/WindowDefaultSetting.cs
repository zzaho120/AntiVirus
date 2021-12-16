using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDefaultSetting : GenericWindow
{
	public override void Open()
	{
		base.Open();
	}

	public override void Close()
	{
		base.Close();
	}

	public void OnNext()
	{
		base.OnNextWindow();
	}

	public WindowManager manager;

	public void StartDoubleWindow()
	{
		var windowId = (int)Windows.Window6 - 1;
		var openWindow = manager.Open(windowId, false) as WindowDefaultSetting;
	}
}
