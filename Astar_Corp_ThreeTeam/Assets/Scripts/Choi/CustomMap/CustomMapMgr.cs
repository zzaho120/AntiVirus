using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMapMgr : MonoBehaviour
{
    public GridTesting testScript;
    string currentStr;
    string findStr;

    //넣기.
    public void Select(string str)
    {
        currentStr = str;
        testScript.mode = CustomMapMode.Create;
        testScript.selectedStr = currentStr;
    }

    //찾기.
    public void Find(string str)
    {
        findStr = str;
        testScript.mode = CustomMapMode.Find;
        testScript.findStr = findStr;
        testScript.Find();
    }


    //private GraphicRaycaster gr;

    //private void Awake()
    //{
    //    gr = GetComponent<GraphicRaycaster>();
    //}

    //private void Update()
    //{
    //    var ped = new PointerEventData(null);
    //    ped.position = Input.mousePosition;
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    gr.Raycast(ped, results);

    //    if (results.Count <= 0) return;

    //    if (results[0].gameObject.layer == LayerMask.NameToLayer("CustomMap"))
    //    {
    //        //Debug.Log($"{results[0].gameObject.name}");


    //    }
    //}
}
