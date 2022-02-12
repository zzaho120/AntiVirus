using TMPro;
using UnityEngine;

public class UIPrint : MonoBehaviour
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    private TextMeshProUGUI squadNum;
    private TextMeshProUGUI invenWeight;

    void Start()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        squadNum = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        invenWeight = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        // ���� �ο� / ���� �ִ� ���� �ο�
        squadNum.text = $"{playerDataMgr.boardingSquad.Count} / {playerDataMgr.truckList[playerDataMgr.saveData.currentCar].capacity}";

        // �κ��丮 �뷮
        //invenWeight.text = $"{playerDataMgr.truckList[playerDataMgr.saveData.currentCar].weight} / {soMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
    }
}
