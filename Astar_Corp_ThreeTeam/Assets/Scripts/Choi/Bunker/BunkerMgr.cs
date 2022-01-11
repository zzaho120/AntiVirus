using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum BunkerKinds
{
    None,
    Agit,
    Pub,
    Store,
    CarCenter,
    Hospital,
    Garage,
}

public class BunkerMgr : MonoBehaviour
{
    public MultiTouch multiTouch;
    public BunkerCamController camController;
    public WindowManager windowManager;
    
    AgitMgr agitMgr;
    PubMgr pubMgr;
    StoreMgr storeMgr;
    HospitalMgr hospitalMgr;
    GarageMgr garageMgr;
    PlayerDataMgr playerDataMgr;

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
    public GameObject carCenterPrefab;
    public GameObject hospitalPrefab;
    public GameObject garagePrefab;
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

        agitMgr.playerDataMgr = playerDataMgr;
        pubMgr.playerDataMgr = playerDataMgr;
        hospitalMgr.playerDataMgr = playerDataMgr;
        garageMgr.playerDataMgr = playerDataMgr;
    }

    private void Start()
    {
        agitMgr.Init();
        pubMgr.Init();
        storeMgr.Init();
        hospitalMgr.Init();
        garageMgr.Init();

        camController.OpenWindow = OpenWindow;
        camController.CloseWindow = CloseWindow;
        camController.SetBunkerKind = SetBunkerKind;
        
        currentWinId = -1;
        currentBunkerKind = BunkerKinds.None;

        bunkerCount = bunkerObjs.Count;
        currentBunkerIndex = -1;

        if (!PlayerPrefs.HasKey("BunkerKind0"))
        {
            for (int i = 0; i < bunkerCount; i++)
            {
                string str = $"BunkerKind{i}";
                PlayerPrefs.SetInt(str, (int)BunkerKinds.None);
            }
            Debug.Log("no key");
        }
        else
        {
            Debug.Log("has key");
            for (int i = 0; i < bunkerCount; i++)
            {
                string str = $"BunkerKind{i}";
                int kind = PlayerPrefs.GetInt(str);
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
                    case BunkerKinds.CarCenter:
                        Create(4);
                        break;
                    case BunkerKinds.Hospital:
                        Create(5);
                        break;
                    case BunkerKinds.Garage:
                        Create(6);
                        break;
                }
            }
            CloseWindow();

            selectedBunker = null;


            currentBunkerIndex = -1;
            currentBunkerKind = BunkerKinds.None;

            if (destroyButton.activeSelf) destroyButton.SetActive(false);
        }
    }

    public void SetBunkerKind(Mode currentMode, GameObject bunker)
    {
        if (currentMode == Mode.Touch)
        {
            var script = bunker.GetComponent<BunkerBase>();
            currentBunkerIndex = script.bunkerId;
            var bunkerName = script.bunkerName;
            selectedBunker = bunker;

            //ÇöÀç º¡Ä¿ Á¾·ù.
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
                    case "CarCenter":
                        currentBunkerKind = BunkerKinds.CarCenter;
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

            //ÇöÀç º¡Ä¿ Á¾·ù.
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
                    case "CarCenter":
                        currentBunkerKind = BunkerKinds.CarCenter;
                        break;
                    case "Hospital":
                        currentBunkerKind = BunkerKinds.Hospital;
                        break;
                    case "Garage":
                        currentBunkerKind = BunkerKinds.Garage;
                        break;
                }
                Debug.Log("ºóº¡Ä¿ ¾Æ´Ô");
            }
            else
            {
                if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);

                currentBunkerKind = BunkerKinds.None;

                Debug.Log("ºóº¡Ä¿ÀÓ");
            }
        }
    }

    public void OpenWindow()
    {
        Debug.Log($"currentBunkerKind : {currentBunkerKind.ToString()}");
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
                currentWinId = (int)BunkerWindows.StoreWindow - 1;
                break;
            case BunkerKinds.CarCenter:
                currentWinId = (int)BunkerWindows.CarCenterWindow - 1;
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
        switch (index)
        {
            case 1:
                currentBunkerKind = BunkerKinds.Agit;
                go = Instantiate(agitPrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
            case 2:
                currentBunkerKind = BunkerKinds.Pub;
                go = Instantiate(pubPrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
            case 3:
                currentBunkerKind = BunkerKinds.Store;
                go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
            case 4:
                currentBunkerKind = BunkerKinds.CarCenter;
                go = Instantiate(carCenterPrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
            case 5:
                currentBunkerKind = BunkerKinds.Hospital;
                go = Instantiate(hospitalPrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
            case 6:
                currentBunkerKind = BunkerKinds.Garage;
                go = Instantiate(garagePrefab, selectedBunker.transform.position, Quaternion.identity);
                break;
        }

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var script = go.GetComponent<BunkerBase>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;
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

        var go = Instantiate(emptyPrefab, selectedBunker.transform.position, Quaternion.identity);
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
        SceneManager.LoadScene("NonBattleMap");
    }
}