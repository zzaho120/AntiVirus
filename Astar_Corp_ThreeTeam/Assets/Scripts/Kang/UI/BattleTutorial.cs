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
                title = "���� ��� �ʻ�ȭ";
                sb.Append("ĳ������ ����, AP, �⺻�������� Ȯ���Ҽ� �ֽ��ϴ�.\n");
                sb.Append("*���Ͽ� �� 6AP�� ����Ҽ� ������,\n");
                sb.Append("�ൿ���� �Ҹ�Ǵ� AP�� �ٸ��ϴ�.");
                break;
            case 1:
                title = "���� ����, ���, ��ư ����";
                sb.Append("�̵�, �����, ���ػ��, ��ų, ������ ��\n");
                sb.Append("���� �� �� �ִ� �ൿ���� �� �ִ� ���� �Դϴ�.\n");
                sb.Append("*�ൿ���� AP�� �Ҹ� �˴ϴ�.");
                break;
            case 2:
                title = "�þ�";
                sb.Append("ĳ���ʹ� �⺻������ �����Ǵ� �þ߸� ������ �̵��� ���� �� �׻� �þ߹����� ���Ͽ� �߰����� �þ߸� �� �� �ֽ��ϴ�.\n");
                sb.Append("�߰����� �þ߿��� �����, ���ػ�ݰ� ���� �ٸ� �ൿ�� �� �� �ֽ��ϴ�.");
                break;
            case 3:
                title = "�̵�";
                sb.Append("ĳ���͸� �����ϸ� �̵��� �� �ִ� Ÿ���� ������ �˴ϴ�.\n");
                sb.Append("�̵��ϱ� ���ϴ� Ÿ���� �� �� ������ ��ġ�ϰų� �ش�Ÿ���� ��ġ �� �����ϴ��� '�̵�'��ư�� ��ġ�Ͽ� �̵��� �� �ֽ��ϴ�.\n");
                sb.Append("�̵��� ���� �� ĳ���� 4���� ���� �� �� ���� �����Ͽ� �߰����� �þ߸� �� �� �ֽ��ϴ�.");
                break;
            case 4:
                title = "��� ���";
                sb.Append("������� �ڽ��� �þ� ���⿡�� �����̴� ���� ���ؼ��� ����ϴ� �ൿ�Դϴ�.\n");
                sb.Append("����� ��� �� �ش� ĳ���ʹ� ���� �����ϸ� ���� ���� ���ƿ� ������ ���� AP�� ����Ͽ� ����� �����ϸ�\n");
                sb.Append("1ȸ ��ݴ� �Ҹ��ϴ� AP���� �ѱ⸶�� �ٸ��ϴ�.");
                break;
            case 5:
                title = "���� ���";
                sb.Append("���ػ���̶� �߰��þ߿� ���̴� ���� �����Ͽ� ����մϴ�.\n");
                sb.Append("���⺰ ��ȿ��Ÿ��� �ٸ��� �Ÿ��� ���� ���߷� �г�Ƽ�� ������ �ֽ��ϴ�.");
                break;
            case 6:
                title = "���� ���";
                sb.Append("���ػ�� ��ư ��ġ �� ���� ��ݰ����� ���� ǥ���� ������ ���� ���� ������ ������ ǥ�õ˴ϴ�.\n");
                sb.Append("���� ��ġ�ϰų� �ϴ� �� �������� ��ġ���� �����ϴ��� ��ư�� ��ġ�Ͽ� �ൿ�� Ȯ���� �� �ֽ��ϴ�.");
                break;
            case 7:
                title = "��Ʈ �ý���";
                sb.Append("���̴� �ý����� �ָ��ִ� ������ ��ġ�� �˼��ֵ��� �����ִ� �ý����Դϴ�.\n");
                sb.Append("�÷��̾� �Ͻ��۽� ���̴��� �߻��Ǹ� ĳ���Ϳ� �������� ���� ���̴��� �߻���ŵ�ϴ�.");
                break;
            case 8:
                title = "��Ʈ �ý���";
                sb.Append("���ڱ�, ���� ������ Ȯ���Ͽ� ������ ���������� �̵������� Ȯ���Ҽ� �ֽ��ϴ�.");
                break;
            case 9:
                title = "���̷��� ����";
                sb.Append("���̷��� ���������� ������ �����ҽ� �ش緹����ŭ�� ������������ ���� �ްԵ˴ϴ�.\n");
                sb.Append("������ �������ε� ������������ ����Ҽ� ������ ���� ������ �ö󰥼��� ������ �Ҹ��� �г�Ƽ�� �ްԵǰ� ����� �̸��� �ֽ��ϴ�.\n");
                sb.Append("��, ĳ���ʹ� ������ ������ ������ ������������ ���ҵǴ� �縸ŭ ���� ����ġ�� ��Եǰ� ���������� ���� ���� ���� �������� �� ������ �ֽ��ϴ�.");
                break;
            case 10:
                title = "���� �غ�";
                sb.Append("ĳ���͸� �����ϸ� �̵��� �� �ִ� Ÿ���� ������ �˴ϴ�.\n");
                sb.Append("�̵��ϱ� ���ϴ� Ÿ���� �� �� ������ ��ġ�ϰų� �ش�Ÿ���� ��ġ �� �����ϴ��� '�̵�'��ư�� ��ġ�Ͽ� �̵��� �� �ֽ��ϴ�.\n");
                sb.Append("�̵��� ���� �� ĳ���� 4���� ���� �� �� ���� �����Ͽ� �߰����� �þ߸� �� �� �ֽ��ϴ�.");
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
