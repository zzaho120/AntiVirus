using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    //저장할 데이터들.
    public int money;
    public int bunkerExitNum;

    //벙커 관련.
    public List<int> bunkerKind { get; set; }
    public int agitLevel { get; set; }
    public int storageLevel { get; set; }
    public int garageLevel { get; set; }
    public int carCenterLevel { get; set; }
    public int hospitalLevel { get; set; }
    public int storeLevel { get; set; }
    public int pubLevel { get; set; }

    //상점 관련.
    public List<string> storeItem { get; set; }
    public List<int> storeItemNum { get; set; }
    public bool storeReset { get; set; }

    //차량 관련.
    public List<string> cars { get; set; }
    public List<int> speedLv { get; set; }
    public List<int> sightLv { get; set; }
    public List<int> weightLv { get; set; }
    public string currentCar;
    public List<int> boarding { get; set; }
    
    //캐릭터 관련.
    public List<string> id { get; set; }
    public List<string> name { get; set; }
    public List<int> hp { get; set; }
    public List<int> maxHp { get; set; }
    public List<int> sensitivity { get; set; }
    public List<int> concentration { get; set; }
    public List<int> willPower { get; set; }
    public List<int> bagLevel { get; set; }

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

    public List<string> bagOtherItemList { get; set; }
    public List<int> bagOtherItemNumList { get; set; }
    public List<int> bagOtherItemFirstIndex { get; set; }
    public List<int> bagOtherItemLastIndex { get; set; }

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
    public List<string> otherItemList { get; set; }
    public List<int> otherItemNumList { get; set; }

    //트럭 데이터.
    public List<string> truckEquippableList { get; set; }
    public List<int> truckEquippableNumList { get; set; }
    public List<string> truckConsumableList { get; set; }
    public List<int> truckConsumableNumList { get; set; }
    public List<string> truckOtherItemList { get; set; }
    public List<int> truckOtherItemNumList { get; set; }
}
