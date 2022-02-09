using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerMgr : MonoBehaviour
{
    public List<PlayerableChar> playerableChars;
    public PlayerableChar selectChar;
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
        for (var idx = 0; idx < playerableChars.Count; idx++)
        {
            playerableChars[idx].StartTurn();
            CalculateVirusAllChar(playerableChars[idx]);
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
                            var isHit = false;
                            if (player.isTankB1Skill)
                            {
                                isHit = true;
                                player.isTankB1Skill = false;
                            }
                            else
                                isHit = Random.Range(0, 100) < weapon.GetAttackAccuracy(curMonster.currentTile.accuracy) + player.characterStats.alertAccuracy;

                            player.AP -= weapon.GetWeaponAP();

                            if (isHit)
                            {
                                var isCrit = Random.Range(0, 100) < player.characterStats.weapon.CritRate + player.characterStats.critRate - curMonster.monsterStats.critResist;
                                curMonster.GetDamage(player, isCrit);
                            }
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
        playerableChar.currentTile.charObj = null;
        var idx = playerableChars.IndexOf(playerableChar);
        playerableChars.RemoveAt(idx);
        Destroy(playerableChar.gameObject);
        BattleMgr.Instance.sightMgr.sightList.RemoveAt(idx);
        BattleMgr.Instance.sightMgr.frontSightList.RemoveAt(idx);
    }

    private void CalculateVirusAllChar(PlayerableChar character)
    {
        CalCulateVirusByMonster(character);
        CalculateVirusPenaly(character);
    }
    private void CalCulateVirusByMonster(PlayerableChar character)
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
                    character.characterStats.virusPenalty["E"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0002":
                    character.characterStats.virusPenalty["B"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0003":
                    character.characterStats.virusPenalty["P"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0004":
                    character.characterStats.virusPenalty["I"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
                case "VIR_0005":
                    character.characterStats.virusPenalty["T"].Calculation(pair.Value, virusBuff, reductionBuff);
                    break;
            }
        }
    }

    private void CalculateVirusPenaly(PlayerableChar character)
    {
        var virusPenaltyDics = character.characterStats.virusPenalty;
        var level = 0;
        foreach (var pair in virusPenaltyDics)
        {
            var virusPenalty = pair.Value;
            level += virusPenalty.penaltyLevel;
            if (virusPenalty.penaltyLevel > 0)
            {
                var virus = virusPenalty.virus;
                var buffList = new List<BuffBase>();
                switch (virus.penaltyType)
                {
                    case VirusPenalyType.DmgDec:
                        buffList.Add(new BuffBase(Stat.Damage, virus.stat_Dec, 1, false));
                        break;
                    case VirusPenalyType.HpDec:
                        buffList.Add(new BuffBase(Stat.Hp, virus.stat_Dec, 1, false));
                        break;
                    case VirusPenalyType.MpDec:
                        buffList.Add(new BuffBase(Stat.Mp, virus.stat_Dec, 1, false));
                        break;
                    case VirusPenalyType.AccuracyDec:
                        buffList.Add(new BuffBase(Stat.Accuracy, virus.stat_Dec, 1, false));
                        break;
                    case VirusPenalyType.All:
                        buffList.Add(new BuffBase(Stat.Damage, virus.stat_Dec, 1, false));
                        buffList.Add(new BuffBase(Stat.Hp, virus.stat_Dec, 1, false));
                        buffList.Add(new BuffBase(Stat.Mp, virus.stat_Dec, 1, false));
                        buffList.Add(new BuffBase(Stat.Accuracy, virus.stat_Dec, 1, false));
                        break;
                }

                foreach (var buff in buffList)
                {
                    character.characterStats.buffMgr.Addbuff(buff);
                }
            }
        }

        character.GetDamage(level);
    }

}