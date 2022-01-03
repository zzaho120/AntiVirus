using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
	// ĳ���� ID
	public string id;
	public string iconId;
	public string profileId;
	public string prefabId;

	// �⺻ ����
	public string name;
	public string type;
	public string description;

	// HP
	public int min_Hp;
	public int max_Hp;
	public int min_Hp_Rise;
	public int max_Hp_Rise;

	// ����
	public int weight;
	public int weight_Rise;

	// ���μ�? �ΰ���?
	public int min_Sensitivity;
	public int max_Sensitivity;
	public int min_Sen_Rise;
	public int max_Sen_Rise;

	// ȸ����
	public int min_Avoid_Rate;
	public int max_Avoid_Rate;

	// ���߷�
	public int min_Concentration;
	public int max_Concentration;
	public int min_Con_Rise;
	public int max_Con_Rise;

	// ���ŷ�
	public int min_Willpower;
	public int max_Willpower;
	public int min_Will_Rise;
	public int max_Will_Rise;

	// ġ��Ÿ Ȯ��
	public int min_Crit_Rate;
	public int max_Crit_Rate;

	// ����?
	public int min_Char_Cost;
	public int max_Char_Cost;

	// ������
	public int exp;
	public int resistance;

	// ��밡�� ����
	public List<string> weapons = new List<string>();
}
