using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWinMgr : MonoBehaviour
{ 
    public PlayerDataMgr playerDataMgr;
    private AgitMgr agitMgr;

    public GameObject skillPage;
    public GameObject skillDetailWin;
    public Text skillDetailTxt;
    public Text skillPointTxt;

    public List<GameObject> skills;
    public int currentIndex;
    int skillIndex;
    Color originCharaterSkillColor;
    Color originStatSkillColor;

    public Dictionary<string, ActiveSkill> activeList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveList = new Dictionary<string, PassiveSkill>();

    public void Init()
    {
        agitMgr = GetComponent<AgitMgr>();

        if (skillDetailWin.activeSelf) skillDetailWin.SetActive(false);
        skillPage.SetActive(true);

        originCharaterSkillColor = skills[0].GetComponent<Image>().color;   // 스킬
        originStatSkillColor = skills[3].GetComponent<Image>().color;       // 스탯
        //3,4,8,9,13,14

        skillIndex = -1;

        // 스킬리스트 불러오기
        activeList = playerDataMgr.activeSkillList;
        passiveList = playerDataMgr.passiveSkillList;
    }

    public void SelectSkill(int index)
    {
        if (skillIndex != -1)
        {
            if (skillIndex == 3 || skillIndex == 4 || skillIndex == 8 ||
                skillIndex == 9 || skillIndex == 13 || skillIndex == 14)
            {
                skills[skillIndex].GetComponent<Image>().color = originStatSkillColor;
            }
            else skills[skillIndex].GetComponent<Image>().color = originCharaterSkillColor;
        }
        skillIndex = index;
        skills[skillIndex].GetComponent<Image>().color = Color.red;

        skillDetailTxt.text = "스킬 설명";
        skillDetailWin.SetActive(true);
    }

    public void SkillAcquisition()
    {
        skillDetailWin.SetActive(false);

        var character = playerDataMgr.currentSquad[currentIndex];
        var skillBtn = skills[skillIndex].GetComponentInChildren<SkillUnitBase>();

        if (passiveList.ContainsKey(skillBtn.skillId))
        {
            character.skillMgr.AddSkill(SkillType.Passive, passiveList[skillBtn.skillId]);
            Debug.Log($"{passiveList[skillBtn.skillId].skillName} 스킬 추가 완료");
        }
    }

    public void OpenSkillPage()
    {
        skillPage.SetActive(true);
        InitCharSkills();
    }

    public void CloseSkillPage()
    {
        skillPage.SetActive(false);
    }
    
    // Skill Page가 오픈될 때 작동하는 메소드
    public void InitCharSkills()
    {
        CharacterStats character = playerDataMgr.currentSquad[currentIndex];

        for (int i = 0; i < character.character.skillA.Count; i++)
        {
            if (activeList.ContainsKey(character.character.skillA[i]))
            {
                skills[i].GetComponentInChildren<Text>().text = activeList[character.character.skillA[i]].name;
                skills[i].GetComponentInChildren<SkillUnitBase>().skillId = activeList[character.character.skillA[i]].id;
            }
        }
        for (int i = 0; i < character.character.skillB.Count; i++)
        {
            if (activeList.ContainsKey(character.character.skillB[i]))
            {
                skills[i + 5].GetComponentInChildren<Text>().text = activeList[character.character.skillB[i]].name;
                skills[i + 5].GetComponentInChildren<SkillUnitBase>().skillId = activeList[character.character.skillB[i]].id;

            }
        }
        for (int i = 0; i < character.character.skillC.Count; i++)
        {
            if (activeList.ContainsKey(character.character.skillC[i]))
            {
                skills[i + 10].GetComponentInChildren<Text>().text = activeList[character.character.skillC[i]].name;
                skills[i + 10].GetComponentInChildren<SkillUnitBase>().skillId = activeList[character.character.skillC[i]].id;

            }
        }
        for (int i = 0; i < character.character.skillD.Count; i++)
        {
            if (activeList.ContainsKey(character.character.skillD[i]))
            {
                skills[i + 17].GetComponentInChildren<Text>().text = activeList[character.character.skillD[i]].name;
                skills[i + 17].GetComponentInChildren<SkillUnitBase>().skillId = activeList[character.character.skillD[i]].id;
            }
        }

        // 스킬창 개수 조절
        // 렙1스킬 (a)
        if (character.character.skillA.Count < 2)
        {
            skills[1].GetComponent<Image>().color = Color.gray;
            skills[1].GetComponent<Button>().interactable = false;
            skills[1].GetComponentInChildren<Text>().enabled = false;

            skills[2].GetComponent<Image>().color = Color.gray;
            skills[2].GetComponent<Button>().interactable = false;
            skills[2].GetComponentInChildren<Text>().enabled = false;
        }
        else if (character.character.skillA.Count < 3)
        {
            skills[2].GetComponent<Image>().color = Color.gray;
            skills[2].GetComponent<Button>().interactable = false;
            skills[2].GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            skills[1].GetComponent<Image>().color = originCharaterSkillColor;
            skills[1].GetComponent<Button>().interactable = true;
            skills[1].GetComponentInChildren<Text>().enabled = true;

            skills[2].GetComponent<Image>().color = originCharaterSkillColor;
            skills[2].GetComponent<Button>().interactable = true;
            skills[2].GetComponentInChildren<Text>().enabled = true;
        }

        // (b)
        if (character.character.skillB.Count < 2)
        {
            skills[6].GetComponent<Image>().color = Color.gray;
            skills[6].GetComponent<Button>().interactable = false;
            skills[6].GetComponentInChildren<Text>().enabled = false;

            skills[7].GetComponent<Image>().color = Color.gray;
            skills[7].GetComponent<Button>().interactable = false;
            skills[7].GetComponentInChildren<Text>().enabled = false;
        }
        else if (character.character.skillB.Count < 3)
        {
            skills[7].GetComponent<Image>().color = Color.gray;
            skills[7].GetComponent<Button>().interactable = false;
            skills[7].GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            skills[6].GetComponent<Image>().color = originCharaterSkillColor;
            skills[6].GetComponent<Button>().interactable = true;
            skills[6].GetComponentInChildren<Text>().enabled = true;

            skills[7].GetComponent<Image>().color = originCharaterSkillColor;
            skills[7].GetComponent<Button>().interactable = true;
            skills[7].GetComponentInChildren<Text>().enabled = true;
        }

        // (c)
        if (character.character.skillC.Count < 2)
        {
            skills[11].GetComponent<Image>().color = Color.gray;
            skills[11].GetComponent<Button>().interactable = false;
            skills[11].GetComponentInChildren<Text>().enabled = false;

            skills[12].GetComponent<Image>().color = Color.gray;
            skills[12].GetComponent<Button>().interactable = false;
            skills[12].GetComponentInChildren<Text>().enabled = false;
        }
        else if (character.character.skillC.Count < 3)
        {
            skills[12].GetComponent<Image>().color = Color.gray;
            skills[12].GetComponent<Button>().interactable = false;
            skills[12].GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            skills[11].GetComponent<Image>().color = originCharaterSkillColor;
            skills[11].GetComponent<Button>().interactable = true;
            skills[11].GetComponentInChildren<Text>().enabled = true;

            skills[12].GetComponent<Image>().color = originCharaterSkillColor;
            skills[12].GetComponent<Button>().interactable = true;
            skills[12].GetComponentInChildren<Text>().enabled = true;
        }

        // (d)
        if (character.character.skillD.Count < 2)
        {
            skills[18].GetComponent<Image>().color = Color.gray;
            skills[18].GetComponent<Button>().interactable = false;
            skills[18].GetComponentInChildren<Text>().enabled = false;

            skills[19].GetComponent<Image>().color = Color.gray;
            skills[19].GetComponent<Button>().interactable = false;
            skills[19].GetComponentInChildren<Text>().enabled = false;
        }
        else if (character.character.skillD.Count < 3)
        {
            skills[19].GetComponent<Image>().color = Color.gray;
            skills[19].GetComponent<Button>().interactable = false;
            skills[19].GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            skills[18].GetComponent<Image>().color = originCharaterSkillColor;
            skills[18].GetComponent<Button>().interactable = true;
            skills[18].GetComponentInChildren<Text>().enabled = true;

            skills[19].GetComponent<Image>().color = originCharaterSkillColor;
            skills[19].GetComponent<Button>().interactable = true;
            skills[19].GetComponentInChildren<Text>().enabled = true;
        }

        // 버튼 잠그기
        if (playerDataMgr.currentSquad[currentIndex].Name == "Sniper")
        {
            for (int i = 20; i < skills.Count; i++)
            {

                skills[i].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (int i = 17; i < skills.Count; i++)
            {

                skills[i].GetComponent<Button>().interactable = false;
            }
        }
    }
}
