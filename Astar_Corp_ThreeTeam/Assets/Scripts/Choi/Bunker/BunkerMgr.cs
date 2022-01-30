using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public enum BunkerKinds
{
    None,
    Agit,
    Pub,
    Store,
    Hospital,
    Garage,
    Storage,
    Locked,
}

public class BunkerMgr : MonoBehaviour
{
    public MultiTouch multiTouch;
    public BunkerCamController camController;
    public WindowManager windowManager;
    
    public AgitMgr agitMgr;
    public PubMgr pubMgr;
    public StoreMgr storeMgr;
    public HospitalMgr hospitalMgr;
    public GarageMgr garageMgr;
    PlayerDataMgr playerDataMgr;
    public CarCenterMgr carCenterMgr;  //수진
    public StorageMgr storageMgr;
    UpgradeMgr upgradeMgr;

    public GameObject createButton;
    public GameObject destroyButton;

    public Camera camera;
    public GameObject selectedBunker;
    public BunkerKinds currentBunkerKind;
    int currentWinId;

    public Animator bunkerMenuAnim;
    bool isBunkerMenuOpen;
    [Header("창관련")]
    public GameObject pauseWin;
    public GameObject optionWin;
    public bool isWinOpen;

    [Header("상단UI")]
    public Text bullet5Txt;
    public Text bullet7Txt;
    public Text bullet9Txt;
    public Text bullet45Txt;
    public Text bullet12Txt;
    public Text nameTxt;
    public Text moneyTxt;
    public GameObject mapButton;

    public GameObject belowUI;

    [Header("Prefabs")]
    public GameObject emptyPrefab;
    public GameObject agitPrefab;
    public GameObject pubPrefab;
    public GameObject storePrefab;
    //public GameObject carCenterPrefab;
    public GameObject hospitalPrefab;
    public GameObject garagePrefab;
    public GameObject storagePrefab;
    public GameObject lockedPrefab;
    public List<GameObject> bunkerObjs;

    int bunkerCount;
    public int currentBunkerIndex;

    bool isOpenWin;

    public Mode currentMode;

    private void Awake()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        agitMgr = GameObject.FindGameObjectWithTag("AgitMgr").GetComponent<AgitMgr>();
        pubMgr = GameObject.FindGameObjectWithTag("PubMgr").GetComponent<PubMgr>();
        storeMgr = GameObject.FindGameObjectWithTag("StoreMgr").GetComponent<StoreMgr>();
        hospitalMgr = GameObject.FindGameObjectWithTag("HospitalMgr").GetComponent<HospitalMgr>();
        garageMgr = GameObject.FindGameObjectWithTag("GarageMgr").GetComponent<GarageMgr>();
        carCenterMgr = GameObject.Find("CarCenterMgr").GetComponent<CarCenterMgr>();
        storageMgr = GameObject.Find("StorageMgr").GetComponent<StorageMgr>();
        upgradeMgr = GameObject.Find("UpgradeMgr").GetComponent<UpgradeMgr>();

        agitMgr.playerDataMgr = playerDataMgr;
        agitMgr.bunkerMgr = this;

        pubMgr.playerDataMgr = playerDataMgr;
        pubMgr.bunkerMgr = this;

        storeMgr.playerDataMgr = playerDataMgr;
        storeMgr.bunkerMgr = this;

        hospitalMgr.playerDataMgr = playerDataMgr;
        garageMgr.playerDataMgr = playerDataMgr;
        carCenterMgr.playerDataMgr = playerDataMgr;
        
        storageMgr.playerDataMgr = playerDataMgr;
        storageMgr.bunkerMgr = this;

