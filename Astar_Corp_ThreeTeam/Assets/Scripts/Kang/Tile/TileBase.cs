using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    private TileMgr mgr;

    [Header("Value")]
    public Vector3 tileIdx;
    public GameObject tileObj;
    public List<Material> materials;
    public bool isWall;
    public int accuracy;
    public int moveAP;
    public int movePoint;
    public int virusLevel;

    [Header("List")]
    public List<TileBase> adjNodes;

    [Header("RunTime")]
    public GameObject charObj;
    public GameObject fogTile;
    public TileBase wallTile;
    public List<HintBase> hintObj = new List<HintBase>();

    public TileBase(TileBase copy)
    {
        tileIdx = copy.tileIdx;
        tileObj = copy.tileObj;
        materials = copy.materials;
        isWall = copy.isWall;
    }
    public void Init(TileMgr tileMgr)
    {
        mgr = tileMgr;
        virusLevel = int.MaxValue;
        InitIdx();
    }

    private void InitIdx()
    {
        if (!isWall)
            tileIdx = transform.position;
        else
            tileIdx = new Vector3(transform.position.x, 1f, transform.position.z);
        gameObject.name = $"Tile ({tileIdx.x}, {tileIdx.y}, {tileIdx.z})";
    }

    public void EnableDisplay(bool isEnabled)
    {
        if (charObj != null)
        {
            if (charObj.tag == "BattleMonster")
            {
                var monster = charObj.GetComponent<MonsterChar>();
                foreach (var renElem in monster.renList)
                {
                    renElem.SetActive(isEnabled);
                }
            }
        }
        foreach (var hint in hintObj)
        {
            hint.hintObj.GetComponent<MeshRenderer>().enabled = isEnabled;
        }

        if (fogTile != null)
            fogTile.SetActive(!isEnabled);
        else
            Debug.Log(name);
    }

    public void SetHighlight()
    {
        if (charObj != null && charObj.tag == "BattleMonster")
        {
            var ren = tileObj.GetComponent<MeshRenderer>();

            ren.material.color = Color.red;
        }
    }

    public void AddHint(HintBase hint)
    {
        hintObj.Add(hint);
    }

    public void RemoveHint(HintBase hint)
    {
        // 해당하는 힌트가 그 타일에 있냐?
        hintObj.Remove(hint);
    }
}