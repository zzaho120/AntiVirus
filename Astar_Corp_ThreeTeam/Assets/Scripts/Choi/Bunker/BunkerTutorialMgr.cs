using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunkerTutorialMgr : MonoBehaviour
{
    [Header("StoryWin")]
    public GameObject storyWin;

    public Text detailTxt;
    int index;

    public void OpenStoryWin()
    {
        if (!storyWin.activeSelf) storyWin.SetActive(true);
    }

    public void CloseStoryWin()
    {
        if (storyWin.activeSelf) storyWin.SetActive(false);
    }

    public void Tutorial()
    {
        switch (index)
        {
            case 0:

                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}
