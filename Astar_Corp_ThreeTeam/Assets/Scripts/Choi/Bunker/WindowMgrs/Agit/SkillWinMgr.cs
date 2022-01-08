using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWinMgr : MonoBehaviour
{ 
    public PlayerDataMgr playerDataMgr;

    // 리스트 순서 / PlayerDataMgr Key
    public Dictionary<int, int> characterInfo = new Dictionary<int, int>();
    public int currentIndex;


}
