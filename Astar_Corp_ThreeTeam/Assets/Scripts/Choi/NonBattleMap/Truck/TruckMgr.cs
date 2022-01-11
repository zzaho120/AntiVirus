using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckMgr : MonoBehaviour
{
    [Header("Truck Squad")]
    public GameObject TruckWin;
    public GameObject Contents;
    public GameObject TruckUnitPrefab;
    public Text RemainingNum;
    [HideInInspector]
    public List<GameObject> TruckUnitGOs = new List<GameObject>();

    [Header("Selected Characters")]
    public GameObject selectedCharPrefab;
    public GameObject selectedChars;
    private TextMeshProUGUI charInfoTxt;

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

        // CurrentSquad Setting
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

        // MemberSelectPopup 창에서 띄울 캐릭터 정보 텍스트
        // 해당 팝업창이 Open 될때 Find
        charInfoTxt = GameObject.Find("CharacterInfo").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Close()
    {
        TruckWin.SetActive(false);
    }

    void SelectBattleCharacter(int num)
    {
        //비어있을 때.
        if (playerDataMgr.currentSquad[num].character.name == string.Empty) return;
        
        //중복될 때.
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            playerDataMgr.battleSquad.Remove(num);
            var go = TruckUnitGOs[num];
            var child = go.transform.GetChild(0).GetComponent<Image>();
            child.color = Color.white;

            RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();
            return;
        }

        //가득찼을때.
        if (playerDataMgr.battleSquad.Count == 4) return;

        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        var selectedGo = TruckUnitGOs[num];
        var selectedChild = selectedGo.transform.GetChild(0).GetComponent<Image>();
        selectedChild.color = Color.red;

        RemainingNum.text = (4 - playerDataMgr.battleSquad.Count).ToString();

        //클릭 시 옆에 캐릭터 정보 표시
        charInfoTxt.text = playerDataMgr.battleSquad[num].Name + "\n" +
            "Hp : " + playerDataMgr.battleSquad[num].currentHp;
    }

    public void PrintSelectedChars()
    {
        // BattleSquad Setting
        foreach (var element in playerDataMgr.battleSquad)
        {
            var go = Instantiate(selectedCharPrefab, selectedChars.transform);
            var goName = go.GetComponentInChildren<TextMeshProUGUI>();
            goName.text = element.Value.character.name;
        }
    }
}
