using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicWindow : GenericWindow
{
    public PlayerableChar selectedChar;

    [Header("Selected Character Weapon")]
    public List<Image> weaponImages;
    public List<GameObject> weaponBulletList;

    [Header("Selected Character Info")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public Image classImage;

    [Header("Selected Character Action")]
    public GameObject actionBtnPrefab;
    public GameObject btnListPrefab;
    public GameObject secondBtnList;
    public GameObject btnListObj;
    public List<BattleActionBtn> actionBtns;
    private List<BattleActionBtn> genActionBtns = new List<BattleActionBtn>(); 
    private List<GameObject> genBtnLists = new List<GameObject>(); 

    [Header("Sprites")]
    public List<Sprite> classSprites;

    [Header("Squads")]
    public GameObject memberPrefab;
    public GameObject squadList;

    [Header("Info Panel")]
    public GameObject infoPanel;

    [Header("Move")]
    public GameObject moveBtn;
    public GameObject cancelBtn;

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void Init()
    {
        cancelBtn.SetActive(false);
        InitSquad();
    }

    public void UpdateUI()
    {
        SetWeaponUI(selectedChar);
        SetActionBtn();
    }

    public void SetSelectedChar(PlayerableChar player)
    {
        if (selectedChar != null)
            selectedChar.isSelected = false;
        selectedChar = player;
        selectedChar.isSelected = true;
        var stats = selectedChar.characterStats;
        hpText.text = $"{stats.currentHp}/{stats.MaxHp}";
        levelText.text = $"{stats.level}";
        nameText.text = $"{stats.Name}";

        switch (stats.character.type)
        {
            case "Tanker":
                classImage.sprite = classSprites[3];
                break;
            case "Healer":
                classImage.sprite = classSprites[1];
                break;
            case "Sniper":
                classImage.sprite = classSprites[2];
                break;
            case "Bombardier":
                classImage.sprite = classSprites[4];
                break;
            case "Scout":
                classImage.sprite = classSprites[0];
                break;
        }

        UpdateUI();
    }

    public void SetWeaponUI(PlayerableChar player)
    {
        var stats = player.characterStats;
        weaponImages[0].sprite = stats.weapon.curWeapon.img;
        weaponImages[1].sprite = stats.weapon.otherWeapon.img;

        for (var i = 0; i < 2; ++i)
        {
            var weaponAPCount = weaponBulletList[i].transform.childCount;
            var bullet = 0;
            if ((stats.weapon.type == WeaponStats.WeaponType.Main && i == 0) || (stats.weapon.type == WeaponStats.WeaponType.Sub && i == 1))
                bullet = stats.weapon.MainWeaponBullet;
            else if ((stats.weapon.type == WeaponStats.WeaponType.Sub && i == 0) || (stats.weapon.type == WeaponStats.WeaponType.Main && i == 1))
                bullet = stats.weapon.SubWeaponBullet;

            for (var j = 0; j < weaponAPCount; ++j)
            {
                var obj = weaponBulletList[i].transform.GetChild(j);

                if (bullet < j + 1)
                    obj.gameObject.SetActive(false);
                else
                    obj.gameObject.SetActive(true);
            }
        }
    }

    public void OnChangeWeapon()
    {
        var stats = selectedChar.characterStats;
        stats.weapon.ChangeWeapon();
        SetWeaponUI(selectedChar);
    }

    private void SetActionBtn()
    {
        // 0 조준
        // 1 경계
        // 2 방향전환
        // 3 재장전
        // 4 아이템
        var stats = selectedChar.characterStats;
        var weapon = stats.weapon;

        if (weapon.fireCount == 0)
        {
            actionBtns[0].SetAP(weapon.curWeapon.firstShotAp);
            actionBtns[1].SetAP(weapon.curWeapon.firstShotAp);
        }
        else
        {
            actionBtns[0].SetAP(weapon.curWeapon.otherShotAp);
            actionBtns[1].SetAP(weapon.curWeapon.otherShotAp);
        }

        actionBtns[2].SetAP(1);

        actionBtns[3].SetAP(weapon.curWeapon.loadAp);

        actionBtns[4].SetAP(0);

        var activeSkill = stats.skillMgr.activeSkills;

        for (var idx = 0; idx < genActionBtns.Count; ++idx)
        {
            Destroy(genActionBtns[idx].gameObject);
        }
        genActionBtns.Clear();

        for (var idx = 0; idx < genBtnLists.Count; ++idx)
        {
            Destroy(genBtnLists[idx]);
        }
        genBtnLists.Clear();

        if (activeSkill.Count > 0)
        {
            for (var idx = 0; idx < activeSkill.Count; ++idx)
            {
                GameObject go = null;
                if (genActionBtns.Count == 0)
                    go = Instantiate(actionBtnPrefab, secondBtnList.transform);
                else
                {
                    var btnList = Instantiate(btnListPrefab, btnListObj.transform);
                    go = Instantiate(actionBtnPrefab, btnList.transform);
                    genBtnLists.Add(btnList);
                }

                var actionBtn = go.GetComponent<BattleActionBtn>();
                genActionBtns.Add(actionBtn);

                actionBtn.btnText.buttonText = activeSkill[idx].skillName;
                Debug.Log($"text : {actionBtn.btnText.buttonText}");
                actionBtn.SetAP(activeSkill[idx].AP);
            }
        }
    }

    public void InitSquad()
    {
        var player = BattleMgr.Instance.playerMgr.playerableChars;
        
        for (var idx = 0; idx < player.Count; ++idx)
        {
            var go = Instantiate(memberPrefab, squadList.transform);
            var memberPanel = go.GetComponent<BattleMemberPanel>();

            memberPanel.Init(player[idx], classSprites, this);
        }
    }

    public void OnClickInfo()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void OnClickMove()
    {
        selectedChar.MoveMode();
        moveBtn.SetActive(false);
        cancelBtn.SetActive(true);
    }

    public void OnClickMoveCancel()
    {
        selectedChar.MoveMode();
        moveBtn.SetActive(true);
        cancelBtn.SetActive(false);
    }
}
