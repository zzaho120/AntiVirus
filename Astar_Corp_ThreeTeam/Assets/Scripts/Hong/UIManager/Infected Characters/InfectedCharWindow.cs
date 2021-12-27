using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectedCharWindow : GenericWindow
{
	private Text[] text;
	private InfectedCharTest character;

	PlayerDataMgr playerDataMgr;
	Dictionary<int, string> currentSquad = new Dictionary<int, string>();

    private void Start()
    {
		var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
		playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
	}

	public override void Open()
	{
		base.Open();
	}

	public void Open(int value)
	{
		base.Open();

		text = GetComponentsInChildren<Text>();
		character = GetComponentInParent<InfectedCharTest>();

		PrintCharacterInfo(value);
	}

    private void PrintCharacterInfo(int value)
    {
		if (playerDataMgr == null)
		{
			var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
			playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
			currentSquad = playerDataMgr.currentSquad;
		}

		if (!currentSquad.ContainsKey(value))
		{
			text[0].text = "이름";
			text[1].text = "정보";
		}
		else
		{
			text[0].text = currentSquad[value];
		}
		//if (value == 0)
  //      {
		//	text[0].text = character.characterList[0].characterStats.Name;
		//	text[1].text = ("HP : " + character.characterList[0].characterStats.currentHp + "\n" +
		//					"Damage : " + character.characterList[0].characterStats.Damage + "\n" +
		//					"Willpower : " + character.characterList[0].characterStats.willpower + "\n" +
		//					"Stamina : " + character.characterList[0].characterStats.stamina).ToString();

		//}
		//else if (value == 1)
		//{
		//	text[0].text = character.characterList[1].characterStats.Name;
		//	text[1].text = ("HP : " + character.characterList[1].characterStats.currentHp + "\n" +
		//					"Damage : " + character.characterList[1].characterStats.Damage + "\n" +
		//					"Willpower : " + character.characterList[1].characterStats.willpower + "\n" +
		//					"Stamina : " + character.characterList[1].characterStats.stamina).ToString();
		//}
		//else if (value == 2)
		//{
		//	text[0].text = character.characterList[2].characterStats.Name;
		//	text[1].text = ("HP : " + character.characterList[2].characterStats.currentHp + "\n" +
		//					"Damage : " + character.characterList[2].characterStats.Damage + "\n" +
		//					"Willpower : " + character.characterList[2].characterStats.willpower + "\n" +
		//					"Stamina : " + character.characterList[2].characterStats.stamina).ToString();
		//}
		//else if (value == 3)
		//{
		//	text[0].text = character.characterList[3].characterStats.Name;
		//	text[1].text = ("HP : " + character.characterList[3].characterStats.currentHp + "\n" +
		//					"Damage : " + character.characterList[3].characterStats.Damage + "\n" +
		//					"Willpower : " + character.characterList[3].characterStats.willpower + "\n" +
		//					"Stamina : " + character.characterList[3].characterStats.stamina).ToString();
		//}
		//else
  //      {
		//	Debug.LogError("Invalid value");
  //      }
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

	//public void StartDoubleWindow()
	//{
	//	//var windowId = (int)Windows.Window6 - 1;
	//	//var openWindow = manager.Open(windowId, false) as WindowDefaultSetting;
	//}
}
