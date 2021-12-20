using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerMgr : MonoBehaviour
{
    public Camera camera;
    GameObject selectedBunker;

    public GameObject gardenPrefab;
    public GameObject operatingRoomPrefab;
    public GameObject storePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<BunkerBase>() != null)
                {
                    //var render = hitInfo.collider.gameObject.GetComponent<MeshRenderer>();
                    //render.material.color = Color.red;
                    selectedBunker = hitInfo.collider.gameObject;
                }
            }
            //else selectedBunker = null;
        }
    }

    public void CreateGarden()
    {
        if (selectedBunker == null) return;

        var go = Instantiate(gardenPrefab, selectedBunker.transform.position, Quaternion.identity);
        Destroy(selectedBunker);
        selectedBunker = go;
    }

    public void CreateOperatingRoom()
    {
        if (selectedBunker == null) return;

        var go = Instantiate(operatingRoomPrefab, selectedBunker.transform.position, Quaternion.identity);
        Destroy(selectedBunker);
        selectedBunker = go;
    }

    public void CreateStore()
    {
        if (selectedBunker == null) return;

        var go = Instantiate(storePrefab, selectedBunker.transform.position, Quaternion.identity);
        Destroy(selectedBunker);
        selectedBunker = go;
    }
}
