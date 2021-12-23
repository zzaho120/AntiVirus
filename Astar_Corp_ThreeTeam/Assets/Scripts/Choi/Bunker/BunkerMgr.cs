using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bunker
{
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

        private void Start()
        {
            currentWinId = -1;
            currentBunkerKind = BunkerKinds.None;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"currentWinId : {currentWinId}");

            if (multiTouch.Tap)
            {
                Ray ray = camera.ScreenPointToRay(multiTouch.curTouchPos);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                    {
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
                currentWinId = (int)Windows.StoreWindow - 1;
                windowManager.Open(currentWinId);
            }
            else if (camController.isZoomIn && currentBunkerKind == BunkerKinds.OperatingRoom)
            {
                currentWinId = (int)Windows.SquadWindow - 1;
                windowManager.Open(currentWinId);
            }
            else if (camController.isZoomIn && currentBunkerKind == BunkerKinds.Garden)
            {
                currentWinId = (int)Windows.InventoryWindow - 1;
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

            var go = Instantiate(gardenPrefab, selectedBunker.transform.position, Quaternion.identity);
            Destroy(selectedBunker);
            selectedBunker = go;

            currentWinId = (int)Windows.InventoryWindow - 1;
            windowManager.Open(currentWinId);
        }

        public void CreateOperatingRoom()
        {
            if (selectedBunker == null) return;

            var go = Instantiate(operatingRoomPrefab, selectedBunker.transform.position, Quaternion.identity);
            Destroy(selectedBunker);
            selectedBunker = go;

            currentWinId = (int)Windows.SquadWindow - 1;
            windowManager.Open(currentWinId);
        }

        public void CreateStore()
        {
            if (selectedBunker == null) return;

            var go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
            Destroy(selectedBunker);
            selectedBunker = go;

            currentWinId = (int)Windows.StoreWindow - 1;
            windowManager.Open(currentWinId);
        }

        public void ExitBunker()
        {
            SceneManager.LoadScene(0);
        }
    }
}
