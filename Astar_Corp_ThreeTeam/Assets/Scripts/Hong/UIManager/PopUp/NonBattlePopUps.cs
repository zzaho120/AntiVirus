using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattlePopUps : GenericWindow
{

	// [����] �ð� ������.. ��ư ������ �� �� �����ǵ���
	//TimeController timeController;
	//
    //private void Start()
    //{
	//	timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    //}

    public override void Open()
	{
		base.Open();
	}

	// �� Pause ���� ����
	//public void StartMapTimer()
    //{
	//	timeController.PauseTime();
	//	timeController.isPause = false;
	//}

	public void Open(int value)
	{
		base.Open();
	}

	public override void Close()
	{
		base.Close();
	}

	public void StartDoubleWindow()
	{
		var windowId = (int)Windows.BunkerWindow - 1;
		var openWindow = manager.Open(windowId, false) as NonBattlePopUps;
	}
}
