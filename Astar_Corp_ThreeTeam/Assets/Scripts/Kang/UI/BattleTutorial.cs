using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleTutorial : MonoBehaviour
{
    public int pageIdx;
    private int maxPageIdx = 11;
    public Text tutorialTitle;
    public TextMeshProUGUI turorialText;
    public Text pageIdxText;

    public void Init()
    {
        pageIdx = 0;
        UpdateTurorial();
    }


    public void UpdateTurorial()
    {
        var sb = new StringBuilder();
        var title = string.Empty;
        switch (pageIdx)
        {
            case 0:
                title = "좌측 상단 초상화";
                sb.Append("캐릭터의 상태, AP, 기본정보들을 확인할수 있습니다.\n");
                sb.Append("*한턴에 총 6AP를 사용할수 있으며,\n");
                sb.Append("행동별로 소모되는 AP가 다릅니다.");
                break;
            case 1:
                title = "왼쪽 조준, 경계, 버튼 모음";
                sb.Append("이동, 경계사격, 조준사격, 스킬, 아이템 등\n");
                sb.Append("실행 할 수 있는 행동들이 모여 있는 공간 입니다.\n");
                sb.Append("*행동에는 AP가 소모 됩니다.");
                break;
            case 2:
                title = "시야";
                sb.Append("캐릭터는 기본적으로 제공되는 시야를 가지고 이동이 끝난 후 항상 시야방향을 정하여 추가적인 시야를 볼 수 있습니다.\n");
                sb.Append("추가적인 시야에서 경계사격, 조준사격과 같이 다른 행동을 할 수 있습니다.");
                break;
            case 3:
                title = "이동";
                sb.Append("캐릭터를 선택하면 이동할 수 있는 타일이 나오게 됩니다.\n");
                sb.Append("이동하기 원하는 타일을 두 번 빠르게 터치하거나 해당타일을 터치 후 우측하단의 '이동'버튼을 터치하여 이동할 수 있습니다.\n");
                sb.Append("이동이 끝난 후 캐릭터 4가지 방향 중 한 곳을 선택하여 추가적인 시야를 볼 수 있습니다.");
                break;
            case 4:
                title = "경계 사격";
                sb.Append("경계사격은 자신의 시야 방향에서 움직이는 적에 대해서만 사격하는 행동입니다.\n");
                sb.Append("경계사격 명령 시 해당 캐릭터는 턴을 종료하며 다음 턴이 돌아올 때까지 남은 AP를 사용하여 사격을 진행하며\n");
                sb.Append("1회 사격당 소모하는 AP양은 총기마다 다릅니다.");
                break;
            case 5:
                title = "조준 사격";
                sb.Append("조준사격이란 추가시야에 보이는 적을 선택하여 사격합니다.\n");
                sb.Append("무기별 유효사거리가 다르며 거리에 따라 명중률 패널티를 받을수 있습니다.");
                break;
            case 6:
                title = "조준 사격";
                sb.Append("조준사격 버튼 터치 시 현재 사격가능한 적이 표시의 정보와 장착 중인 무기의 정보가 표시됩니다.\n");
                sb.Append("적을 터치하거나 하단 적 아이콘을 터치한후 우측하단의 버튼을 터치하여 행동을 확정할 수 있습니다.");
                break;
            case 7:
                title = "힌트 시스템";
                sb.Append("레이더 시스템은 멀리있는 몬스터의 위치를 알수있도록 도와주는 시스템입니다.\n");
                sb.Append("플레이어 턴시작시 레이더가 발생되며 캐릭터와 가까울수록 강한 레이더를 발생시킵니다.");
                break;
            case 8:
                title = "힌트 시스템";
                sb.Append("발자국, 혈흔 정보를 확인하여 몬스터의 상태정보와 이동정보를 확인할수 있습니다.");
                break;
            case 9:
                title = "바이러스 내성";
                sb.Append("바이러스 영역내에서 전투를 진행할시 해당레벨만큼의 감염게이지를 매턴 받게됩니다.\n");
                sb.Append("몬스터의 공격으로도 감염게이지가 상승할수 있으며 감염 레벨이 올라갈수록 전투에 불리한 패널티를 받게되고 사망에 이를수 있습니다.\n");
                sb.Append("단, 캐릭터는 내성을 가질수 있으며 감염게이지가 감소되는 양만큼 내성 경험치를 얻게되고 내성레벨에 따라 매턴 감염 게이지를 덜 받을수 있습니다.");
                break;
            case 10:
                title = "사전 준비";
                sb.Append("캐릭터를 선택하면 이동할 수 있는 타일이 나오게 됩니다.\n");
                sb.Append("이동하기 원하는 타일을 두 번 빠르게 터치하거나 해당타일을 터치 후 우측하단의 '이동'버튼을 터치하여 이동할 수 있습니다.\n");
                sb.Append("이동이 끝난 후 캐릭터 4가지 방향 중 한 곳을 선택하여 추가적인 시야를 볼 수 있습니다.");
                break;
        }

        tutorialTitle.text = title;
        turorialText.text = sb.ToString();
        pageIdxText.text = $"{pageIdx + 1} / {maxPageIdx + 1}";
    }

    public void OnClickNextPage()
    {
        pageIdx++;
        pageIdx = Mathf.Clamp(pageIdx, 0, maxPageIdx);
        UpdateTurorial();
    }

    public void OnClickPrevPage()
    {
        pageIdx--;
        pageIdx = Mathf.Clamp(pageIdx, 0, maxPageIdx + 1);
        UpdateTurorial();
    }
}
