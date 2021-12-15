using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;

    private List<TileBase> tileList = new List<TileBase>();

    private int MAX_X_IDX = 10;
    private int MAX_Y_IDX = 2;
    private int MAX_Z_IDX = 10;

    private int OFFSET_X = 1;
    private int OFFSET_Y = 1;
    private int OFFSET_Z = 1;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        GenerateTile();
    }

    private void GenerateTile()
    {
        for(int i = 0; i < MAX_Y_IDX; ++i)
        {
            for (int j = 0; j < MAX_Z_IDX; ++j)
            {
                for (int k = 0; k < MAX_X_IDX; ++k)
                {
                    if (i != 0)
                        continue;

                    var go = Instantiate(tilePrefab, gameObject.transform);
                    go.name = $"Tile {k}, {i}, {j}";
                    go.transform.position = new Vector3(OFFSET_X * k,
                                                        OFFSET_Y * i,
                                                        OFFSET_Z * j);
                    
                    var tileBase = go.GetComponent<TileBase>();
                    tileBase.tileIdx = new Vector3(j, i, j);
                    tileBase.wallValue = (Wall)Random.Range(0000, 1111);
                    tileBase.Generate();

                    if (tileBase != null)
                        tileList.Add(tileBase);
                }
            }
        }
    }
}
