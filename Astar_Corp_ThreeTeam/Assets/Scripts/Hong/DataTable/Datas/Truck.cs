using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : ScriptableObject
{
	public string id;
	public string name;

	// ���� �ο�
	public int capacity;

	// �ӵ�
	public int speed;
	public int speed_Rise;
	public int speedUp_Cost;

	// �þ�
	public int sight;
	public int sight_Rise;
	public int sightUp_Cost;

	// Ʈ��ũ(Ʈ�� �κ��丮) ����
	public int weight;
	public int weight_Rise;
	public int weightUp_Cost;

	// ����
	public int price;
}
