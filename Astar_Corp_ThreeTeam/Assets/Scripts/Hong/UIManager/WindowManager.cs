using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour
{
	[HideInInspector]	
	public GenericWindow[] windows;		// Generic Windows ��� ����

	public int currentWindowID;
	public int defaultWindowID;

	public GenericWindow GetWindow(int value)
	{
		return windows[value];
	}

	private void ToggleVisability(int value, bool closeAllOpen = true)
	{
		var total = windows.Length;

		// ���� �ִ� �� �ݰ� �Ѿ��
		if (closeAllOpen)
		{
			for (var i = 0; i < total; i++)
			{
				var window = windows[i];
				if (window.gameObject.activeSelf)
					window.Close();
			}
		}
		// false�� �߰� �۾� ���� �߰��� ����
		GetWindow(value).Open();
	}

	// WindowManager.Open --> (int value, bool open)
	// GenericWindow.Open --> (x)
	// ��� : windowManager.Open(windowId, false) as BattleWindow;
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
