using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    //저장할 데이터들.
    //캐릭터 관련.
    public List<string> id { get; set; }
    public List<string> name { get; set; }
    public List<int> hp { get; set; }
    public List<int> sensitivity { get; set; }
    public List<int> concentration { get; set; }
    public List<int> willPower { get; set; }

    //기타.
    public List<string> antivirus { get; set; }
    public List<string> mainWeapon { get; set; }
    public List<int> mainWeaponNum { get; set; }
    public List<string> subWeapon { get; set; }
    public List<int> subWeaponNum { get; set; }

    public List<string> activeSkillList { get; set; }
    public List<string> passiveSkillList { get; set; }

    public List<string> equippableList { get; set; }
    public List<int> equippableNumList { get; set; }
    public List<string> consumableList { get; set; }
    public List<int> consumableNumList { get; set; }
}
