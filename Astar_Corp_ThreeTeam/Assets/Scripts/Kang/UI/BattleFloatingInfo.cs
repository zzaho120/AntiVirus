using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFloatingInfo : MonoBehaviour
{
    public Slider hpBar;
    public List<Image> virusImageList;
    public List<GameObject> virusDetailList;
    public GameObject virusDetalObj;
    public List<Slider> virusBarList;
    public GameObject virusBarObj;

    public PlayerableChar player;
    public MonsterChar monster;
    public bool isSelected;

    public void Init(PlayerableChar player)
    {
        this.player = player;

        UpdateInfo();
    }

    public void Init(MonsterChar monster)
    {
        this.monster = monster;

        UpdateInfo();
    }

    public void InitUI()
    {
        foreach (var virusImage in virusImageList)
        {
            virusImage.gameObject.SetActive(false);
        }

        foreach (var virusDetail in virusDetailList)
        {
            virusDetail.SetActive(false);
        }
    }

    public void UpdateInfo()
    {
        InitUI();

        if (player != null)
        {
            var stats = player.characterStats;
            var virusPenalty = stats.virusPenalty;

            hpBar.value = (float)stats.currentHp / stats.MaxHp;
            foreach (var pair in virusPenalty)
            {
                var virus = pair.Value;

                if (!isSelected)
                {
                    virusDetalObj.SetActive(true);
                    switch (virus.virus.name)
                    {
                        case "E":
                            virusImageList[0].gameObject.SetActive(true);
                            break;
                        case "B":
                            virusImageList[1].gameObject.SetActive(true);
                            break;
                        case "P":
                            virusImageList[2].gameObject.SetActive(true);
                            break;
                        case "I":
                            virusImageList[3].gameObject.SetActive(true);
                            break;
                        case "T":
                            virusImageList[4].gameObject.SetActive(true);
                            break;
                    }
                }
                else
                {
                    virusDetalObj.SetActive(false);
                    switch (virus.virus.name)
                    {
                        case "E":
                            virusImageList[0].gameObject.SetActive(false);
                            virusDetailList[0].gameObject.SetActive(true);
                            virusBarList[0].value = (float)virus.penaltyGauge / virus.GetMaxGauge();
                            break;
                        case "B":
                            virusImageList[1].gameObject.SetActive(false);
                            virusDetailList[1].gameObject.SetActive(true);
                            virusBarList[1].value = (float)virus.penaltyGauge / virus.GetMaxGauge();
                            break;
                        case "P":
                            virusImageList[2].gameObject.SetActive(false);
                            virusDetailList[2].gameObject.SetActive(true);
                            virusBarList[2].value = (float)virus.penaltyGauge / virus.GetMaxGauge();
                            break;
                        case "I":
                            virusImageList[3].gameObject.SetActive(false);
                            virusDetailList[3].gameObject.SetActive(true);
                            virusBarList[3].value = (float)virus.penaltyGauge / virus.GetMaxGauge();
                            break;
                        case "T":
                            virusImageList[4].gameObject.SetActive(false);
                            virusDetailList[4].gameObject.SetActive(true);
                            virusBarList[4].value = (float)virus.penaltyGauge / virus.GetMaxGauge();
                            break;
                    }
                }
            }
        }
        else if (monster != null)
        {
            var stats = monster.monsterStats;
            hpBar.value = (float)stats.currentHp / stats.originMaxHp;
            virusBarObj.SetActive(false);
            virusDetalObj.SetActive(false);
        }
    }

    public void LateUpdate()
    {
        if (player != null)
            transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(1f, 1f, 0));
        else if (monster != null)
            transform.position = Camera.main.WorldToScreenPoint(monster.transform.position + new Vector3(1f, 1f, 0));

        UpdateInfo();
    }

    public bool CheckDestroy()
    {
        if (player != null)
        {
            if (player.characterStats.currentHp <= 0)
                return true;
        }    
        else if (monster != null)
        {
            if (monster.monsterStats.currentHp <= 0)
                return true;
        }

        return false;
    }
}
