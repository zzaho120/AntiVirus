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
        var monsters = BattleMgr.Instance.monsterMgr.monsters;
        foreach (var character in playerableChars)
        {
            character.StartTurn();

            var levelList = new List<int>();
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
                    case "1":
                        character.characterStats.virusPanalty["E"].Calculation(pair.Value);
                        break;
                    case "2":
                        character.characterStats.virusPanalty["B"].Calculation(pair.Value);
                        break;
                    case "3":
                        character.characterStats.virusPanalty["P"].Calculation(pair.Value);
                        break;
                    case "4":
                        character.characterStats.virusPanalty["I"].Calculation(pair.Value);
                        break;
                    case "5":
                        character.characterStats.virusPanalty["T"].Calculation(pair.Value);
                        break;
                }
            }
        }
    }

    public void CheckEndTurn(object empty)
    {
        var turnEndCount = 0; 
        foreach (var player in playerableChars)
        {
            if (player.status == PlayerState.TurnEnd || player.status == PlayerState.Alert)
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
                        if (weapon.CheckAvailShot(player.AP, PlayerState.Alert))
                        {
                            var isHit = weapon.CheckAlertAccuracy(curMonster.currentTile.accuracy);
                            player.AP -= weapon.GetWeaponAP(PlayerState.Attack);

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
}