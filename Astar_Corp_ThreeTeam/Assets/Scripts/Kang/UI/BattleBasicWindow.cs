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
    public GameObject actionPanel;
    public List<BattleActionBtn> actionBtns;
    private List<BattleActionBtn> genActionBtns = new List<BattleActionBtn>(); 
    private List<GameObject> genBtnLists = new List<GameObject>(); 

    [Header("Sprites")]
    public List<Sprite> monsterSprite;

    [Header("Squads")]
    public GameObject memberPrefab;
    public GameObject squadList;

    [Header("Info Panel")]
    public GameObject infoPanel;

    [Header("Move")]
    public GameObject moveBtn;
    public GameObject cancelBtn;
    private bool isMove;

    [Header("Fire")]
    public GameObject monsterPanelPrefab;
    public GameObject firePanel;
    public GameObject notMonsterPanel;
    public GameObject monsterListPanel;
    public GameObject confirmBtn;
    private List<BattleMonsterPanel> monsterPanels = new List<BattleMonsterPanel>();
    public MonsterChar targetMonster;

    [Header("Direction")]
    public GameObject directionBtns;
    public GameObject leftDirectionPanel;
    public GameObject directionCancelBtn;

    public bool isTurn
    {
        get => BattleMgr.Instance.turn == BattleTurn.Player;
    }

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
        notMonsterPanel.SetActive(false);
        firePanel.SetActive(false);
        confirmBtn.SetActive(false);
        monsterListPanel.SetActive(false);
        directionBtns.SetActive(false);
        infoPanel.SetActive(false);
        leftDirectionPanel.SetActive(false);
        InitSquad();
    }

    public void UpdateUI()
    {
        SetWeaponUI(selectedChar);
        SetActionBtn();
    }

    public void SetSelectedChar(PlayerableChar player)
    {
        selectedChar = player;

        var stats = selectedChar.characterStats;
        hpText.text = $"{stats.currentHp}/{stats.MaxHp}";
        levelText.text = $"{stats.level}";
        nameText.text = $"{stats.characterName}";
        classImage.sprite = stats.character.icon;

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
        var isFullApMove = selectedChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        if (isFullApMove)
            actionBtns[0].SetAP(0);
        else if (weapon.fireCount == 0)
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

            memberPanel.Init(player[idx], this);
        }
    }

    public void OnClickInfo()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void OnClickMove()
    {
        if (selectedChar.AP > 0 && isTurn)
        {
            isMove = true;
            selectedChar.MoveMode();
            directionBtns.SetActive(false);
            moveBtn.SetActive(false);
            cancelBtn.SetActive(true);
        }
    }

    public void OnClickMoveCancel()
    {
        isMove = false;
        selectedChar.MoveMode();
        moveBtn.SetActive(true);
        cancelBtn.SetActive(false);
    }

    public void OnClickFire()
    {
        var isFullApMove = selectedChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        if ((selectedChar.AP > selectedChar.characterStats.weapon.curWeapon.firstShotAp || isFullApMove) && isTurn)
        {
            actionPanel.SetActive(false);
            firePanel.SetActive(true);
            moveBtn.SetActive(false);
            infoPanel.SetActive(false);
            directionBtns.SetActive(false);
            selectedChar.status = CharacterState.Attack;
            selectedChar.ReturnMoveTile();

            for (var idx = 0; idx < monsterPanels.Count; ++idx)
            {
                Destroy(monsterPanels[idx].gameObject);
            }
            monsterPanels.Clear();

            var monsterList = BattleMgr.Instance.sightMgr.GetMonsterInPlayerSight(selectedChar);
            if (monsterList.Count == 0)
            {
                notMonsterPanel.SetActive(true);
            }
            else
            {
                confirmBtn.SetActive(true);
                monsterListPanel.SetActive(true);
                for (var idx = 0; idx < monsterList.Count; ++idx)
                {
                    var go = Instantiate(monsterPanelPrefab, monsterListPanel.transform);
                    var monsterPanel = go.GetComponent<BattleMonsterPanel>();

                    monsterPanel.Init(selectedChar, monsterList[idx], monsterSprite);
                    monsterPanels.Add(monsterPanel);
                }
                targetMonster = monsterList[0];
                CameraController.Instance.SetCameraTrs(targetMonster.transform);
            }
        }
    }

    public void OnClickFireCancel()
    {
        actionPanel.SetActive(true);
        firePanel.SetActive(false);
        monsterListPanel.SetActive(false);
        moveBtn.SetActive(true);
        selectedChar.status = CharacterState.Wait;

        confirmBtn.SetActive(false);

        notMonsterPanel.SetActive(false);
    }

    public void OnClickFireConfirm()
    {
        selectedChar.ActionAttack(targetMonster);
        targetMonster = null;
        OnClickFireCancel();
        selectedChar.status = CharacterState.Wait;
        UpdateUI();
    }

    public void OnClickDirection()
    {
        if ((selectedChar.AP > 1 || isMove) && isTurn)
        {
            directionBtns.SetActive(true);
            leftDirectionPanel.SetActive(true);
            actionPanel.SetActive(false);
            moveBtn.SetActive(false);
            infoPanel.SetActive(false);
        }

        if (isMove)
            directionCancelBtn.SetActive(false);
        else
            directionCancelBtn.SetActive(true);
    }

    public void OnClickDirectionCancel()
    {
        actionPanel.SetActive(true);
        directionBtns.SetActive(false);
        leftDirectionPanel.SetActive(false);
        moveBtn.SetActive(true);
    }

    public void OnClickDirectionBtn(int direction)
    {
        selectedChar.SetDirection((DirectionType)(1 << direction));
        BattleMgr.Instance.sightMgr.UpdateFrontSight(selectedChar);

        var isFullApMove = selectedChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        if (isFullApMove)
            actionBtns[0].SetAP(0);

        if ((selectedChar.status == CharacterState.Alert || selectedChar.AP <= 0) && !isFullApMove)
            selectedChar.EndPlayer();
        else if (selectedChar.status == CharacterState.Move)
            selectedChar.WaitPlayer();

        if (!isMove)
            selectedChar.AP -= 1;
        isMove = false;
        OnClickDirectionCancel();
    }

    public void OnClickTurnEnd()
    {
        BattleMgr.Instance.OnChangeTurn(null);
    }
}
