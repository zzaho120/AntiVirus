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
}
