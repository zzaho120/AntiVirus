using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTile : MonoBehaviour
{
    public GameObject tilePrefabs;
    public GameObject parent;
    public int maxX = 5;
    public int maxY = 5;
    public void Generate()
    {
        if (tilePrefabs == null)
        {
            tilePrefabs = (GameObject)Resources.Load("Kang/Prefabs/TileBase");
        }

        if (parent == null)
        {
            parent = new GameObject("Parent");
            parent.transform.SetParent(gameObject.transform);
            parent.transform.localPosition = new Vector3(0, 0, 0);
        }

        for(var x = 0; x < maxX; x++)
        {
            for (var y = 0; y < maxY; y++)
            {
                var newIdx = new Vector3(x, 0, y);
                var go = Instantiate(tilePrefabs, parent.transform);
                go.transform.position += newIdx;
                Debug.Log(newIdx);
            }
        }
    }
}
