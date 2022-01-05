using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionMgr : MonoBehaviour
{
    public BunkerMgr manager;
    public GameObject conWindow;
    public List<GameObject> bunkers;
    public GameObject prefab;
    public List<GameObject> prefabs;

    private void Start()
    {
        foreach (var element in bunkers)
        {
            var go = Instantiate(prefab, conWindow.transform);
            go.transform.position = Camera.main.WorldToScreenPoint(element.transform.position);

            //var button = go.transform.GetChild(2).gameObject.GetComponent<Button>();
            //button.onClick.AddListener(delegate { manager.CloseWindow(); });

            prefabs.Add(go);
        }
    }
}
