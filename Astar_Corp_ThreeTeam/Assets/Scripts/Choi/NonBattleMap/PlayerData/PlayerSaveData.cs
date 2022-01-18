using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    //저장할 데이터들.
    public List<int> bunkerKind { get; set; }
    public List<int> bunkerLevel { get; set; }

    //캐릭터 관련.
    public List<string> id { get; set; }
    public List<int> boarding { get; set; }
    public List<string> name { get; set; }
    public List<int> hp { get; set; }
    public List<int> maxHp { get; set; }
    public List<int> sensitivity { get; set; }
    public List<int> concentration { get; set; }
    public List<int> willPower { get; set; }

    //패널티.
    //"E", "B", "P", "I", "T" 
    public List<int> gaugeE { get; set; }
    public List<int> gaugeB { get; set; }
    public List<int> gaugeP { get; set; }
    public List<int> gaugeI { get; set; }
    public List<int> gaugeT { get; set; }

    public List<int> levelE { get; set; }
    public List<int> levelB { get; set; }
    public List<int> levelP { get; set; }
    public List<int> levelI { get; set; }
    public List<int> levelT { get; set; }

    //각 유저의 가방.
    public List<string> bagEquippableList { get; set; }
    public List<int> bagEquippableNumList { get; set; }
    public List<int> bagEquippableFirstIndex { get; set; }
    public List<int> bagEquippableLastIndex { get; set; }

    public List<string> bagConsumableList { get; set; }
    public List<int> bagConsumableNumList { get; set; }
    public List<int> bagConsumableFirstIndex { get; set; }
    public List<int> bagConsumableLastIndex { get; set; }

    //기타.
    public List<string> mainWeapon { get; set; }
    public List<string> subWeapon { get; set; }
  
    public List<string> activeSkillList { get; set; }
    public List<string> passiveSkillList { get; set; }

    //아지트 데이터.
    public List<string> equippableList { get; set; }
    public List<int> equippableNumList { get; set; }
    public List<string> consumableList { get; set; }
    public List<int> consumableNumList { get; set; }

    //트럭 데이터.
    public List<string> truckEquippableList { get; set; }
    public List<int> truckEquippableNumList { get; set; }
    public List<string> truckConsumableList { get; set; }
    public List<int> truckConsumableNumList { get; set; }
}
