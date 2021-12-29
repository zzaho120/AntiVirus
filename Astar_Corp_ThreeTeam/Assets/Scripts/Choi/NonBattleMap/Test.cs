using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ScriptableMgr scriptableMgr;
    // Start is called before the first frame update
    void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;
        Debug.Log($"Test : {scriptableMgr.characterList.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
