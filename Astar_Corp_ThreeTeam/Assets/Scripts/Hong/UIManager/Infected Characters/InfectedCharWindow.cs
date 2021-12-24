using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectedCharWindow : GenericWindow
{
	private Text[] text;
	private InfectedCharTest character;

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
		if (value == 0)
        {
			text[0].text = character.characterList[0].characterInfo.Name;
			text[1].text = ("HP : " + character.characterList[0].characterInfo.currentHp + "\n" +
							"Damage : " + character.characterList[0].characterInfo.Damage + "\n" +
							"Willpower : " + character.characterList[0].characterInfo.willpower + "\n" +
							"Stamina : " + character.characterList[0].characterInfo.stamina).ToString();

		}
		else if (value == 1)
		{
			text[0].text = character.characterList[1].characterInfo.Name;
			text[1].text = ("HP : " + character.characterList[1].characterInfo.currentHp + "\n" +
							"Damage : " + character.characterList[1].characterInfo.Damage + "\n" +
							"Willpower : " + character.characterList[1].characterInfo.willpower + "\n" +
							"Stamina : " + character.characterList[1].characterInfo.stamina).ToString();
		}
		else if (value == 2)
		{
			text[0].text = character.characterList[2].characterInfo.Name;
			text[1].text = ("HP : " + character.characterList[2].characterInfo.currentHp + "\n" +
							"Damage : " + character.characterList[2].characterInfo.Damage + "\n" +
							"Willpower : " + character.characterList[2].characterInfo.willpower + "\n" +
							"Stamina : " + character.characterList[2].characterInfo.stamina).ToString();
		}
		else if (value == 3)
		{
			text[0].text = character.characterList[3].characterInfo.Name;
			text[1].text = ("HP : " + character.characterList[3].characterInfo.currentHp + "\n" +
							"Damage : " + character.characterList[3].characterInfo.Damage + "\n" +
							"Willpower : " + character.characterList[3].characterInfo.willpower + "\n" +
							"Stamina : " + character.characterList[3].characterInfo.stamina).ToString();
		}
		else
        {
			Debug.LogError("Invalid value");
        }
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
