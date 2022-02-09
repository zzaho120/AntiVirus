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
	public new string name;
	public string type;
	public string description;

	// �ǰ�
	public int minHealth;			// �ּ� �ǰ�
	public int maxHealth;			// �ִ� �ǰ�
	public int healthChance;		// ���� �� ���õ� Ȯ��
	public int helathRise;			// ���õ� �� �����ϴ� �ǰ�
	public int critResistRateRise;	// (�ǰ� 2����Ʈ��) ũ��Ƽ�� ���׷� ����

	// HP
	public int minHp;				// �ּ� hp
	public int maxHp;               // �ִ� hp
	public int hpRise;              // ���õ� �� �����ϴ� hp

	// ���� �ʰ� �� �г�Ƽ
	public int mpPenalty1;			// 100 ~ 150 % �ʰ�
	public int mpPenalty2;			// 150 ~ 200 % �ʰ�
	public int mpPenalty3;			// 200% ~ �ʰ�

	// ������
	public int minSensitivity;		// �ּ� ������
	public int maxSensitivity;		// �ִ� ������
	public int senChance;			// ���� �� ���õ� Ȯ��
	public int senRise;				// ���õ� �� �����ϴ� ���� ��ġ

	// ȸ����
	public int minAvoidRate;		// �ּ� ȸ����
	public int maxAvoidRate;		// �ִ� ȸ����
	public int avoidRateRisePerSen;	// 1����Ʈ�� ȸ���� ������ (������ 1����Ʈ�� �����ϴ� ȸ���� ����)
	// 10, 20 ����Ʈ �� �����ϴ� MP�� +1

	// ���߷�
	public int minConcentration;	// �ּ� ���߷�
	public int maxConcentration;	// �ִ� ���߷�
	public int concentrationChance;	// ���� �� ���õ� Ȯ��
	public int concentrationRise;	// ���õ� �� �����ϴ� ���� ��ġ
	public int accurRatePerCon;     // ���߷� 1����Ʈ�� ���߷� ������
	// 10, 20 ����Ʈ �� �����ϴ� �þ� ���� +1

	// ���ŷ�
	public int minWillpower;		// �ּ� ���ŷ�
	public int maxWillpower;		// �ִ� ���ŷ�
	public int willChance;			// ���� �� ���õ� Ȯ��
	public int willRise;            // ���õ� �� �����ϴ� ��ġ

	public int firePenaltyDec;		// 2����Ʈ�� ���ӻ�� �г�Ƽ ����
	public int critRateRise;        // 3����Ʈ�� ũȮ ����
	
	// ġ��Ÿ Ȯ��
	// = �� ũ��Ƽ�� Ȯ�� - ��� ũ��Ƽ�� ���׷�

	//public int alertAccurRateRise;	// 3����Ʈ�� �����߷� ������ --> ������

	// ���� ���?
	public int minCharCost;			// �뺴 �ּ� ���� ���
	public int maxCharCost;			// �뺴 �ִ� ���� ���

	// ������
	//public int resistGauge;        // ������ ���� ������ ��	--> DB ���� ����
	public int virusDec_Lev0;		// ���� 0���� �� ���̷��� ������ ���ҷ�
	public int virusDec_Lev1;		// ���� 1���� ���̷��� ������ ���ҷ� (���� �� ����)

	// ��밡�� ����
	public List<string> weapons = new List<string>();

	// ���� ��ų ����Ʈ
	public List<string> skillA = new List<string>();
	public List<string> skillB = new List<string>();
	public List<string> skillC = new List<string>();
	public List<string> skillD = new List<string>();

	public Sprite fullImg;
	public Sprite halfImg;
	public Sprite icon;
}
