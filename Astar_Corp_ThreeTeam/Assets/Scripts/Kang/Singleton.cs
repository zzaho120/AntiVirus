using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> :
    MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (FindObjectOfType(typeof(T)))
                return instance;

            if (instance == null)
            {
                GameObject obj;
                var prefab = Resources.Load($"Prefabs/Singleton/{typeof(T).Name}") as GameObject;
                if (prefab != null)
                {
                    obj = Instantiate(prefab);
                    instance = obj.GetComponent<T>();
                    obj.name = $"Singleton {obj.name}";
                }
                else
                {
                    obj = new GameObject();
                    obj.name = $"Singleton {typeof(T).Name}";
                }

                if (instance == null)
                    instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}