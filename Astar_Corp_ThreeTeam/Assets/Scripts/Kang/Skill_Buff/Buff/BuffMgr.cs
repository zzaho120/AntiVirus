using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr
{
    public List<BuffBase> buffList = new List<BuffBase>();

    public void Addbuff(BuffBase buff)
    {
        buffList.Add(buff);
    }

    public bool CheckBuff(BuffBase check)
    {
        foreach (var buff in buffList)
        {
            if (buff.stat == check.stat)
                return false;
        }

        return true;
    }

    public List<BuffBase> GetBuffList(Stat statType)
    {
        var newBuffList = new List<BuffBase>();

        foreach (var buff in buffList)
        {
            if (buff.stat == statType)
                newBuffList.Add(buff); 
        }

        return newBuffList;
    }

    public void StartTurn()
    {
        var removeList = new List<BuffBase>();
        foreach (var buff in buffList)
        {
            buff.StartTurn();
            if (buff.IsOverLifeTurn)
                removeList.Add(buff);
        }

        RemoveBuff(removeList);
    }

    public void RemoveBuff(List<BuffBase> removeList)
    {
        foreach (var remove in removeList)
            RemoveBuff(remove);
    }

    public void RemoveBuff(BuffBase buff)
    {
        buffList.Remove(buff);
    }
}
