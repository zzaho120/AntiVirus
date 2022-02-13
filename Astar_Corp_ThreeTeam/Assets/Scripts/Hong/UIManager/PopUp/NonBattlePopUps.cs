using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattlePopUps : GenericWindow
{

	// [수정] 시간 남으면.. 버튼 눌렸을 때 맵 정지되도록
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

	// 맵 Pause 상태 해제
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
