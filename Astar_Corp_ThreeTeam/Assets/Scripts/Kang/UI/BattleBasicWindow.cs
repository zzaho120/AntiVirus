using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Info
{
    Monster,
    Hint,
    Virus
}

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
    public GameObject changeWeaponBtn;

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
    public List<Sprite> virusSprite;

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
    public BattleInfoPanel battleinfoPanel;

    [Header("Move")]
    public GameObject moveBtn;
    public GameObject cancelBtn;

    [Header("Fire")]
    public GameObject monsterPanelPrefab;
    public GameObject firePanel;
    public GameObject notMonsterPanel;
    public GameObject monsterListPanel;
    public GameObject fireConfirmBtn;
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

    [Header("Turn End")]
    public GameObject turnEndBtn;

    [Header("Floating Info")]
    public BattleFloatingInfo battleFloatingInfoPrefab;
    public GameObject CharacterUI;
    private List<BattleFloatingInfo> battleFloatingInfoList = new List<BattleFloatingInfo>();

    [Header("Item")]
    public BattleItemPanel itemPanel;
    public List<BattleItem> itemBtns;
    public BattleItem curItem;
    public GameObject itemCancelBtn;

    [Header("Skill")]
    public GameObject skillPanel;
    public GameObject skillConfirmBtn;

    [Header("Name")]
    public List<string> names;
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
        fireConfirmBtn.SetActive(false);
        monsterListPanel.SetActive(false);
        directionBtns.SetActive(false);
        infoPanel.SetActive(false);
        leftDirectionPanel.SetActive(false);
        alertPanel.SetActive(false);
        alertConfirmBtn.SetActive(false);
        skillPanel.SetActive(false);
        skillConfirmBtn.SetActive(false);
        itemCancelBtn.SetActive(false);
        InitSquad();
        InitFloatingInfo();

        EventBusMgr.Subscribe(EventType.StartPlayer, StartTurn);
    }

    public void UpdateUI()
    {
        SetWeaponUI(selectedChar);
        SetActionBtn();
        UpdateMemberUI();
    }

    public void SetSelectedChar(PlayerableChar player)
    {
        BattleFloatingInfo info = null;
        if (selectedChar != null)
        {
            selectedChar.ReturnSightTile();
            info = GetFlotingInfo(selectedChar);
            if (info != null)
                info.isSelected = false;
        }

        selectedChar = player;
        info = GetFlotingInfo(selectedChar);
        if (info != null)
            info.isSelected = infoPanel.activeSelf;

        if (selectedChar.subWeapon == null)
            changeWeaponBtn.SetActive(false);
        else
            changeWeaponBtn.SetActive(true);

        var players = BattleMgr.Instance.playerMgr.playerableChars;
        var playerIdx = -1;
        for (var idx = 0; idx < players.Count; ++idx)
        {
            if (player == players[idx])
                playerIdx = idx;
        }

        state = CharacterState.Wait;
        var stats = selectedChar.characterStats;
        hpText.text = $"{stats.currentHp}/{stats.MaxHp}";
        levelText.text = $"Lv{stats.level}";
        nameText.text = $"{names[playerIdx]}";
        classImage.sprite = stats.character.icon;
        selectedChar.DisplaySightTile();
        UpdateUI();
    }

    public void UpdateMemberUI()
    {
        for (var idx = 0; idx < memberList.Count; ++idx)
        {
            memberList[idx].UpdateUI();
        }

        var players = BattleMgr.Instance.playerMgr.playerableChars;
        foreach (var player in players)
        {
            UpdateExtraInfo(player);
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
                break;
            }

            var weaponAPCount = weaponBulletList[i].transform.childCount;
            var bullet = 0;
            if ((stats.weapon.type == WeaponStats.WeaponType.Main && i == 0) || (stats.weapon.type == WeaponStats.WeaponType.Sub && i == 1))
                bullet = stats.weapon.MainWeaponBullet;
            else if ((stats.weapon.type == WeaponStats.WeaponType.Sub && i == 0) || (stats.weapon.type == WeaponStats.WeaponType.Main && i == 1))
                bullet = stats.weapon.SubWeaponBullet;

            var scale = Vector2.zero;
            if (i == 0)
            {
                switch (stats.weapon.curWeapon.bulletType)
                {
                    case 1:
                        scale = new Vector2(15f, 30f);
                        break;
                    case 2:
                        scale = new Vector2(13f, 26f);
                        break;
                    case 3:
                        scale = new Vector2(7f, 14f);
                        break;
                    case 4:
                        scale = new Vector2(9f, 18f);
                        break;
                    case 5:
                        scale = new Vector2(11f, 22f);
                        break;
                }
            }
            else if (i == 1)
            {
                switch (stats.weapon.otherWeapon.bulletType)
                {
                    case 1:
                        scale = new Vector2(15f, 30f);
                        break;
                    case 2:
                        scale = new Vector2(13f, 26f);
                        break;
                    case 3:
                        scale = new Vector2(7f, 14f);
                        break;
                    case 4:
                        scale = new Vector2(9f, 18f);
                        break;
                    case 5:
                        scale = new Vector2(11f, 22f);
                        break;
                }
            }

            for (var j = 0; j < weaponAPCount; ++j)
            {
                var obj = weaponBulletList[i].transform.GetChild(j);
                var rectTr = obj.GetComponent<RectTransform>();
                rectTr.sizeDelta = scale;
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

        if (alertPanel.activeSelf)
            OnClickAlert();

        selectedChar.ChangeWeaponObject();
        SetActionBtn();
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

                actionBtn.btnText.text = "한놈만\n쏜다";
                actionBtn.SetAP(activeSkill[idx].AP);

                var btn = actionBtn.GetComponent<Button>();

                btn.onClick.AddListener(OnClickSkill);
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

    public void InitFloatingInfo()
    {
        var battleMgr = BattleMgr.Instance;
        var players = battleMgr.playerMgr.playerableChars;
        var monsters = battleMgr.monsterMgr.monsters;

        foreach (var player in players)
        {
            var go = Instantiate(battleFloatingInfoPrefab, CharacterUI.transform);
            var info = go.GetComponent<BattleFloatingInfo>();
            info.Init(player);

            battleFloatingInfoList.Add(info);
        }
        foreach (var monster in monsters)
        {
            var go = Instantiate(battleFloatingInfoPrefab, CharacterUI.transform);
            var info = go.GetComponent<BattleFloatingInfo>();
            info.Init(monster);
            monster.info = info;
            battleFloatingInfoList.Add(info);
        }
    }

    public BattleFloatingInfo GetFlotingInfo(PlayerableChar player)
    {
        return battleFloatingInfoList.Find(info => info.player == player);
    }

    public void OnClickInfo()
    {
        if (state == CharacterState.Wait)
        {
            SetInfoPanel(!infoPanel.activeSelf);
            BattleMgr.Instance.ChangeUIMode(infoPanel.activeSelf);
            var info = GetFlotingInfo(selectedChar);
            if (info != null)
                info.isSelected = infoPanel.activeSelf;
        }
    }

    public void SetInfoPanel(bool enabled)
    {
        infoPanel.SetActive(enabled);

        if (enabled)
        {
            battleinfoPanel.Init();
        }

        if (selectedChar != null)
        {
            if (enabled)
                selectedChar.ReturnSightTile();
            else
                selectedChar.DisplaySightTile();
        }
    }
    public void OnClickMove()
    {
        
        if (selectedChar.AP > 0 && isTurn)
        {
            state = CharacterState.Move;
            selectedChar.MoveMode();
            directionBtns.SetActive(false);
            moveBtn.SetActive(false);
            actionPanel.SetActive(false);
            cancelBtn.SetActive(true);
        }
    }

    public void OnClickMoveCancel()
    {
        state = CharacterState.Wait;
        selectedChar.status = CharacterState.Wait;
        selectedChar.ReturnMoveTile();
        moveBtn.SetActive(true);
        actionPanel.SetActive(true);
        cancelBtn.SetActive(false);
        selectedChar.DisplaySightTile();
    }

    public void OnClickFire()
    {
        state = CharacterState.Attack;
        var isFullApMove = selectedChar.characterStats.buffMgr.GetBuffList(Stat.FullApMove).Count > 0;

        var weapon = selectedChar.characterStats.weapon;

        var shotAp = 0;
        if (weapon.fireCount > 0)
            shotAp = weapon.curWeapon.otherShotAp;
        else
            shotAp = weapon.curWeapon.firstShotAp;

        if ((selectedChar.AP >= shotAp || isFullApMove) && isTurn)
        {
            actionPanel.SetActive(false);
            moveBtn.SetActive(false);
            SetInfoPanel(false);
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
                fireConfirmBtn.SetActive(true);
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

        fireConfirmBtn.SetActive(false);

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
            SetInfoPanel(false);
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
        selectedChar.ReturnSightTile();
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
        selectedChar.DisplaySightTile();
    }

    public void OnClickTurnEnd()
    {
        SetInfoPanel(false);
        turnEndBtn.SetActive(false);
        BattleMgr.Instance.OnChangeTurn(null);
    }

    public void StartTurn(object[] empty)
    {
        turnEndBtn.SetActive(true);
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
            selectedChar.ReturnSightTile();
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
            UpdateUI();
        }
    }

    public void SetInfoText(Info info, GameObject gameObject)
    {
        Sprite sprite = null;
        switch (info)
        {
            case Info.Monster:
                var monster = gameObject.GetComponentInChildren<MonsterChar>();
                switch (monster.monsterStats.monster.name)
                {
                    case "Bear":
                        sprite = monsterSprite[0];
                        break;

                    case "Boar":
                        sprite = monsterSprite[1];
                        break;

                    case "Wolf":
                        sprite = monsterSprite[2];
                        break;

                    case "Spider":
                        sprite = monsterSprite[3];
                        break;

                    case "Jaguar":
                        sprite = monsterSprite[4];
                        break;

                    case "Tiger":
                        sprite = monsterSprite[5];
                        break;

                    case "Fox":
                        sprite = monsterSprite[6];
                        break;
                }
                battleinfoPanel.SetMonsterInfo(monster, sprite);
                break;
            case Info.Hint:
                battleinfoPanel.SetHintInfo(gameObject.GetComponent<HintBase>());
                break;
            case Info.Virus:
                var virusTile = gameObject.GetComponent<VirusBase>();
                switch (virusTile.virusName)
                {
                    case "E":
                        sprite = virusSprite[0];
                        break;
                    case "B":
                        sprite = virusSprite[1];
                        break;
                    case "P":
                        sprite = virusSprite[2];
                        break;
                    case "I":
                        sprite = virusSprite[3];
                        break;
                    case "T":
                        sprite = virusSprite[4];
                        break;
                }
                battleinfoPanel.SetVirusInfo(gameObject.GetComponent<VirusBase>(), sprite);
                break;
        }
    }

    public void CheckRemoveUI()
    {
        for (var idx = 0; idx  < battleFloatingInfoList.Count; ++idx)
        {
            if (battleFloatingInfoList[idx].CheckDestroy())
            {
                Destroy(battleFloatingInfoList[idx].gameObject);
                battleFloatingInfoList.RemoveAt(idx);
            }
        }
    }

    public void OnClickItem()
    {
        actionPanel.SetActive(false);
        moveBtn.SetActive(false);
        itemCancelBtn.SetActive(true);
        itemPanel.gameObject.SetActive(true);
        itemPanel.Init(selectedChar);
    }
    public void OnClickItemCancel()
    {
        actionPanel.SetActive(true);
        moveBtn.SetActive(true);
        itemCancelBtn.SetActive(false);
        itemPanel.gameObject.SetActive(false);
    }

    public void OnClickItemConfirm()
    {
        OnClickItemCancel();
    }

    public void OnClickSkill()
    {
        state = CharacterState.Attack;
        selectedChar.HMG_A1_Skill();
        actionPanel.SetActive(false);
        moveBtn.SetActive(false);
        skillPanel.SetActive(true);
        skillConfirmBtn.SetActive(true);

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

    public void OnClickSkillCancel()
    {
        state = CharacterState.Wait;
        //selectedChar.HMG_A1_SkillCancel();
        actionPanel.SetActive(true);
        moveBtn.SetActive(true);
        skillPanel.SetActive(false);
        monsterListPanel.SetActive(false);
        notMonsterPanel.SetActive(false);
        skillConfirmBtn.SetActive(false);
        SetInfoPanel(false);
    }

    public void OnClickSkillConfirm()
    {
        if (state == CharacterState.Attack)
        {
            selectedChar.PlayAttackAnim(targetMonster);
            targetMonster = null;
            state = CharacterState.Wait;
            OnClickSkillCancel();
            SetActionBtn();
            UpdateUI();
        }
    }
}
