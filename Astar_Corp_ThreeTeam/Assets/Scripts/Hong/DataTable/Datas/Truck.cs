using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : ScriptableObject
{
	public string id;
	public string name;

	// 수용 인원
	public int capacity;

	// 속도
	public int speed;
	public int speed_Rise;
	public int speedUp_Cost;

	// 시야
	public int sight;
	public int sight_Rise;
	public int sightUp_Cost;

	// 트렁크(트럭 인벤토리) 무게
	public int weight;
	public int weight_Rise;
	public int weightUp_Cost;

	// 가격
	public int price;
}
