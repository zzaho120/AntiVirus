using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMonster : ScriptableObject
{
    public string id;
    public string prefabId;
    public new string name;

    public int speed;           // 이동속도
    public int suddenAtkRate;   // 기습 확률 (2초에 한번 검사)
    public int sightRange;      // 감지 거리
    public int areaRange;       // 영역 크기 (반지름)
    public int areaMaxNum;      // 영역 내 최대 젠 수
    public int createTime;      // 1마리 젠 쿨타임
    public int battleMinNum;    // 충돌 시 전투 최소 마리 수
    public int battleMaxNum;    // 충돌 시 전투 최대 마리 수
}
