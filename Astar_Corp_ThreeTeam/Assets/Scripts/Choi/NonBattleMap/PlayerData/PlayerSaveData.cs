using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    //저장할 데이터들.
    public List<string> ids { get; set; }
    public List<string> names { get; set; }
    public List<int> hps { get; set; }
    public List<int> offensePowers { get; set; }
    public List<int> willPowers { get; set; }
    public List<int> staminas { get; set; }
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
