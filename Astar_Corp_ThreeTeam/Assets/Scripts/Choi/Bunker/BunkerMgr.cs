using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum BunkerKinds
{
    None,
    Garden,
    OperatingRoom,
    Store,
    Garage,
}

public class BunkerMgr : MonoBehaviour
{
    public MultiTouch multiTouch;
    public BunkerCamController camController;
    public WindowManager windowManager;
    
    StoreMgr storeMgr;
    InventoryMgr inventoryMgr;
    SquadMgr squadMgr;
    TrunkMgr trunkMgr;
    BoardingMgr boardingMgr;
    HospitalMgr hospitalMgr;
    AgitMgr agitMgr;
    PubMgr pubMgr;
    PlayerDataMgr playerDataMgr;

    public GameObject createButton;
    public GameObject destroyButton;

    public Camera camera;
    public GameObject selectedBunker;
    public BunkerKinds currentBunkerKind;
    int currentWinId;

    public GameObject emptyPrefab;
    public GameObject gardenPrefab;
    public GameObject operatingRoomPrefab;
    public GameObject storePrefab;
    public GameObject garagePrefab;
    public List<GameObject> bunkerObjs;

    int bunkerCount;
    public int currentBunkerIndex;

    bool isOpenWin;

    public Mode currentMode;

    private void Awake()
    {
        playerDataMgr = GameObject.FindGameObjectWithTag("PlayerDataMgr").GetComponent<PlayerDataMgr>();
        storeMgr = GameObject.FindGameObjectWithTag("StoreMgr").GetComponent<StoreMgr>();
        inventoryMgr = GameObject.FindGameObjectWithTag("InventoryMgr").GetComponent<InventoryMgr>();
        squadMgr = GameObject.FindGameObjectWithTag("SquadMgr").GetComponent<SquadMgr>();
        trunkMgr = GameObject.FindGameObjectWithTag("GarbageMgr").GetComponent<TrunkMgr>();
        boardingMgr = GameObject.FindGameObjectWithTag("GarbageMgr").GetComponent<BoardingMgr>();
        hospitalMgr = GameObject.FindGameObjectWithTag("HospitalMgr").GetComponent<HospitalMgr>();
        agitMgr = GameObject.FindGameObjectWithTag("AgitMgr").GetComponent<AgitMgr>();
        pubMgr = GameObject.FindGameObjectWithTag("PubMgr").GetComponent<PubMgr>();

        inventoryMgr.playerDataMgr = playerDataMgr;
        squadMgr.playerDataMgr = playerDataMgr;
        trunkMgr.playerDataMgr = playerDataMgr;
        boardingMgr.playerDataMgr = playerDataMgr;
        hospitalMgr.playerDataMgr = playerDataMgr;
        agitMgr.playerDataMgr = playerDataMgr;
        pubMgr.playerDataMgr = playerDataMgr;
    }

