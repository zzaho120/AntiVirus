using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicWindow : GenericWindow
{
    public PlayerableChar selectedChar;
    public CharacterState state;

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
    private List<BattleMemberPanel> memberList = new List<BattleMemberPanel>();

    [Header("Sqauds Extra Info")]
    public GameObject extraInfoPrefab;
    public GameObject extraInfoPanel;
    private List<BattleMemberExtraInfo> extraInfoList = new List<BattleMemberExtraInfo>();

    [Header("Info Panel")]
    public GameObject infoPanel;

    [Header("Move")]
    public GameObject moveBtn;
    public GameObject cancelBtn;

    [Header("Fire")]
    public GameObject monsterPanelPrefab;
    public GameObject firePanel;
    public GameObject notMonsterPanel;
    public GameObject monsterListPanel;
    public GameObject fireAttackBtn;
    private List<BattleMonsterPanel> monsterPanels = new List<BattleMonsterPanel>();
    public MonsterChar targetMonster;

    [Header("Direction")]
    public GameObject directionBtns;
    public GameObject leftDirectionPanel;
    public GameObject directionCancelBtn;

    [Header("Alert")]
    public GameObject alertPanel;
    public GameObject attackTimeObj;
    public GameObject alertConfirmBtn;
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
        state = CharacterState.Wait;
        cancelBtn.SetActive(false);
        notMonsterPanel.SetActive(false);
        firePanel.SetActive(false);
        fireAttackBtn.SetActive(false);
        monsterListPanel.SetActive(false);
        directionBtns.SetActive(false);
        infoPanel.SetActive(false);
        leftDirectionPanel.SetActive(false);
        alertPanel.SetActive(false);
        alertConfirmBtn.SetActive(false);
        InitSquad();
    }

    public void UpdateUI()
    {
        SetWeaponUI(selectedChar);
        SetActionBtn();
        UpdateMemberUI();
    }

    public void SetSelectedChar(PlayerableChar player)
    {
        selectedChar = player;
        state = CharacterState.Wait;
        var stats = selectedChar.characterStats;
        hpText.text = $"{stats.currentHp}/{stats.MaxHp}";
        levelText.text = $"Lv{stats.level}";
        nameText.text = $"{stats.characterName}";
        classImage.sprite = stats.character.icon;

        UpdateUI();
    }

    public void UpdateMemberUI()
    {
        for (var idx = 0; idx < memberList.Count; ++idx)
        {
            memberList[idx].UpdateUI();
        }
    }
    public void SetWeaponUI(PlayerableChar player)
    {
        var stats = player.characterStats;
        weaponImages[0].sprite = stats.weapon.curWeapon.img;
        if (stats.weapon.otherWeapon != null)
            weaponImages[1].sprite = stats.weapon.otherWeapon.img;

        for (var i = 0; i < 2; ++i)
        {
            if (i == 1 && stats.weapon.otherWeapon == null)
            {
                var childCount = weaponBulletList[i].transform.childCount;
                for (var j = 0; j < childCount; ++j)
                {
                    var obj = weaponBulletList[i].transform.GetChild(j);
                    obj.gameObject.SetActive(false);
                }
            }

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


        if (weapon.CheckAvailBullet)
        {
            actionBtns[0].gameObject.SetActive(true);
            actionBtns[1].gameObject.SetActive(true);
        }
        else
        {
            actionBtns[0].gameObject.SetActive(false);
            actionBtns[1].gameObject.SetActive(false);
        }

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

        if (weapon.WeaponBullet == weapon.curWeapon.bullet)
            actionBtns[3].gameObject.SetActive(false);
        else
            actionBtns[3].gameObject.SetActive(true);

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

                actionBtn.btnText.text = activeSkill[idx].skillName;
                Debug.Log($"text : {actionBtn.btnText.text}");
                actionBtn.SetAP(activeSkill[idx].AP);
            }
        }
    }

    public void InitSquad()
    {
        var player = BattleMgr.Instance.playerMgr.playerableChars;
        
        for (var idx = 0; idx < player.Count; ++idx)
        {
            var memberGo = Instantiate(memberPrefab, squadList.transform);
            var extraGo = Instantiate(extraInfoPrefab, extraInfoPanel.transform);
            var memberPanel = memberGo.GetComponent<BattleMemberPanel>();
            var extraInfo = extraGo.GetComponent<BattleMemberExtraInfo>();

            memberPanel.Init(player[idx], this);
            memberList.Add(memberPanel);

            extraInfo.Init(player[idx]);
            extraInfoList.Add(extraInfo);
        }
    }

    public void OnClickInfo()
    {
        if (state == CharacterState.Wait)
            infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void OnClickMove()
    {
        
        if (selectedChar.AP > 0 && isTurn)
        {
            state = CharacterState.Move;
            selectedChar.MoveMode();
            directionBtns.SetActive(false);
            moveBtn.SetActive(false);
            cancelBtn.SetActive(true);
        }
    }

    public void OnClickMoveCancel()
    {
        state = CharacterState.Wait;
        selectedChar.status = CharacterState.Wait;
        selectedChar.ReturnMoveTile();
        moveBtn.SetActive(true);
        cancelBtn.SetActive(false);
    }

    public void OnClickFire()
    {
        state = CharacterState.Attack;
        var isFullApMove = selectedChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        if ((selectedChar.AP >= selectedChar.characterStats.weapon.curWeapon.firstShotAp || isFullApMove) && isTurn)
        {
            actionPanel.SetActive(false);
            moveBtn.SetActive(false);
            infoPanel.SetActive(false);
            directionBtns.SetActive(false);
            firePanel.SetActive(true);
            firePanel.GetComponent<BattleFirePanel>().SetDmgText(selectedChar);
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
                fireAttackBtn.SetActive(true);
                monsterListPanel.SetActive(true);
                for (var idx = 0; idx < monsterList.Count; ++idx)
                {
                    var go = Instantiate(monsterPanelPrefab, monsterListPanel.transform);
                    var monsterPanel = go.GetComponent<BattleMonsterPanel>();

                    monsterPanel.Init(selectedChar, monsterList[idx], monsterSprite, this);
                    
                    monsterPanels.Add(monsterPanel);
                }
                targetMonster = monsterList[0];
                CameraController.Instance.SetCameraTrs(targetMonster.transform);
            }
        }
    }

    public void OnClickFireCancel()
    {
        state = CharacterState.Wait;
        actionPanel.SetActive(true);
        firePanel.SetActive(false);
        monsterListPanel.SetActive(false);
        moveBtn.SetActive(true);
        selectedChar.status = CharacterState.Wait;

        fireAttackBtn.SetActive(false);

        notMonsterPanel.SetActive(false);
    }

    public void OnClickFireConfirm()
    {
        if (state == CharacterState.Attack)
        {
            selectedChar.PlayAttackAnim(targetMonster);
            targetMonster = null;
            OnClickFireCancel();
            selectedChar.status = CharacterState.Wait;
            SetActionBtn();
            UpdateUI();
        }
    }

    public void OnClickDirection()
    {
        if ((selectedChar.AP > 0 ||
        state == CharacterState.Move) && isTurn)
        {
            directionBtns.SetActive(true);
            leftDirectionPanel.SetActive(true);
            actionPanel.SetActive(false);
            moveBtn.SetActive(false);
            infoPanel.SetActive(false);
        }

        if (state == CharacterState.Move)
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

        if (state == CharacterState.Wait)
        {
            selectedChar.AP -= 1;
            UpdateUI();
        }
        state = CharacterState.Wait;
        OnClickDirectionCancel();
    }

    public void OnClickTurnEnd()
    {
        BattleMgr.Instance.OnChangeTurn(null);
    }

    public void OnClickAlert()
    {
        state = CharacterState.Alert;
        actionPanel.SetActive(false);
        alertPanel.SetActive(true);
        alertConfirmBtn.SetActive(true);
        var childCount = attackTimeObj.transform.childCount;
        var attackCount = selectedChar.AP / selectedChar.characterStats.weapon.curWeapon.firstShotAp;
        for (var idx = 0; idx < childCount; ++idx)
        {
            var child = attackTimeObj.transform.GetChild(idx);
            if (idx < attackCount)
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }

        moveBtn.SetActive(false);
    }

    public void OnClickAlertCancel()
    {
        state = CharacterState.Wait;
        alertPanel.SetActive(false);
        alertConfirmBtn.SetActive(false);
        actionPanel.SetActive(true);
        moveBtn.SetActive(true);
    }

    public void OnClickAlertConfirm()
    {
        if (state == CharacterState.Alert)
        {
            selectedChar.AlertMode();
            OnClickAlertCancel();
            UpdateUI();
            UpdateExtraInfo(selectedChar);
        }
    }

    public void UpdateExtraInfo(PlayerableChar player)
    {
        foreach (var extra in extraInfoList)
        {
            if (extra.player == player)
                extra.UpdateExtraInfo();
        }
    }

    public void OnClickReload()
    {
        if (state == CharacterState.Wait)
        {
            selectedChar.ReloadWeapon();
            SetWeaponUI(selectedChar);
            SetActionBtn();
        }
    }
}
