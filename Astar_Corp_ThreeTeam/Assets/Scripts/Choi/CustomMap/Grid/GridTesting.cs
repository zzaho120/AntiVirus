using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CustomMapMode
{ 
    None,
    Create,
    Find
}

public class GridTesting : MonoBehaviour
{
    public string selectedStr;
    public string findStr;
    public CustomMapMode mode;

    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        selectedStr = null;
        findStr = null;
        mode = CustomMapMode.Create;

       
        grid = new Grid(10, 20, 10f,Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedStr != null && mode == CustomMapMode.Create)
                grid.SetValue(CodeJieun.Utility.GetMouseWorldPosition(), selectedStr);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            grid.save();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            grid.load();
        }
    }

    public void Find()
    {
        grid.FindValue(findStr);
    }
}
