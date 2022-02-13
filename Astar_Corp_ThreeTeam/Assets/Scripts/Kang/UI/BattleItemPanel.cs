using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItemPanel : MonoBehaviour
{
    public List<BattleItem> itemBases;
    
    public void Init(PlayerableChar player)
    {
        var bags = player.characterStats.bag;

        var idx = 0;

        foreach (var itemBase in itemBases)
        {
            itemBase.gameObject.SetActive(false);
        }
        foreach (var pair in bags)
        {
            var item = ScriptableMgr.Instance.GetConsumable(pair.Key);
            itemBases[idx].gameObject.SetActive(true);
            itemBases[idx].Init(item);
            idx++;
        }
    }
}
