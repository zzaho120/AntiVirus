using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TutorialKind
{ 
    None,
    Bunker,
    Agit,
    Storage,
    CarCener,
    Hospital,
    Pub,
    Store,
    Garage
}

public class BunkerTutorialMgr : MonoBehaviour
{
    public BunkerMgr bunkerMgr;

    [Header("StoryWin")]
    public GameObject storyWin;
    public GameObject tutorialWin;
    public Image tutorialImg;
    public Text pageTxt;

    public List<Sprite> bunkerSprites;
    public Sprite agitSprites;
    public Sprite storageSprites;
    public Sprite carCenterSprites;
    public Sprite hospitalSprites;
    public Sprite pubSprites;
    public List< Sprite> storeSprites;
    public List< Sprite> garageSprites;

    public Text detailTxt;
    int index;
    TutorialKind kind;

    public void Setting(int index)
    {
        switch (index)
        {
            case 0:
                kind = TutorialKind.Bunker;
                break;
            case 1:
                kind = TutorialKind.Agit;
                break;
            case 2:
                kind = TutorialKind.Storage;
                break;
            case 3:
                kind = TutorialKind.CarCener;
                break;
            case 4:
                kind = TutorialKind.Hospital;
                break;
            case 5:
                kind = TutorialKind.Pub;
                break;
            case 6:
                kind = TutorialKind.Store;
                break;
            case 7:
                kind = TutorialKind.Garage;
                break;
        }
    }

    public void Previous()
    {
        if (!(kind == TutorialKind.Bunker || kind == TutorialKind.Store
            || kind == TutorialKind.Garage)) return;
        if (index == 1) return;

        index = 1;
        pageTxt.text = "1/2";

        switch (kind)
        {
            case TutorialKind.Bunker:
                tutorialImg.sprite = bunkerSprites[0];
                break;
            case TutorialKind.Store:
                tutorialImg.sprite = storeSprites[0];
                break;
            case TutorialKind.Garage:
                tutorialImg.sprite = garageSprites[0];
                break;
        }
    }

    public void Next()
    {
        if (!(kind == TutorialKind.Bunker || kind == TutorialKind.Store
               || kind == TutorialKind.Garage)) return;
        if (index == 2) return;

        index = 2;
        pageTxt.text = "2/2";

        switch (kind)
        {
            case TutorialKind.Bunker:
                tutorialImg.sprite = bunkerSprites[1];
                break;
            case TutorialKind.Store:
                tutorialImg.sprite = storeSprites[1];
                break;
            case TutorialKind.Garage:
                tutorialImg.sprite = garageSprites[1];
                break;
        }
    }

    public void Open()
    {
        bunkerMgr.isWinOpen = true;
        index = 1;

        switch (kind)
        {
            case TutorialKind.Bunker:
                tutorialImg.sprite = bunkerSprites[0];
                pageTxt.text = $"1 / 2";
                break;
            case TutorialKind.Agit:
                tutorialImg.sprite = agitSprites;
                pageTxt.text = $"1 / 1";
                break;
            case TutorialKind.Storage:
                tutorialImg.sprite = storageSprites;
                pageTxt.text = $"1 / 1";
                break;
            case TutorialKind.CarCener:
                tutorialImg.sprite = carCenterSprites;
                pageTxt.text = $"1 / 1";
                break;
            case TutorialKind.Hospital:
                tutorialImg.sprite = hospitalSprites;
                pageTxt.text = $"1 / 1";
                break;
            case TutorialKind.Pub:
                tutorialImg.sprite = pubSprites;
                pageTxt.text = $"1 / 1";
                break;
            case TutorialKind.Store:
                tutorialImg.sprite = storeSprites[0];
                pageTxt.text = $"1 / 2";
                break;
            case TutorialKind.Garage:
                tutorialImg.sprite = garageSprites[0];
                pageTxt.text = $"1 / 2";
                break;
        }
        tutorialWin.SetActive(true);
    }

    public void Close()
    {
        bunkerMgr.isWinOpen = false;

        kind = TutorialKind.None;
        tutorialWin.SetActive(false);
    }

    public void OpenStoryWin()
    {
        bunkerMgr.isWinOpen = true;
        if (!storyWin.activeSelf) storyWin.SetActive(true);
    }

    public void CloseStoryWin()
    {
        bunkerMgr.isWinOpen = false;
        if (storyWin.activeSelf) storyWin.SetActive(false);
        Setting(0);
        Open();
    }
}
