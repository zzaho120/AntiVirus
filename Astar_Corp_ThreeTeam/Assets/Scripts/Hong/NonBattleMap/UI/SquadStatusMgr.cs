using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadStatusMgr : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject infectedCharContent;

    public Sprite[] hpImgs;

    // 캐릭터 오브젝트 저장용
    private Dictionary<int, GameObject> charGoList = new Dictionary<int, GameObject>();
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    public void Init()
    {
        int i = 0;
        foreach (var element in PlayerDataMgr.Instance.boardingSquad)
        {
            var character = Instantiate(characterPrefab, infectedCharContent.transform);

            // 캐릭터 초상화
            Image charImg = character.transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = PlayerDataMgr.Instance.currentSquad[element.Value].character.halfImg;

            // 캐릭터 상태들
            GameObject states = character.transform.GetChild(1).gameObject;

            // 0 풀피
            // 1 반피
            // 2 딸피

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
            charGoList.Add(element.Value, character); // 오브젝트 추가
            charKeyList.Add(num, element.Key);      // 캐릭터 키 추가
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
