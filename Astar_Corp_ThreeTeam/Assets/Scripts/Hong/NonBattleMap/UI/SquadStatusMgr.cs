using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadStatusMgr : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject infectedCharContent;

    public Sprite[] hpImgs;

    // ĳ���� ������Ʈ �����
    private Dictionary<int, GameObject> charGoList = new Dictionary<int, GameObject>();
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    public void Init()
    {
        int i = 0;
        foreach (var element in PlayerDataMgr.Instance.boardingSquad)
        {
            var character = Instantiate(characterPrefab, infectedCharContent.transform);

            // ĳ���� �ʻ�ȭ
            Image charImg = character.transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = PlayerDataMgr.Instance.currentSquad[element.Value].character.halfImg;

            // ĳ���� ���µ�
            GameObject states = character.transform.GetChild(1).gameObject;

            // 0 Ǯ��
            // 1 ����
            // 2 ����

            // Hp
            var hpImg = states.transform.GetChild(0).GetComponent<Image>();
            var hpRate = PlayerDataMgr.Instance.currentSquad[element.Value].currentHp / PlayerDataMgr.Instance.currentSquad[element.Value].MaxHp;
            if (hpRate >= 0.5)
                hpImg.sprite = hpImgs[0];
            else if (hpRate >= 0.2)
                hpImg.sprite = hpImgs[1];
            else
                hpImg.sprite = hpImgs[2];

            // Virus
            //var virusGroup = child.transform.GetChild(2).gameObject;
            string[] virusName = new string[5];
            virusName[0] = "E";
            virusName[1] = "B";
            virusName[2] = "P";
            virusName[3] = "I";
            virusName[4] = "T";
            for (int j = 1; j < virusName.Length + 1; j++)
            {
                if (PlayerDataMgr.Instance.currentSquad[element.Value].virusPenalty[virusName[j - 1]].penaltyLevel >= 1)
                    states.transform.GetChild(j).gameObject.SetActive(true);
                else states.transform.GetChild(j).gameObject.SetActive(false);
            }

            int num = i;
            charGoList.Add(element.Value, character); // ������Ʈ �߰�
            charKeyList.Add(num, element.Key);      // ĳ���� Ű �߰�
            i++;
        }
    }

    public void SquadUpdate()
    {
        if (charGoList.Count != 0)
        {
            foreach (var character in charGoList)
            {
                Destroy(character.Value);
            }
            charGoList.Clear();
        }
        if (charKeyList.Count != 0) charKeyList.Clear();

        Init();
    }
}
