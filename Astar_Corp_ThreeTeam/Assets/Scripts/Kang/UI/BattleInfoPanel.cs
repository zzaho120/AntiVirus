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
        sb.Append($"ü�� : {stats.currentHp} / {stats.originMaxHp}\n");
        sb.Append($"���ݷ� : {stats.monster.minDmg} ~ {stats.monster.maxDmg}\n");
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
                infoImageTitle.text = "���ڱ�";
                break;
            case HintType.Bloodprint:
                infoImage.sprite = BattleMgr.Instance.hintMgr.bloodSprite;
                infoImageTitle.text = "����";
                break;
        }

        var sb = new StringBuilder();
        sb.Append($"���� �̸� : {hint.ownerName}\n");
        sb.Append($"���� �ϼ� : {hint.lifeTime - BattleMgr.Instance.turnCount + 1}");
        switch (hint.type)
        {
            case HintType.Footprint:
                sb.Append($"\n���� ���� : {hint.direction}");
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
        sb.Append($"���̷��� �̸� : {virus.virusName}\n");
        sb.Append($"���̷��� ���� : {virus.virusLevel}\n");
        sb.Append($"ƽ�� ������ ��·� : {virus.increasePerTic}");
        explainText.text = sb.ToString();
    }

    public void SetItemInfo(Consumable item)
    {
        infoImage.gameObject.SetActive(true);
        infoImage.sprite = item.img;
        infoImageTitle.text = string.Empty;
        var sb = new StringBuilder();
        sb.Append($"������ �̸� : {item.storeName}\n");

        if (item.hpRecovery > 0)
        {
            sb.Append($"��� : ü�� ȸ��\n");
            sb.Append($"ü�� ȸ���� : {item.hpRecovery}");
        }
        else if (item.virusGaugeDec > 0)
        {
            sb.Append($"��� : ��ü ���� ������ ����\n");
            sb.Append($"���� ������ ���ҷ� : {item.virusGaugeDec}");
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