    private void Start()
    {
        storeMgr.Init();
        inventoryMgr.Init();
        squadMgr.Init();
        trunkMgr.Init();
        boardingMgr.Init();
        hospitalMgr.Init();
        agitMgr.Init();
        pubMgr.Init();

        camController.OpenWindow = OpenWindow;
        camController.CloseWindow = CloseWindow;
        
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
                    case BunkerKinds.Garden:
                        CreateGarden();
                        break;
                    case BunkerKinds.OperatingRoom:
                        CreateOperatingRoom();
                        break;
                    case BunkerKinds.Store:
                        CreateStore();
                        break;
                    case BunkerKinds.Garage:
                        CreateGarage();
                        break;
                }
                Debug.Log($"{str} :{bunkerKinds.ToString()}");
            }
            selectedBunker = null;


            currentBunkerIndex = -1;
            currentBunkerKind = BunkerKinds.None;

            if (destroyButton.activeSelf) destroyButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (multiTouch.Tap) currentMode = Mode.Touch;
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) currentMode = Mode.Mouse;

        Ray ray;
        if (currentMode == Mode.Touch)
        {
            ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                {
                    if (camController.isZoomIn)
                    {
                        if(createButton.activeSelf) createButton.SetActive(false);
                    }

                    var script = hitInfo.collider.gameObject.GetComponent<BunkerBase>();
                    currentBunkerIndex = script.bunkerId;

                    selectedBunker = hitInfo.collider.gameObject;
                }

                //현재 벙커 종류.
                if (hitInfo.collider.gameObject.GetComponent<GardenRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Garden;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<OperatingRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.OperatingRoom;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<StoreRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Store;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<Garage>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Garage;
                    camController.isCurrentEmpty = false;
                }
                else
                {
                    if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);
                    
                    currentBunkerKind = BunkerKinds.None;
                    camController.isCurrentEmpty = true;
                }
            }
        }
        else if (currentMode == Mode.Mouse)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                {
                    if (camController.isZoomIn)
                    {
                        if (createButton.activeSelf) createButton.SetActive(false);
                    }

                    var script = hitInfo.collider.gameObject.GetComponent<BunkerBase>();
                    currentBunkerIndex = script.bunkerId;

                    selectedBunker = hitInfo.collider.gameObject;
                }

                //현재 벙커 종류.
                if (hitInfo.collider.gameObject.GetComponent<GardenRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Garden;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<OperatingRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.OperatingRoom;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<StoreRoom>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Store;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<Garage>() != null)
                {
                    if (!camController.isZoomIn && !destroyButton.activeSelf) destroyButton.SetActive(true);

                    currentBunkerKind = BunkerKinds.Garage;
                    camController.isCurrentEmpty = false;
                }
                else
                {
                    if (!camController.isZoomIn && !createButton.activeSelf) createButton.SetActive(true);
                    
                    currentBunkerKind = BunkerKinds.None;
                    camController.isCurrentEmpty = true;
                }
            }
        }
        currentMode = Mode.None;
    }

    public void OpenWindow()
    {
        if (camController.isZoomIn && currentBunkerKind == BunkerKinds.Store)
        {
            currentWinId = (int)BunkerWindows.StoreWindow - 1;
            windowManager.Open(currentWinId);
        }
        else if (camController.isZoomIn && currentBunkerKind == BunkerKinds.OperatingRoom)
        {
            currentWinId = (int)BunkerWindows.SquadWindow - 1;
            windowManager.Open(currentWinId);
        }
        else if (camController.isZoomIn && currentBunkerKind == BunkerKinds.Garden)
        {
            inventoryMgr.Refresh();
            currentWinId = (int)BunkerWindows.InventoryWindow - 1;
            windowManager.Open(currentWinId);
        }
        else if (camController.isZoomIn && currentBunkerKind == BunkerKinds.Garage)
        {
            inventoryMgr.Refresh();
            currentWinId = (int)BunkerWindows.GarageWindow - 1;
            windowManager.Open(currentWinId);
        }
    }

    public void OpenConstructionWin()
    {
        if (!camController.isZoomIn)
        {
            currentWinId = (int)BunkerWindows.ConstructionWindow - 1;
            windowManager.Open(currentWinId);
        }
    }

    public void CloseWindow()
    {
        if (/*!camController.isZoomIn &&*/ currentWinId != -1)
        {
            if (currentBunkerKind == BunkerKinds.OperatingRoom)
                squadMgr.characterListUI.SetActive(false);
            windowManager.windows[currentWinId].Close();
            currentWinId = -1;
        }
    }


    public void CreateGarden()
    {
        if (selectedBunker == null) return;
        if (createButton.activeSelf) createButton.SetActive(false);
        if(!destroyButton.activeSelf) destroyButton.SetActive(true);
        currentBunkerKind = BunkerKinds.Garden;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(gardenPrefab, selectedBunker.transform.position, Quaternion.identity);
        
        var script = go.GetComponent<GardenRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;
        camController.currentObject = selectedBunker;

        OpenWindow();
    }

    public void CreateOperatingRoom()
    {
        if (selectedBunker == null) return;
        if (createButton.activeSelf) createButton.SetActive(false);
        if (!destroyButton.activeSelf) destroyButton.SetActive(true);

        currentBunkerKind = BunkerKinds.OperatingRoom;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(operatingRoomPrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<OperatingRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;
        camController.currentObject = selectedBunker;

        OpenWindow();
    }

    public void CreateStore()
    {
        if (selectedBunker == null) return;
        if (createButton.activeSelf) createButton.SetActive(false);
        if (!destroyButton.activeSelf) destroyButton.SetActive(true);

        currentBunkerKind = BunkerKinds.Store;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<StoreRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;
        bunkerObjs[script.bunkerId] = go;
        camController.currentObject = selectedBunker;

        OpenWindow();
    }

    public void CreateGarage()
    {
        if (selectedBunker == null) return;
        if (createButton.activeSelf) createButton.SetActive(false);
        if (!destroyButton.activeSelf) destroyButton.SetActive(true);

        currentBunkerKind = BunkerKinds.Garage;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(garagePrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<Garage>();
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