using System.Linq;
using TMPro;
using UnityEngine;

public class PrintTruckUI : MonoBehaviour
{
    private TextMeshProUGUI squadNum;
    private TextMeshProUGUI invenWeight;

    public int truckCurWeight;

    public void Init()
    {
        squadNum = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        invenWeight = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        // ���� �ο� / ���� �ִ� ���� �ο�
        // �κ��丮 �뷮
        if (PlayerDataMgr.Instance.saveData.currentCar == null)
        {
            Debug.Log("�ӽ� Ʈ�� ������ ���");
            squadNum.text = $"{PlayerDataMgr.Instance.boardingSquad.Count} / {ScriptableMgr.Instance.GetTruck("TRU_0001").capacity}";
            invenWeight.text = $"{truckCurWeight} / {ScriptableMgr.Instance.GetTruck("TRU_0001").weight}";
        }
        else
        {
            squadNum.text = $"{PlayerDataMgr.Instance.boardingSquad.Count} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].capacity}";
            invenWeight.text = $"{truckCurWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
        }
    }
}
