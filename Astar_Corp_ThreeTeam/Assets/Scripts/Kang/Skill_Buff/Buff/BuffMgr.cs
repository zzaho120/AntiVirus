using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr
{
    public List<BuffBase> buffList;

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
}
