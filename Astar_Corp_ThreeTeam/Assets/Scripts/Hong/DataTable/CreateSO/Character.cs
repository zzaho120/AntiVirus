using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
	// 캐릭터 ID
	public string id;
	public string iconId;
	public string profileId;
	public string prefabId;

	// 기본 정보
	public string name;
	public string type;
	public string description;

	// HP
	public int min_Hp;
	public int max_Hp;
	public int min_Hp_Rise;
	public int max_Hp_Rise;

	// 무게
	public int weight;
	public int weight_Rise;

	// 예민성? 민감도?
	public int min_Sensitivity;
	public int max_Sensitivity;
	public int min_Sen_Rise;
	public int max_Sen_Rise;

	// 회피율
	public int min_Avoid_Rate;
	public int max_Avoid_Rate;

	// 집중력
	public int min_Concentration;
	public int max_Concentration;
	public int min_Con_Rise;
	public int max_Con_Rise;

	// 정신력
	public int min_Willpower;
	public int max_Willpower;
	public int min_Will_Rise;
	public int max_Will_Rise;

	// 치명타 확률
	public int min_Crit_Rate;
	public int max_Crit_Rate;

	// 가격?
	public int min_Char_Cost;
	public int max_Char_Cost;

	// 게이지
	public int exp;
	public int resistance;

	// 사용가능 무기
	public List<string> weapons = new List<string>();
}
