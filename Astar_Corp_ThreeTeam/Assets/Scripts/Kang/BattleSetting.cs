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

public enum CharacterClass
{
    Tanker,
    Healer,
    Scout,
    Bombardier,
    Sniper
}

public enum WeaponName
{
    Revolver_01,
    Nailgun_01,
    Pistol_01,
    Bat_Wood_01,
    Pipe_01,
    Crowbar_01,
    FireAxe_01,
    Hybrid_03,
    Shotgun_01,
    SubMGun_01,
    SubMGun_02,
    SubMGun_03,
    AssaultRifle_01,
    AssaultRifle_02,
    AssaultRifle_03,
    Rifle_01,
    MachineGun_01,
    MachineGun_02,
    Minigun_01,
    RocketLauncher_01,
    HuntingRifle_01,
    HuntingRifle_Clean_01,
    Rifle_03,
    Hand,

}
public class BattleSetting : MonoBehaviour
{
    public List<Vector2> playerPos;
    public List<CharacterClass> characterClasses;
    public List<WeaponName> mainWeaponName;
    public List<WeaponName> subWeaponName;
    public List<Vector2> monsterPos;
    public List<MonsterType> monsters;
    public List<VirusType> monsterVirus;
    public List<int> monsterVirusLevel;
    public List<GameObject> monsterPrefabs;
    public List<GameObject> weaponPrefabs;
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

                var weapon = playerableChar.characterStats.weapon;
                playerableChar.mainWeapon = weaponPrefabs[weapon.mainWeapon.battleID];
                if (playerableChar.subWeapon != null)
                    playerableChar.subWeapon = weaponPrefabs[weapon.subWeapon.battleID];
                var weaponGo = Instantiate(playerableChar.mainWeapon, playerableChar.rightHandTr);
                playerableChar.currentWeapon = weaponGo;
                weaponGo.transform.localRotation = Quaternion.Euler(playerableChar.weaponRot);
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
                BattleMgr.Instance.startTurn = BattleTurn.Player;
                var playerableChar = player.GetComponent<PlayerableChar>();
                playerableChar.characterStats = new CharacterStats();
                switch (characterClasses[idx])
                {
                    case CharacterClass.Tanker:
                        playerableChar.characterStats.character = ScriptableMgr.Instance.GetCharacter("CHAR_0001");
                        break;
                    case CharacterClass.Healer:
                        playerableChar.characterStats.character = ScriptableMgr.Instance.GetCharacter("CHAR_0002");
                        break;
                    case CharacterClass.Scout:
                        playerableChar.characterStats.character = ScriptableMgr.Instance.GetCharacter("CHAR_0005");
                        break;
                    case CharacterClass.Bombardier:
                        playerableChar.characterStats.character = ScriptableMgr.Instance.GetCharacter("CHAR_0004");
                        break;
                    case CharacterClass.Sniper:
                        playerableChar.characterStats.character = ScriptableMgr.Instance.GetCharacter("CHAR_0003");
                        break;
                }

                var zeroStr = string.Empty;
                if ((int)mainWeaponName[idx] > 8)
                    zeroStr = "00";
                else
                    zeroStr = "000";


                playerableChar.characterStats.weapon = new WeaponStats();
                playerableChar.characterStats.weapon.mainWeapon = ScriptableMgr.Instance.GetEquippable($"WEP_{zeroStr}{(int)mainWeaponName[idx] + 1}");

                if ((int)subWeaponName[idx] > 8)
                    zeroStr = "00";
                else
                    zeroStr = "000";
                playerableChar.characterStats.weapon.subWeapon = ScriptableMgr.Instance.GetEquippable($"WEP_{zeroStr}{(int)subWeaponName[idx] + 1}");

                playerableChar.mainWeapon = weaponPrefabs[(int)mainWeaponName[idx]];
                playerableChar.subWeapon = weaponPrefabs[(int)subWeaponName[idx]];
                var weaponGo = Instantiate(playerableChar.mainWeapon, playerableChar.rightHandTr);
                playerableChar.currentWeapon = weaponGo;

                weaponGo.transform.localRotation = Quaternion.Euler(playerableChar.weaponRot);
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
