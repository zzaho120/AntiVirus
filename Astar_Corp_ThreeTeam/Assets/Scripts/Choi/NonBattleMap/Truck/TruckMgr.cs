using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckMgr : MonoBehaviour
{
    public GameObject TruckWin;
    public GameObject Contents;
    public GameObject TruckUnitPrefab;
    public List<GameObject> TruckUnitGOs = new List<GameObject>();
    public Text RemainingNum;

    PlayerDataMgr playerDataMgr;
    
    void Start()
    {
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

        Init();
    }

    public void Init()
    {
        if (playerDataMgr == null)
        {
            var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
            playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
        }

        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();

        if (TruckUnitGOs.Count != 0)
        {
            foreach (var element in TruckUnitGOs)
            {
                Destroy(element);
            }
            TruckUnitGOs.Clear();

            Contents.transform.DetachChildren();
        }

        foreach (var element in playerDataMgr.currentSquad)
        {
            var go = Instantiate(TruckUnitPrefab, Contents.transform);
            var name = go.transform.GetChild(1).GetComponent<Text>();
            name.text = element.Value.character.name;

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectBattleCharacter(element.Key); });

            TruckUnitGOs.Add(go);
        }
    }

    public void Open()
    {
        TruckWin.SetActive(true);
        Init();
    }

    public void Close()
    {
        TruckWin.SetActive(false);
    }

    void SelectBattleCharacter(int num)
    {
        //������� ��.
        if (playerDataMgr.currentSquad[num].character.name == string.Empty) return;
        
        //�ߺ��� ��.
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            playerDataMgr.battleSquad.Remove(num);
            var go = TruckUnitGOs[num];
            var child = go.transform.GetChild(0).GetComponent<Image>();
            child.color = Color.white;

            RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
            return;
        }

        //����á����.
        if (playerDataMgr.battleSquad.Count == 4) return;

        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        var selectedGo = TruckUnitGOs[num];
        var selectedChild = selectedGo.transform.GetChild(0).GetComponent<Image>();
        selectedChild.color = Color.red;

        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
    }

    


}
