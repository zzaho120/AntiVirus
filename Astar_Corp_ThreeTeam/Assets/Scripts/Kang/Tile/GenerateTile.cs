using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTile : MonoBehaviour
{
    public static int count = 1;
    public GameObject tilePrefabs;
    public GameObject parent;
    public int maxX = 2;
    public int maxY = 2;
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
            parent.transform.position = Vector3.zero;
            parent.transform.localPosition = new Vector3(-0.5f, -0.5f, -0.5f);
        }

        transform.position = Vector3.zero;
        transform.localScale = new Vector3(-1f, 1f, -1f);
        var obj = transform.GetChild(0);
        obj.transform.position = Vector3.zero;
        obj.localScale = new Vector3(0.4f, 1f, 0.4f);

        name = $"Battle_Tile {count++}";

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

    public void GenerateBuilding()
    {
        if (tilePrefabs == null)
        {
            tilePrefabs = (GameObject)Resources.Load("Kang/Prefabs/WallBase");
        }

        if (parent == null)
        {
            parent = new GameObject("Parent");
            parent.transform.SetParent(gameObject.transform);
            parent.transform.position = Vector3.zero;
            parent.transform.localPosition = new Vector3(-0.5f, -0.5f, -0.5f);
        }

        transform.position = Vector3.zero;
        transform.localScale = new Vector3(-1f, 1f, -1f);
        var obj = transform.GetChild(0);
        obj.transform.position = Vector3.zero;
        obj.localScale = new Vector3(0.8f, .8f, .8f);

        name = $"Battle_Building {count++}";

        for (var x = 0; x < maxX; x++)
        {
            var isZeroOrMax_X = x == 0 || x == maxX - 1;
            for (var y = 0; y < maxY; y++)
            {
                if (!isZeroOrMax_X)
                {
                    var isZeroOrMax_Y = y == 0 || y == maxY - 1;
                    if (!isZeroOrMax_Y)
                        continue;
                }    

                var newIdx = new Vector3(x, 0, y);
                var go = Instantiate(tilePrefabs, parent.transform);
                go.transform.position += newIdx;
                Debug.Log(newIdx);
            }
        }
    }
}
