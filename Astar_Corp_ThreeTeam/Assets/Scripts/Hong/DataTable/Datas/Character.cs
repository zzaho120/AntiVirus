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

	// HP
	public int minHp;				// �ּ� HP
	public int maxHp;				// �ִ� HP
	public int hpChance;			// ���� �� ���õ� Ȯ��
	public int hpRise;				// ���õ� �� �����ϴ� ü��

	// ����
	public int weight;				// �� �� �ִ� �ּ� ����
	public int weight_Rise;         // ���õ� �� �����ϴ� ����

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
	public int avoidRateRisePerSen;	// 1����Ʈ�� ȸ���� ������ (������ 1����Ʈ�� �����ϴ� ȸ���� ���� �̾߱��ε�)
	// 10, 20 ����Ʈ �� �����ϴ� �þ� ���� +1
	// 15, 25 ����Ʈ �� �����ϴ� MP�� +1

	// ���߷�
	public int minConcentration;	// �ּ� ���߷�
	public int maxConcentration;	// �ִ� ���߷�
	public int concentrationChance;	// ���� �� ���õ� Ȯ��
	public int concentrationRise;	// ���õ� �� �����ϴ� ���� ��ġ
	public int accurRatePerCon;		// ���߷� 1����Ʈ�� ���߷� ������

	// ���ŷ�
	public int minWillpower;		// �ּ� ���ŷ�
	public int maxWillpower;		// �ִ� ���ŷ�
	public int willChance;			// ���� �� ���õ� Ȯ��
	public int willRise;            // ���õ� �� �����ϴ� ��ġ
	public int alertAccurRateRise;	// 3����Ʈ�� �����߷� ������

	// ġ��Ÿ Ȯ��
	// = �� ũ��Ƽ�� Ȯ�� - ��� ũ��Ƽ�� ���׷�
	public int critRateRise;		// ũ��Ƽ�� Ȯ�� ����
	public int critResistRateRise;	// ũ��Ƽ�� ���׷� ����

	// ���� ���?
	public int minCharCost;			// �뺴 �ּ� ���� ���
	public int maxCharCost;			// �뺴 �ִ� ���� ���

	// ������
	public int exp;					// ������ ����ġ �� (���� �� ����)
	public int resistGauge;         // ������ ���� ������ �� (���� �� ����)
	public int virusDec_Lev0;		// ���� 0���� �� ���̷��� ������ ���ҷ�
	public int virusDec_Lev1;		// ���� 1���� ���̷��� ������ ���ҷ� (���� �� ����)

	// ��밡�� ����
	public List<string> weapons = new List<string>();
	public List<string> skillA = new List<string>();
	public List<string> skillB = new List<string>();
	public List<string> skillC = new List<string>();
}
