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

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

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

                Debug.Log($"kind : {kind}");

                selectedBunker = bunkerObjs[i];
                currentBunkerIndex = i;
                currentBunkerKind = bunkerKinds;
                switch (bunkerKinds)
                {
                    case BunkerKinds.None:
                        break;
                    case BunkerKinds.Garden:
                        Debug.Log("Garden");
                        CreateGarden();
                        break;
                    case BunkerKinds.OperatingRoom:
                        Debug.Log("OperatingRoom");
                        CreateOperatingRoom();
                        break;
                    case BunkerKinds.Store:
                        Debug.Log("Store");
                        CreateStore();
                        break;
                }
                selectedBunker = null;
                currentBunkerIndex = -1;
                currentBunkerKind = BunkerKinds.None;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"currentWinId : {currentWinId}");

        if (multiTouch.Tap)
        {
            Ray ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                {
                    var script = hitInfo.collider.gameObject.GetComponent<BunkerBase>();
                    currentBunkerIndex = script.bunkerId;

                    selectedBunker = hitInfo.collider.gameObject;
                }

                //현재 벙커 종류.
                if (hitInfo.collider.gameObject.GetComponent<GardenRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.Garden;
                }
                else if (hitInfo.collider.gameObject.GetComponent<OperatingRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.OperatingRoom;
                }
                else if (hitInfo.collider.gameObject.GetComponent<StoreRoom>() != null)
                {
                    currentBunkerKind = BunkerKinds.Store;
                }
                else currentBunkerKind = BunkerKinds.None;
            }
        }

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

        if (!camController.isZoomIn && currentWinId != -1)
        {
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

        Debug.Log($"currentBunkerIndex : {currentBunkerIndex}");
        Debug.Log($"currentBunkerKind : {(int)currentBunkerKind}");

        var go = Instantiate(gardenPrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<GardenRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        currentWinId = (int)BunkerWindows.InventoryWindow - 1;
        windowManager.Open(currentWinId);
    }

    public void CreateOperatingRoom()
    {
        if (selectedBunker == null) return;
        currentBunkerKind = BunkerKinds.OperatingRoom;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        Debug.Log($"currentBunkerIndex : {currentBunkerIndex}");
        Debug.Log($"currentBunkerKind : {(int)currentBunkerKind}");

        var go = Instantiate(operatingRoomPrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<OperatingRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        currentWinId = (int)BunkerWindows.SquadWindow - 1;
        windowManager.Open(currentWinId);
    }

    public void CreateStore()
    {
        if (selectedBunker == null) return;
        currentBunkerKind = BunkerKinds.Store;

        string str = $"BunkerKind{currentBunkerIndex}";
        PlayerPrefs.SetInt(str, (int)currentBunkerKind);

        Debug.Log($"currentBunkerIndex : {currentBunkerIndex}");
        Debug.Log($"currentBunkerKind : {(int)currentBunkerKind}");

        var go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
        var script = go.GetComponent<StoreRoom>();
        script.bunkerId = currentBunkerIndex;

        Destroy(selectedBunker);
        selectedBunker = go;

        currentWinId = (int)BunkerWindows.StoreWindow - 1;
        windowManager.Open(currentWinId);
    }

    public void ExitBunker()
    {
        SceneManager.LoadScene(0);
    }
}