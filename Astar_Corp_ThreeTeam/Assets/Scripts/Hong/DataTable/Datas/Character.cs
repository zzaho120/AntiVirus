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
	public int min_Hp;				// �ּ� HP
	public int max_Hp;				// �ִ� HP
	public int min_Hp_Rise;			// ������ �ּ� ���� HP
	public int max_Hp_Rise;			// ������ �ִ� ���� HP

	// ����
	public int weight;				// �� �� �ִ� �ּ� ����
	public int weight_Rise;			// �������� �߰��Ǵ� ����

	// ���μ�? �ΰ���?
	public int min_Sensitivity;		// �ּ� ������
	public int max_Sensitivity;		// �ִ� ������
	public int min_Sen_Rise;		// ������ �ּ� ���� ������
	public int max_Sen_Rise;		// ������ �ִ� ���� ������

	// ȸ����
	public int min_Avoid_Rate;		// �ּ� ȸ����
	public int max_Avoid_Rate;		// �ִ� ȸ����

	// ���߷�
	public int min_Concentration;	// �ּ� ���߷�
	public int max_Concentration;	// �ִ� ���߷�
	public int min_Con_Rise;		// ������ �ּ� ���� ���߷�
	public int max_Con_Rise;		// ������ �ִ� ���� ���߷�

	// ���ŷ�
	public int min_Willpower;		// �ּ� ���ŷ�
	public int max_Willpower;		// �ִ� ���ŷ�
	public int min_Will_Rise;		// ������ �ּ� ���� ���ŷ�
	public int max_Will_Rise;		// ������ �ִ� ���� ���ŷ�

	// ġ��Ÿ Ȯ��
	public int min_Crit_Rate;		// �ּ� ũȮ
	public int max_Crit_Rate;		// �ִ� ũȮ

	// ����?
	public int min_Char_Cost;		// �뺴 �ּ� ���� ���
	public int max_Char_Cost;		// �뺴 �ִ� ���� ���

	// ������
	public int exp;					// ������ ����ġ �� (���� �� ����)
	public int resistance;          // ������ ���� ������ �� (���� �� ����)
	public int virusDec;			// ���� 1���� ���̷��� ������ ���ҷ�

	// ��밡�� ����
	public List<string> weapons = new List<string>();
}
