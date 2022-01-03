using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BunkerKinds
{
    None,
    Garden,
    OperatingRoom,
    Store,
}

public class BunkerMgr : MonoBehaviour
{
    public MultiTouch multiTouch;
    public BunkerCamController camController;
    public WindowManager windowManager;
    public SquadMgr squadMgr;
    public InventoryMgr inventoryMgr;

    public Camera camera;
    GameObject selectedBunker;
    BunkerKinds currentBunkerKind;
    int currentWinId;

    public GameObject gardenPrefab;
    public GameObject operatingRoomPrefab;
    public GameObject storePrefab;
    public List<GameObject> bunkerObjs;

    int bunkerCount;
    int currentBunkerIndex;

    bool isOpenWin;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
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
                }
                Debug.Log($"{str} :{bunkerKinds.ToString()}");
            }
            selectedBunker = null;


            currentBunkerIndex = -1;
            currentBunkerKind = BunkerKinds.None;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (multiTouch.Tap && !camController.isZoomIn)
        {
            Ray ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {

                if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                {
                    Debug.Log("º¡Ä¿ Å¬¸¯");

                    var script = hitInfo.collider.gameObject.GetComponent<BunkerBase>();
                    currentBunkerIndex = script.bunkerId;

                    selectedBunker = hitInfo.collider.gameObject;
                }

                //ÇöÀç º¡Ä¿ Á¾·ù.
                if (hitInfo.collider.gameObject.GetComponent<GardenRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.Garden;
                    camController.isCurrentEmpty = false;
                    inventoryMgr.Init();
                }
                else if (hitInfo.collider.gameObject.GetComponent<OperatingRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.OperatingRoom;
                    camController.isCurrentEmpty = false;
                }
                else if (hitInfo.collider.gameObject.GetComponent<StoreRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.Store;
                    camController.isCurrentEmpty = false;
                }
                else
                {
                    currentBunkerKind = BunkerKinds.None;
                    camController.isCurrentEmpty = true;
                }
            }
        }
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
            currentWinId = (int)BunkerWindows.InventoryWindow - 1;
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
        currentBunkerKind = BunkerKinds.Garden;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(gardenPrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<GardenRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        OpenWindow();
    }

    public void CreateOperatingRoom()
    {
        if (selectedBunker == null) return;
        
        currentBunkerKind = BunkerKinds.OperatingRoom;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(operatingRoomPrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<OperatingRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        OpenWindow();
    }

    public void CreateStore()
    {
        if (selectedBunker == null) return;
        currentBunkerKind = BunkerKinds.Store;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        var go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<StoreRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        OpenWindow();
    }

    public void ExitBunker()
    {
        SceneManager.LoadScene(0);
    }
}