        upgradeMgr.playerDataMgr = playerDataMgr;
        upgradeMgr.bunkerMgr = this;
        upgradeMgr.CloseWindow = CloseWindow;
    }

    private void Start()
    {
        if (pauseWin.activeSelf) pauseWin.SetActive(false);
        if (optionWin.activeSelf) optionWin.SetActive(false);
        isWinOpen = false;

        if (!belowUI.activeSelf) belowUI.SetActive(true);
        if (!mapButton.activeSelf) mapButton.SetActive(true);
        if (agitMgr.upperUI.activeSelf) agitMgr.upperUI.SetActive(false);
        if (agitMgr.belowUI.activeSelf) agitMgr.belowUI.SetActive(false);
        isBunkerMenuOpen = true;

        RefreshGoods();

        agitMgr.Init();
        pubMgr.Init();
        storeMgr.Init();
        hospitalMgr.Init();
        garageMgr.Init();
        carCenterMgr.Init();
        storageMgr.Init();

        camController.OpenWindow = OpenWindow;
        camController.CloseWindow = CloseWindow;
        camController.SetBunkerKind = SetBunkerKind;

        currentWinId = -1;
        currentBunkerKind = BunkerKinds.None;
        currentBunkerIndex = -1;

        selectedBunker = null;
        currentBunkerIndex = -1;
        currentBunkerKind = BunkerKinds.None;

        if (destroyButton.activeSelf) destroyButton.SetActive(false);
    }

    public void SetBunkerKind(Mode currentMode, GameObject bunker)
    {
        //if (currentMode == Mode.Touch)
        //{
            var script = bunker.GetComponent<BunkerBase>();
            currentBunkerIndex = script.bunkerId;
            var bunkerName = script.bunkerName;
            selectedBunker = bunker;

            //현재 벙커 종류.
            //if (!bunkerName.Equals("None"))
            //{
            //    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                switch (bunkerName)
                {
                    case "Agit":
                        currentBunkerKind = BunkerKinds.Agit;
                        break;
                    case "Pub":
                        currentBunkerKind = BunkerKinds.Pub;
                        break;
                    case "Store":
                        currentBunkerKind = BunkerKinds.Store;
                        break;
                    case "Hospital":
                        currentBunkerKind = BunkerKinds.Hospital;
                        break;
                    case "Garage":
                        currentBunkerKind = BunkerKinds.Garage;
                        break;
                    case "Storage":
                        currentBunkerKind = BunkerKinds.Storage;
                        break;
                }
        //    }
        //    else
        //    {
        //        if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);

        //        currentBunkerKind = BunkerKinds.None;
        //    }
        //}
        //else if (currentMode == Mode.Mouse)
        //{
        //    var script = bunker.GetComponent<BunkerBase>();
        //    currentBunkerIndex = script.bunkerId;
        //    var bunkerName = script.bunkerName;
        //    selectedBunker = bunker;

        //    //현재 벙커 종류.
        //    if (!bunkerName.Equals("None"))
        //    {
        //        if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

        //        switch (bunkerName)
        //        {
        //            case "Agit":
        //                currentBunkerKind = BunkerKinds.Agit;
        //                break;
        //            case "Pub":
        //                currentBunkerKind = BunkerKinds.Pub;
        //                break;
        //            case "Store":
        //                currentBunkerKind = BunkerKinds.Store;
        //                break;
        //            case "Hospital":
        //                currentBunkerKind = BunkerKinds.Hospital;
        //                break;
        //            case "Garage":
        //                currentBunkerKind = BunkerKinds.Garage;
        //                break;
        //            case "Storage":
        //                currentBunkerKind = BunkerKinds.Storage;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);

        //        currentBunkerKind = BunkerKinds.None;
        //    }
        //}
    }

    public void OpenWindow()
    {
        isWinOpen = true;
        if (mapButton.activeSelf) mapButton.SetActive(false);
        switch (currentBunkerKind)
        {
            case BunkerKinds.Agit:
                nameTxt.text = "아지트"; 
                agitMgr.Init();
                agitMgr.OpenMainWin();
                currentWinId = (int)BunkerWindows.AgitWindow - 1;
                break;
            case BunkerKinds.Pub:
                pubMgr.OpenMainWin();
                currentWinId = (int)BunkerWindows.PubWindow - 1;
                break;
            case BunkerKinds.Store:
                storeMgr.Init();
                storeMgr.OpenMainWin();
                currentWinId = (int)BunkerWindows.StoreWindow - 1;
                break;
            case BunkerKinds.Hospital:
                hospitalMgr.Init();
                currentWinId = (int)BunkerWindows.HospitalWindow - 1;
                break;
            case BunkerKinds.Garage:
                garageMgr.Init();
                currentWinId = (int)BunkerWindows.GarageWindow - 1;
                break;
            case BunkerKinds.Storage:
                storageMgr.Init();
                storageMgr.OpenMainWin();
                currentWinId = (int)BunkerWindows.StorageWindow - 1;
                break;
        }
        windowManager.Open(currentWinId);
    }

    public void OpenConstructionWin()
    {
        if (!camController.isZoomIn)
        {
            upgradeMgr.Init();
            currentWinId = (int)BunkerWindows.UpgradeWindow - 1;
            windowManager.Open(currentWinId);
        }
    }

    public void CloseWindow()
    {
        if (currentWinId != -1)
        {
            nameTxt.text = "벙커";
            if (!mapButton.activeSelf) mapButton.SetActive(true);
            isWinOpen = false;

            windowManager.windows[currentWinId].Close();
            currentWinId = -1;
        }
    }

    public void Create(int index)
    {
        if (selectedBunker == null) return;
        if (createButton.activeSelf) createButton.SetActive(false);
        if(!destroyButton.activeSelf) destroyButton.SetActive(true);

        GameObject go = null;
        int currentLevel = 0;
        switch (index)
        {
            case 1:
                currentBunkerKind = BunkerKinds.Agit;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 1;
                go = Instantiate(agitPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.agitLevel;
                break;
            case 2:
                currentBunkerKind = BunkerKinds.Pub;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 2;
                go = Instantiate(pubPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.pubLevel;
                break;
            case 3:
                currentBunkerKind = BunkerKinds.Store;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 3;
                go = Instantiate(storePrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.storeLevel;
                break;
            case 4:
                currentBunkerKind = BunkerKinds.Hospital;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 4;
                go = Instantiate(hospitalPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.hospitalLevel;
                break;
            case 5:
                currentBunkerKind = BunkerKinds.Garage;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 5;
                go = Instantiate(garagePrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.garageLevel;
                break;
            case 6:
                currentBunkerKind = BunkerKinds.Storage;
                //playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 6;
                go = Instantiate(storagePrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.storageLevel;
                break;
        }
        //PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //string str = $"BunkerKind{currentBunkerIndex}";
        //PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var script = go.GetComponent<BunkerBase>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        //bunkerObjs[script.bunkerId] = go;

        //var child = bunkerObjs[script.bunkerId].transform.GetChild(0).gameObject;
        //child.GetComponent<TextMeshPro>().text = $"Lv{currentLevel}";
        camController.currentObject = selectedBunker;

        OpenWindow();
    }

    public void Destroy()
    {
        //if (selectedBunker == null) return;
        //if (!createButton.activeSelf) createButton.SetActive(true);
        //if (destroyButton.activeSelf) destroyButton.SetActive(false);

        //currentBunkerKind = BunkerKinds.None;

        //string str = $"BunkerKind{currentBunkerIndex}";
        //PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        //var go = Instantiate(emptyPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
        //var script = go.GetComponent<BunkerBase>();
        //script.bunkerId = currentBunkerIndex;

        //Destroy(selectedBunker);
        //selectedBunker = go;
        //bunkerObjs[script.bunkerId] = go;
        //camController.currentObject = selectedBunker;

        //CloseWindow();
    }

    public void ExitBunker()
    {
        if (!playerDataMgr.ableToExit) return;

        playerDataMgr.saveData.bunkerExitNum += 1;
        if (playerDataMgr.saveData.bunkerExitNum == 5)
        {
            playerDataMgr.saveData.storeReset = true;
            playerDataMgr.saveData.pubReset = true;
            playerDataMgr.saveData.bunkerExitNum = 0;
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        //SceneManager.LoadScene("TestNonBattleMap");
        SceneManager.LoadScene("NonBattleMap");
    }
    public void Upgrade(string name)
    {
        int upgradeCost = 0;
        switch (name)
        {
            case "Agit":
                if (playerDataMgr.saveData.agitLevel == 5) return;

                Bunker agitLevelInfo = playerDataMgr.bunkerList["BUN_0001"];
                switch (playerDataMgr.saveData.agitLevel)
                {
                    case 1:
                       upgradeCost = agitLevelInfo.level2Cost;
                        break;
                    case 2:
                       upgradeCost = agitLevelInfo.level3Cost;
                        break;
                    case 3:
                       upgradeCost = agitLevelInfo.level4Cost;
                        break;
                    case 4:
                       upgradeCost = agitLevelInfo.level5Cost;
                        break;
                }
                if (playerDataMgr.saveData.money - upgradeCost < 0) return;

                playerDataMgr.saveData.agitLevel++;
                playerDataMgr.saveData.money -= upgradeCost;
                PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                var child = bunkerObjs[4].transform.GetChild(0).gameObject;
                child.GetComponent<TextMeshPro>().text = $"Lv.{playerDataMgr.saveData.agitLevel}";

                agitMgr.Init();
                agitMgr.RefreshUpgradeWin();
                break;
            case "Storage":
                if (playerDataMgr.saveData.storageLevel == 5) return;

                Bunker storageLevelInfo = playerDataMgr.bunkerList["BUN_0002"];
                switch (playerDataMgr.saveData.storageLevel)
                {
                    case 1:
                        upgradeCost = storageLevelInfo.level2Cost;
                        break;
                    case 2:
                        upgradeCost = storageLevelInfo.level3Cost;
                        break;
                    case 3:
                        upgradeCost = storageLevelInfo.level4Cost;
                        break;
                    case 4:
                        upgradeCost = storageLevelInfo.level5Cost;
                        break;
                }
                if (playerDataMgr.saveData.money - upgradeCost < 0) return;

                playerDataMgr.saveData.storageLevel++;
                playerDataMgr.saveData.money -= upgradeCost;
                PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                child = bunkerObjs[5].transform.GetChild(0).gameObject;
                child.GetComponent<TextMeshPro>().text = $"Lv.{playerDataMgr.saveData.storageLevel}";

                storageMgr.Init();
                storageMgr.RefreshUpgradeWin();
                break;
            case "Pub":
                if (playerDataMgr.saveData.pubLevel == 5) return;

                Bunker pubSoldierLevelInfo = playerDataMgr.bunkerList["BUN_0007"];
                switch (playerDataMgr.saveData.pubLevel)
                {
                    case 1:
                        upgradeCost = pubSoldierLevelInfo.level2Cost;
                        break;
                    case 2:
                        upgradeCost = pubSoldierLevelInfo.level3Cost;
                        break;
                    case 3:
                        upgradeCost = pubSoldierLevelInfo.level4Cost;
                        break;
                    case 4:
                        upgradeCost = pubSoldierLevelInfo.level5Cost;
                        break;
                }
                if (playerDataMgr.saveData.money - upgradeCost < 0) return;

                playerDataMgr.saveData.pubLevel++;
                playerDataMgr.saveData.money -= upgradeCost;
                PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                child = bunkerObjs[2].transform.GetChild(0).gameObject;
                child.GetComponent<TextMeshPro>().text = $"Lv.{playerDataMgr.saveData.pubLevel}";

                pubMgr.Init();
                pubMgr.RefreshUpgradeWin();
                break;
            case "Store":
                if (playerDataMgr.saveData.storeLevel == 5) return;

                Bunker storeLevelInfo = playerDataMgr.bunkerList["BUN_0006"];
                switch (playerDataMgr.saveData.storeLevel)
                {
                    case 1:
                        upgradeCost = storeLevelInfo.level2Cost;
                        break;
                    case 2:
                        upgradeCost = storeLevelInfo.level3Cost;
                        break;
                    case 3:
                        upgradeCost = storeLevelInfo.level4Cost;
                        break;
                    case 4:
                        upgradeCost = storeLevelInfo.level5Cost;
                        break;
                }
                if (playerDataMgr.saveData.money - upgradeCost < 0) return;

                playerDataMgr.saveData.storeLevel++;
                playerDataMgr.saveData.money -= upgradeCost;
                PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

                child = bunkerObjs[3].transform.GetChild(0).gameObject;
                child.GetComponent<TextMeshPro>().text = $"Lv.{playerDataMgr.saveData.storeLevel}";

                storeMgr.Init();
                storeMgr.RefreshUpgradeWin();
                break;
        }
    }

    public void RefreshGoods()
    {
        int bullet5Num = 0;
        int bullet7Num = 0;
        int bullet9Num = 0;
        int bullet45Num = 0;
        int bullet12Num = 0;

        foreach (var element in playerDataMgr.currentOtherItems)
        {
            switch (element.Key)
            {
                case "BUL_0004":
                    bullet5Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0005":
                    bullet7Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0002":
                    bullet9Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0003":
                    bullet45Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
                case "BUL_0001":
                    bullet12Num += playerDataMgr.currentOtherItemsNum[element.Key];
                    break;
            }
        }
        bullet5Txt.text = $"{bullet5Num}";
        bullet7Txt.text = $"{bullet7Num}";
        bullet9Txt.text = $"{bullet9Num}";
        bullet45Txt.text = $"{bullet45Num}";
        bullet12Txt.text = $"{bullet12Num}";
        moneyTxt.text = $"{playerDataMgr.saveData.money}";
    }

    //창 관련.
    public void BunkerMenu()
    {
        isBunkerMenuOpen = !isBunkerMenuOpen;
        bunkerMenuAnim.SetBool("isOpen", isBunkerMenuOpen);
    }

    public void OpenPauseWin()
    {
        pauseWin.SetActive(true);
        isWinOpen = true;
    }

    public void ClosePauseWin()
    {
        pauseWin.SetActive(false);
        isWinOpen = false;
    }

    public void OpenOptionWin()
    {
        optionWin.SetActive(true);
    }

    public void CloseOptionWin()
    {
        optionWin.SetActive(false);
    }
}