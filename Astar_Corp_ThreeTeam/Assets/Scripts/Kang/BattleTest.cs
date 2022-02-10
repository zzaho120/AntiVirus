using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Bear,
    Boar,
    Fox,
    Jaguar,
    Spider,
    Tiger,
    Wolf
}

public class BattleTest : MonoBehaviour
{
    public List<Vector2> playerPos;
    public List<Vector2> monsterPos;
    public List<MonsterType> monsters;
    public List<VirusType> monsterVirus;
    public List<int> monsterVirusLevel;
    public List<GameObject> monsterPrefabs;
    public GameObject player;
    public void Init()
    {
        var playerDataMgrObj = GameObject.FindWithTag("PlayerDataMgr");
        var isExistDataMgr = playerDataMgrObj != null;
        PlayerDataMgr playerDataMgr = null;
        if (isExistDataMgr)
        {
            var playerPrefab = BattleMgr.Instance.playerPrefab;
            playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

            for (var idx = 0; idx < playerDataMgr.battleSquad.Count; ++idx)
            {
                var player = Instantiate(playerPrefab, new Vector3(playerPos[idx].x, .5f, playerPos[idx].y), Quaternion.Euler(new Vector3(0, 180, 0)));
                player.transform.SetParent(BattleMgr.Instance.playerMgr.transform);
                var playerableChar = player.GetComponent<PlayerableChar>();
                playerableChar.characterStats = playerDataMgr.battleSquad[idx];
            }

            if (playerDataMgr.isMonsterAtk)
                BattleMgr.Instance.startTurn = BattleTurn.Enemy;
            else
                BattleMgr.Instance.startTurn = BattleTurn.Player;
        }
        else
        {
            for (var idx = 0; idx < playerPos.Count; ++idx)
            {
                var playerPrefab = BattleMgr.Instance.playerPrefab;
                var player = Instantiate(playerPrefab, new Vector3(playerPos[idx].x, .5f, playerPos[idx].y), Quaternion.Euler(new Vector3(0, 180, 0)));
                player.transform.SetParent(BattleMgr.Instance.playerMgr.transform);
                player.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                this.player = player;
            }
        }

        for (var idx = 0; idx < monsterPos.Count; ++idx)
        {
            var monsterPrefab = monsterPrefabs[(int)monsters[idx]];
            var monster = Instantiate(monsterPrefab, new Vector3(monsterPos[idx].x, .5f, monsterPos[idx].y), Quaternion.Euler(new Vector3(0, 180, 0)));
            monster.transform.SetParent(BattleMgr.Instance.monsterMgr.transform);

            var monsterChar = monster.GetComponentInChildren<MonsterChar>();

            string virus = null;
            switch (monsterVirus[idx])
            {
                case VirusType.None:
                    break;
                case VirusType.E:
                    virus = "E";
                    break;
                case VirusType.B:
                    virus = "B";
                    break;
                case VirusType.P:
                    virus = "P";
                    break;
                case VirusType.I:
                    virus = "I";
                    break;
                case VirusType.T:
                    virus = "T";
                    break;
            }
            monsterChar.monsterStats = monster.GetComponentInChildren<MonsterStats>();
            monsterChar.monsterStats.virus = ScriptableMgr.Instance.GetVirus(virus);
            monsterChar.monsterStats.virusLevel = monsterVirusLevel[idx];
        }
    }
}
