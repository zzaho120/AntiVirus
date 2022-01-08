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

        //int key = characterInfo[currentIndex];
        //var character = playerDataMgr.currentSquad[key];
        //hpSlider.value = character.currentHp / character.maxHp;
        ////여기.
        //float max = 100f;
        //eVirusSlider.value = character.virusPanalty["E"].penaltyGauge / max;
        //bVirusSlider.value = character.virusPanalty["B"].penaltyGauge / max;
        //pVirusSlider.value = character.virusPanalty["P"].penaltyGauge / max;
        //iVirusSlider.value = character.virusPanalty["I"].penaltyGauge / max;
        //tVirusSlider.value = character.virusPanalty["T"].penaltyGauge / max;
    }

    public void RecoveryHp()
    {
        if (currentIndex == -1) return;
        //돈도 부족하면 리턴.

        int key = characterInfo[currentIndex];

        //int loss = playerDataMgr.currentSquad[key].maxHp - playerDataMgr.currentSquad[key].currentHp;
        //int recoveryAmount = Mathf.FloorToInt(loss * 0.1f);
        //playerDataMgr.currentSquad[key].currentHp += recoveryAmount;
        //if (playerDataMgr.currentSquad[key].currentHp == playerDataMgr.currentSquad[key].maxHp)
        //    playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].maxHp;
        ////체력 UI변경.
        //hpSlider.value = playerDataMgr.currentSquad[key].currentHp / playerDataMgr.currentSquad[key].maxHp;
    }

    public void RecoveryVirusGauge()
    { 
    
    }
}
