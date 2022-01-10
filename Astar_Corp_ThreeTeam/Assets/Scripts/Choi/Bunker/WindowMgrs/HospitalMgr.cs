using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HospitalMgr : MonoBehaviour
{
    public GameObject characterListContent;
    public GameObject characterPrefab;
    public GameObject stateWin;

    //상태창.
    public Slider hpSlider;
    public Slider eVirusSlider;
    public Slider bVirusSlider;
    public Slider pVirusSlider;
    public Slider iVirusSlider;
    public Slider tVirusSlider;

    public PlayerDataMgr playerDataMgr;

    List<GameObject> characterObjs = new List<GameObject>();
    // 리스트 순서 / PlayerDataMgr Key
    Dictionary<int, int> characterInfo = new Dictionary<int, int>();
    int currentIndex;
    Color originColor;



    public void Init()
    {
        //이전 정보 삭제.
        if (characterObjs.Count != 0)
        {
            foreach (var element in characterObjs)
            {
                Destroy(element);
            }
            characterObjs.Clear();

            characterListContent.transform.DetachChildren();
        }
        if (characterInfo.Count != 0) characterInfo.Clear();

        if (stateWin.activeSelf) stateWin.SetActive(false);

        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            var go = Instantiate(characterPrefab, characterListContent.transform);
            go.transform.GetChild(1).GetComponent<Text>().text = element.Value.character.name;
            characterObjs.Add(go);

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            characterInfo.Add(num, element.Key);
            
            i++;
        }

        currentIndex = -1;
        originColor = characterPrefab.GetComponent<Image>().color;
    }

    void SelectCharacter(int index)
    {
        if (currentIndex == index) return;
        if (currentIndex != -1)
        {
            characterObjs[currentIndex].GetComponent<Image>().color = originColor;
        }

        currentIndex = index;
        characterObjs[currentIndex].GetComponent<Image>().color = Color.red;

        //상태창 관리.
        stateWin.SetActive(true);

        int key = characterInfo[currentIndex];
        var character = playerDataMgr.currentSquad[key];
        
        //테스트.
        character.currentHp = 20;
        hpSlider.value = (float)character.currentHp / character.maxHp;

        //테스트.
        character.virusPanalty["E"].penaltyGauge = 30;
        
        eVirusSlider.value = (float)character.virusPanalty["E"].penaltyGauge / character.virusPanalty["E"].GetMaxGauge();
        bVirusSlider.value = (float)character.virusPanalty["B"].penaltyGauge / character.virusPanalty["B"].GetMaxGauge();
        pVirusSlider.value = (float)character.virusPanalty["P"].penaltyGauge / character.virusPanalty["P"].GetMaxGauge();
        iVirusSlider.value = (float)character.virusPanalty["I"].penaltyGauge / character.virusPanalty["I"].GetMaxGauge();
        tVirusSlider.value = (float)character.virusPanalty["T"].penaltyGauge / character.virusPanalty["T"].GetMaxGauge();
    }

    public void RecoveryHp()
    {
        if (currentIndex == -1) return;
        //돈도 부족하면 리턴.

        int key = characterInfo[currentIndex];

        int loss = playerDataMgr.currentSquad[key].maxHp - playerDataMgr.currentSquad[key].currentHp;
        int recoveryAmount = Mathf.FloorToInt(loss * 0.1f);
        playerDataMgr.currentSquad[key].currentHp += recoveryAmount;
        if (playerDataMgr.currentSquad[key].currentHp >= playerDataMgr.currentSquad[key].maxHp)
            playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].maxHp;
        //체력 UI변경.
        hpSlider.value = (float)playerDataMgr.currentSquad[key].currentHp / playerDataMgr.currentSquad[key].maxHp;
    }

    public void RecoveryVirusGauge()
    {
        if (currentIndex == -1) return;
        //돈도 부족하면 리턴.

        int key = characterInfo[currentIndex];

        //int recoveryAmount = Mathf.FloorToInt(loss * 0.1f);
        //playerDataMgr.currentSquad[key].currentHp += recoveryAmount;
        //if (playerDataMgr.currentSquad[key].currentHp >= playerDataMgr.currentSquad[key].maxHp)
        //    playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].maxHp;
        ////체력 UI변경.
        //hpSlider.value = (float)playerDataMgr.currentSquad[key].currentHp / playerDataMgr.currentSquad[key].maxHp;

    }
}
