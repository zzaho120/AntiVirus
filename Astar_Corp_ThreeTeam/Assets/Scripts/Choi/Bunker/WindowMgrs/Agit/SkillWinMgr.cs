using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWinMgr : MonoBehaviour
{ 
    public PlayerDataMgr playerDataMgr;

    public GameObject skillPage;
    public GameObject skillDetailWin;
    public Text skillDetailTxt;
    public Text skillPointTxt;

    public List<GameObject> skills;
    public int currentIndex;
    int skillIndex;
    Color originCharaterSkillColor;
    Color originStatSkillColor;
    
    public void Init()
    {
        if (skillDetailWin.activeSelf) skillDetailWin.SetActive(false);
        skillPage.SetActive(true);

        originCharaterSkillColor = skills[0].GetComponent<Image>().color;
        originStatSkillColor = skills[3].GetComponent<Image>().color;
        //3,4,8,9,13,14

        skillIndex = -1;
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
    }

    public void OpenSkillPage()
    {
        skillPage.SetActive(true);
    }

    public void CloseSkillPage()
    {
        skillPage.SetActive(false);
    }
}
