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
    public GameObject truckEquips;
    public GameObject truckConsums;
    private TextMeshProUGUI charInfoTxt;

    [Header("Truck Inventory")]
    public GameObject equipTruckItem;
    public GameObject consumTruckItem;

    PlayerDataMgr playerDataMgr;
    
    void Start()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        Init();
    }

    public void Init()
    {
        if (playerDataMgr == null)
        {
            playerDataMgr = PlayerDataMgr.Instance;
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

    private bool isItemInit;

    public void TruckUISetting()
    {
        // BattleSquad Setting
        foreach (var element in playerDataMgr.battleSquad)
        {
            var go = Instantiate(selectedCharPrefab, selectedChars.transform);
            var goName = go.GetComponentInChildren<TextMeshProUGUI>();
            goName.text = element.Value.character.name;

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharInventory(element.Key); });
        }

        if (!isItemInit)
        {
            Debug.Log("Init Items");

            // 트럭 장비템 출력
            foreach (var element in playerDataMgr.truckEquippables)
            {
                var go = Instantiate(TruckUnitPrefab, truckEquips.transform);
                go.AddComponent<Button>();

                var goName = go.GetComponentInChildren<Text>();
                if (goName != null)
                    goName.text = element.Value.name;
            }

            // 트럭 소비템 출력
            foreach (var element in playerDataMgr.truckConsumables)
            {
                var go = Instantiate(TruckUnitPrefab, truckConsums.transform);
                go.AddComponent<Button>();

                var goName = go.GetComponentInChildren<Text>();
                if (goName != null)
                    goName.text = element.Value.name;
            }

            isItemInit = true;
        }
    }

    // 트럭 아이템 카테고리 선택 (장비 vs 소비)
    public void SelectItemType(int value)
    {
        var conImg = consumTruckItem.GetComponent<Image>();
        var wepImg = equipTruckItem.GetComponent<Image>();

        if (value == 0)
        {
            if (conImg.enabled)
                conImg.enabled = false;

            wepImg.enabled = true;
        }
        else if (value == 1)
        {
            //if (wepImg.enabled)
            //    wepImg.enabled = false;

            conImg.enabled = true;
        }
    }

    private void SelectCharInventory(int key)
    {
        // 임시로
        var charName = GameObject.Find("Char Name").GetComponent<TextMeshProUGUI>();
        charName.text = playerDataMgr.battleSquad[key].character.name;
    }
}
