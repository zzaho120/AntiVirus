using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleTutorial : MonoBehaviour
{
    public int pageIdx;
    private int maxPageIdx = 8;
    public Image tutorialImage;
    public TextMeshProUGUI pageIdxText;

    public List<Sprite> tutorialImages;

    public void Init()
    {
        pageIdx = 0;
        UpdateTurorial();
    }


    public void UpdateTurorial()
    {
        tutorialImage.sprite = tutorialImages[pageIdx];
        pageIdxText.text = $"{pageIdx + 1} / {maxPageIdx + 1}";
    }

    public void OnClickNextPage()
    {
        pageIdx++;
        pageIdx = Mathf.Clamp(pageIdx, 0, maxPageIdx);
        UpdateTurorial();
    }

    public void OnClickPrevPage()
    {
        pageIdx--;
        pageIdx = Mathf.Clamp(pageIdx, 0, maxPageIdx + 1);
        UpdateTurorial();
    }
}
