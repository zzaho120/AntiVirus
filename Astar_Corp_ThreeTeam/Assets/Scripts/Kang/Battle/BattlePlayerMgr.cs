using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerMgr : MonoBehaviour
{
    public List<PlayerableChar> playerableChars;

    public void Init()
    {
        playerableChars.Clear();
        var playerArr = transform.GetComponentsInChildren<PlayerableChar>();
        foreach (var player in playerArr)
        {
            player.Init();
            playerableChars.Add(player);
        }

        EventBusMgr.Subscribe(EventType.StartPlayer, StartTurn);
        EventBusMgr.Subscribe(EventType.EndPlayer, CheckEndTurn);
        EventBusMgr.Subscribe(EventType.DetectAlert, DetectAlert);
    }

    public void StartTurn(object empty)
    {
        foreach (var character in playerableChars)
        {
            character.StartTurn();
            CalculateVirusAllChar(character);
        }
    }

    public void CheckEndTurn(object empty)
    {
        var turnEndCount = 0;
        foreach (var player in playerableChars)
        {
            if (player.status == CharacterState.TurnEnd || player.status == CharacterState.Alert)
                turnEndCount++;
        }

        if (turnEndCount == playerableChars.Count)
            EventBusMgr.Publish(EventType.ChangeTurn);
    }

    public void DetectAlert(object[] param)
    {
        var boolList = (bool[])param[0];
        var curMonster = (MonsterChar)param[1];
        for (var playerIdx = 0; playerIdx < playerableChars.Count; ++playerIdx)
        {
            var player = playerableChars[playerIdx];
            var alertList = player.alertList;

            if (boolList[playerIdx])
            {
                var isCheck = false;
                if (alertList.Exists(checkMonster => checkMonster == curMonster))
                    isCheck = true;

                if (!isCheck)
                {
                    var weapon = player.characterStats.weapon;
                    if (weapon.CheckAvailBullet)
                    {
                        if (weapon.CheckAvailShot(player.AP, CharacterState.Alert))
                        {
                            var isHit = weapon.CheckAlertAccuracy(curMonster.currentTile.accuracy);
                            player.AP -= weapon.GetWeaponAP();

                            if (isHit)
                                curMonster.GetDamage(player.characterStats.weapon.Damage);
                            else
                            {
                                var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
                                window.SetMsgText($"You missed {curMonster.name}");
                            }
                        }
                    }
                    else
                    {
                        if (weapon.CheckReloadAP(player.AP))
                            player.ReloadWeapon();
                    }

                    player.alertList.Add(curMonster);
                }
            }
            else
            {
                if (alertList.Exists(checkMonster => checkMonster == curMonster))
                {
                    var window = BattleMgr.Instance.battleWindowMgr.Open((int)BattleWindows.Msg - 1, false).GetComponent<MsgWindow>();
                    window.SetMsgText($"You missed {curMonster.name}");
                    alertList.Remove(curMonster);
                }
            }
        }
    }

    public void RemovePlayer(PlayerableChar playerableChar)
    {
        var idx = playerableChars.IndexOf(playerableChar);
        playerableChars.RemoveAt(idx);
        Destroy(playerableChar.gameObject);
    }

    private void CalculateVirusAllChar(PlayerableChar character)
    {
        var levelList = new List<int>();
        var monsters = BattleMgr.Instance.monsterMgr.monsters;
        var buffMgr = character.characterStats.buffMgr;
        var reductionList = buffMgr.GetBuffList(Stat.Reduction);
        var reductionBuff = 1f;
        foreach (var reduction in reductionList)
        {
            reductionBuff *= reduction.GetAmount();
        }

        var virusList = buffMgr.GetBuffList(Stat.Virus);
        var virusBuff = 1f;
        foreach (var virus in virusList)
        {
            virusBuff *= virus.GetAmount();
        }

        foreach (var monster in monsters)
        {
            levelList.Add(character.GetVirusLevel(monster));
        }

        var virusLevelDics = new Dictionary<string, int>();
        for (var idx = 0; idx < levelList.Count; ++idx)
        {
            var virusType = monsters[idx].monsterStats.virus.id;
            if (levelList[idx] < 1)
                continue;

            if (!virusLevelDics.ContainsKey(virusType))
                virusLevelDics.Add(virusType, levelList[idx]);
            else if (virusLevelDics[virusType] < levelList[idx])
                virusLevelDics[virusType] = levelList[idx];
        }

        foreach (var pair in virusLevelDics)
        {
            switch (pair.Key)
            {
                case "VIR_0001":
                    character.characterStats.virusPanalty["E"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0002":
                    character.characterStats.virusPanalty["B"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0003":
                    character.characterStats.virusPanalty["P"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0004":
                    character.characterStats.virusPanalty["I"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0005":
                    character.characterStats.virusPanalty["T"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
            }
        }
    }
}