using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTableBase
{
    protected string csvFilePath = string.Empty;

    protected Dictionary<string, DataTableElemBase> data = new Dictionary<string, DataTableElemBase>();

    public int Count { get => data.Count; }

    public abstract void Load();
    
    public T GetData<T>(string ID) where T : DataTableElemBase
    {
        if (!data.ContainsKey(ID))
            return null;

        return data[ID] as T; ;
    }
}