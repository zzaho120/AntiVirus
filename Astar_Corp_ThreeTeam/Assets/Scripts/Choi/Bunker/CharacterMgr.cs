using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMgr : MonoBehaviour
{
    public GameObject ListPrefab;
    public GameObject ListParent;
    public GameObject squadObj;
    public GameObject antibodyView;
    public GameObject weaponView;

    PlayerDataMgr playerDataMgr;
    List<Antibody> antibodyList = new List<Antibody>();
    List<GameObject> antibodyObjs = new List<GameObject>();

    List<AttackDefinition> weaponList = new List<AttackDefinition>();
    List<GameObject> weaponObjs = new List<GameObject>();

    void Start()
    {
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        antibodyList = playerDataMgr.antibodyList;
        weaponList = playerDataMgr.weaponList;

        var currentSquad = playerDataMgr.currentSquad;
        int i = 0;
        foreach (var element in currentSquad)
        {
            var child = squadObj.transform.GetChild(i).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Text>().text = element.Value;
            i++;
        }
    }

    public void OpenAntibody()
    {
        int i = 0;
        foreach (var element in antibodyList)
        {
            //버튼.
            var go = Instantiate(ListPrefab, ListParent.transform);
            go.AddComponent<Button>().onClick.AddListener(delegate { ClickAntibody(i); });
            
            //텍스트.
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.name;
            antibodyObjs.Add(go);
            i++;
        }
    }

    public void ClickAntibody(int i)
    { 
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
