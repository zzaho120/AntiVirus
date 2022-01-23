using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public enum BunkerKinds
{
    None,
    Agit,
    Pub,
    Store,
    Hospital,
    Garage,
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
    UpgradeMgr upgradeMgr;

    public GameObject createButton;
    public GameObject destroyButton;

    public Camera camera;
    public GameObject selectedBunker;
    public BunkerKinds currentBunkerKind;
    int currentWinId;

    [Header("Prefabs")]
    public GameObject emptyPrefab;
    public GameObject agitPrefab;
    public GameObject pubPrefab;
    public GameObject storePrefab;
    //public GameObject carCenterPrefab;
    public GameObject hospitalPrefab;
    public GameObject garagePrefab;
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
        upgradeMgr = GameObject.Find("UpgradeMgr").GetComponent<UpgradeMgr>();

        agitMgr.playerDataMgr = playerDataMgr;
        pubMgr.playerDataMgr = playerDataMgr;
        storeMgr.playerDataMgr = playerDataMgr;
        hospitalMgr.playerDataMgr = playerDataMgr;
        garageMgr.playerDataMgr = playerDataMgr;
        carCenterMgr.playerDataMgr = playerDataMgr;
        upgradeMgr.playerDataMgr = playerDataMgr;
    }

    private void Start()
    {
        agitMgr.Init();
        pubMgr.Init();
        storeMgr.Init();
        hospitalMgr.Init();
        garageMgr.Init();
        carCenterMgr.Init();

        camController.OpenWindow = OpenWindow;
        camController.CloseWindow = CloseWindow;
        camController.SetBunkerKind = SetBunkerKind;

        currentWinId = -1;
        currentBunkerKind = BunkerKinds.None;

        bunkerCount = bunkerObjs.Count;
        currentBunkerIndex = -1;

        int count = playerDataMgr.saveData.bunkerKind.Count;
        for (int i=0; i<count; i++)
        {
            int kind = playerDataMgr.saveData.bunkerKind[i];
            BunkerKinds bunkerKinds = (BunkerKinds)kind;

            selectedBunker = bunkerObjs[i];
            currentBunkerIndex = i;
            currentBunkerKind = bunkerKinds;

            switch (bunkerKinds)
            {
                case BunkerKinds.None:
                    break;
                case BunkerKinds.Agit:
                    Create(1);
                    break;
                case BunkerKinds.Pub:
                    Create(2);
                    break;
                case BunkerKinds.Store:
                    Create(3);
                    break;
                case BunkerKinds.Hospital:
                    Create(4);
                    break;
                case BunkerKinds.Garage:
                    Create(5);
                    break;
                case BunkerKinds.Locked:
                    selectedBunker = bunkerObjs[i];
                    var go = Instantiate(lockedPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);

                    var script = go.GetComponent<BunkerBase>();
                    script.bunkerId = i;

                    Destroy(selectedBunker);
                    selectedBunker = null;
                    bunkerObjs[i] = go;

                    break;
            }
            var bunkerBase = bunkerObjs[currentBunkerIndex].GetComponent<BunkerBase>();
        }
        CloseWindow();
        selectedBunker = null;
        currentBunkerIndex = -1;
        currentBunkerKind = BunkerKinds.None;

        //if (playerDataMgr.isFirst)
        //{
        //    //랜덤으로 잠기도록.
        //    int[] randomIndexArr = new int[2];
        //    randomIndexArr[0] = Random.Range(0, 4);
        //    randomIndexArr[1] = Random.Range(4, 9);

        //    foreach (var element in randomIndexArr)
        //    {
        //        selectedBunker = bunkerObjs[element];
        //        var go = Instantiate(lockedPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);

        //        var script = go.GetComponent<BunkerBase>();
        //        script.bunkerId = element;

        //        Destroy(selectedBunker);
        //        selectedBunker = null;
        //        bunkerObjs[element] = go;

        //        playerDataMgr.saveData.bunkerKind[element] = 6;
        //        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        //    }
        //}

        if (destroyButton.activeSelf) destroyButton.SetActive(false);
    }

    public void SetBunkerKind(Mode currentMode, GameObject bunker)
    {
        if (currentMode == Mode.Touch)
        {
            var script = bunker.GetComponent<BunkerBase>();
            currentBunkerIndex = script.bunkerId;
            var bunkerName = script.bunkerName;
            selectedBunker = bunker;

            //현재 벙커 종류.
            if (!bunkerName.Equals("None"))
            {
                if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

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
                }
            }
            else
            {
                if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);

                currentBunkerKind = BunkerKinds.None;
            }
        }
        else if (currentMode == Mode.Mouse)
        {
            var script = bunker.GetComponent<BunkerBase>();
            currentBunkerIndex = script.bunkerId;
            var bunkerName = script.bunkerName;
            selectedBunker = bunker;

            //현재 벙커 종류.
            if (!bunkerName.Equals("None"))
            {
                if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

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
                }
            }
            else
            {
                if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);

                currentBunkerKind = BunkerKinds.None;
            }
        }
    }

    public void OpenWindow()
    {
        switch (currentBunkerKind)
        {
            case BunkerKinds.Agit:
                agitMgr.Init();
                currentWinId = (int)BunkerWindows.AgitWindow - 1;
                break;
            case BunkerKinds.Pub:
                currentWinId = (int)BunkerWindows.PubWindow - 1;
                break;
            case BunkerKinds.Store:
                storeMgr.Init();
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
                playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 1;
                go = Instantiate(agitPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.agitLevel;
                break;
            case 2:
                currentBunkerKind = BunkerKinds.Pub;
                playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 2;
                go = Instantiate(pubPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.pubLevel;
                break;
            case 3:
                currentBunkerKind = BunkerKinds.Store;
                playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 3;
                go = Instantiate(storePrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.storeLevel;
                break;
            case 4:
                currentBunkerKind = BunkerKinds.Hospital;
                playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 4;
                go = Instantiate(hospitalPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.hospitalLevel;
                break;
            case 5:
                currentBunkerKind = BunkerKinds.Garage;
                playerDataMgr.saveData.bunkerKind[currentBunkerIndex] = 5;
                go = Instantiate(garagePrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
                currentLevel = playerDataMgr.saveData.garageLevel;
                break;
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var script = go.GetComponent<BunkerBase>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;

        var child = bunkerObjs[script.bunkerId].transform.GetChild(0).gameObject;
        child.GetComponent<TextMeshPro>().text = $"Lv{currentLevel}";
        camController.currentObject = selectedBunker;

        OpenWindow();
    }

    public void Destroy()
    {
        if (selectedBunker == null) return;
        if (!createButton.activeSelf) createButton.SetActive(true);
        if (destroyButton.activeSelf) destroyButton.SetActive(false);

        currentBunkerKind = BunkerKinds.None;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(emptyPrefab, selectedBunker.transform.position, selectedBunker.transform.rotation);
        var script = go.GetComponent<BunkerBase>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;
        camController.currentObject = selectedBunker;

        CloseWindow();
    }

    public void ExitBunker()
    {
        if (!playerDataMgr.ableToExit) return;

        playerDataMgr.saveData.bunkerExitNum += 1;
        if (playerDataMgr.saveData.bunkerExitNum == 5)
        {
            playerDataMgr.saveData.storeReset = true;
            playerDataMgr.saveData.bunkerExitNum = 0;
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        //SceneManager.LoadScene("NonBattleMap");
        //SceneManager.LoadScene("NonBattleAsset");
    }
}