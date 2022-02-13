using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfoPanel : MonoBehaviour
{
    public Image infoImage;
    public TextMeshProUGUI infoImageTitle;
    public TextMeshProUGUI explainText;

    public void SetMonsterInfo(MonsterChar monster, Sprite monsterSprite)
    {
        infoImage.gameObject.SetActive(true);
        var stats = monster.monsterStats;
        infoImage.sprite = monsterSprite;
        infoImageTitle.text = $"Lv{stats.virusLevel} {monster.ownerName}";
        var sb = new StringBuilder();
        sb.Append($"체력 : {stats.currentHp} / {stats.originMaxHp}\n");
        sb.Append($"공격력 : {stats.monster.minDmg} ~ {stats.monster.maxDmg}\n");
        sb.Append($"AP : {stats.monster.ap}");
        explainText.text = sb.ToString();
    }

    public void SetHintInfo(HintBase hint)
    {
        infoImage.gameObject.SetActive(true);
        switch (hint.type)
        {
            case HintType.Footprint:
                infoImage.sprite = BattleMgr.Instance.hintMgr.footprintSprite;
                infoImageTitle.text = "발자국";
                break;
            case HintType.Bloodprint:
                infoImage.sprite = BattleMgr.Instance.hintMgr.bloodSprite;
                infoImageTitle.text = "혈흔";
                break;
        }

        var sb = new StringBuilder();
        sb.Append($"몬스터 이름 : {hint.ownerName}\n");
        sb.Append($"남은 턴수 : {hint.lifeTime - BattleMgr.Instance.turnCount + 1}");
        switch (hint.type)
        {
            case HintType.Footprint:
                sb.Append($"\n방향 정보 : {hint.direction}");
                break;
            case HintType.Bloodprint:
                break;
        }
        explainText.text = sb.ToString();
    }
    public void SetVirusInfo(VirusBase virus, Sprite sprite)
    {
        infoImage.gameObject.SetActive(true);
        infoImage.sprite = sprite;
        infoImageTitle.text = string.Empty;
        var sb = new StringBuilder();
        sb.Append($"바이러스 이름 : {virus.virusName}\n");
        sb.Append($"바이러스 레벨 : {virus.virusLevel}\n");
        sb.Append($"틱당 게이지 상승량 : {virus.increasePerTic}");
        explainText.text = sb.ToString();
    }

    public void SetItemInfo(Consumable item)
    {
        infoImage.gameObject.SetActive(true);
        infoImage.sprite = item.img;
        infoImageTitle.text = string.Empty;
        var sb = new StringBuilder();
        sb.Append($"아이템 이름 : {item.storeName}\n");

        if (item.hpRecovery > 0)
        {
            sb.Append($"기능 : 체력 회복\n");
            sb.Append($"체력 회복량 : {item.hpRecovery}");
        }
        else if (item.virusGaugeDec > 0)
        {
            sb.Append($"기능 : 전체 감염 게이지 감소\n");
            sb.Append($"감염 게이지 감소량 : {item.virusGaugeDec}");
        }

        explainText.text = sb.ToString();
    }

    public void Init()
    {
        infoImage.sprite = null;
        infoImage.gameObject.SetActive(false);
        infoImageTitle.text = string.Empty;
        explainText.text = string.Empty;
    }
}
