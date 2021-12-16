using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour
{
	[HideInInspector]	
	public GenericWindow[] windows;		// Generic Windows 담고 있음

	public int currentWindowID;
	public int defaultWindowID;

	public GenericWindow GetWindow(int value)
	{
		return windows[value];
	}

	private void ToggleVisability(int value, bool closeAllOpen = true)
	{
		var total = windows.Length;

		// 열려 있는 거 닫고 넘어가기
		if (closeAllOpen)
		{
			for (var i = 0; i < total; i++)
			{
				var window = windows[i];
				if (window.gameObject.activeSelf)
					window.Close();
			}
		}
		// false면 추가 작업 없이 추가로 열기
		GetWindow(value).Open();
	}

	// WindowManager.Open --> (int value, bool open)
	// GenericWindow.Open --> (x)
	// 사용 : windowManager.Open(windowId, false) as BattleWindow;
	public GenericWindow Open(int value, bool closeAllOpen = true)
	{
		if (value < 0 || value >= windows.Length)
			return null;

		currentWindowID = value;

		ToggleVisability(currentWindowID, closeAllOpen);

		return GetWindow(currentWindowID);
	}

	void Start()
	{
		GenericWindow.manager = this;
		Open(defaultWindowID);
	}


}
