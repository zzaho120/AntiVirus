using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VirusZone1Info : MonoBehaviour
{
    public InfectedCharTest squadUI;
    public Vector3 laboratoryPos;
    public float virusZone2Radius;
    public string virusType;

    GameObject player;
    PlayerController playerController;
    PlayerDataMgr playerDataMgr;

    float timer;
    float decreaseTimer; 
    float decreaseTime;
    bool isGameOver;

    private void Start()
    {
        //Debug.Log("Script : VirusZone1Info");
        var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
        playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();

        timer = 0f;
        decreaseTimer = 0f;
        decreaseTime = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerDataMgr == null)
            {
                var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
                playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
            }

            //Debug.Log("플레이어가 들어왔습니다.");
            //Debug.Log($"바이러스 종류 : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();

            squadUI.TurnOnWarning(0);
            squadUI.TurnOnWarning(1);
            squadUI.TurnOnWarning(2);
            squadUI.TurnOnWarning(3);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            if (playerDataMgr == null)
            {
                var playerDataMgrObj = GameObject.FindGameObjectWithTag("PlayerDataMgr");
                playerDataMgr = playerDataMgrObj.GetComponent<PlayerDataMgr>();
            }

            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0f;
                decreaseTimer++;
            }

            var distance = Vector2.Distance(new Vector2(laboratoryPos.x, laboratoryPos.z), new Vector2(player.transform.position.x, player.transform.position.z));
            if (distance > virusZone2Radius)
            {
                foreach (var element in playerDataMgr.currentSquad.Keys.ToList())
                {
                    //항체를 가지고 있지 않다면.
                    var characterName = playerDataMgr.currentSquad[element].character.name;
                    if (!playerDataMgr.characterInfos[characterName].antivirus.Contains($"{virusType}1"))
                    {

                        //squadUI.TurnOnWarning(element);
                    }
                }
            }
        }
            //timer += Time.deltaTime;
            //if (timer > 1)
            //{
            //    timer = 0f;
            //    decreaseTimer++;
            //}
            //if (decreaseTimer >= decreaseTime)
            //{
            //    decreaseTimer = 0f;

            //    foreach (var key in playerDataMgr.currentSquad.Keys.ToList())
            //    {
            //        CharacterDetail info;
            //        playerDataMgr.characterInfos.TryGetValue(playerDataMgr.currentSquad[key], out info);
            //        if (info.stamina > 1)
            //        {
            //            info.stamina -= 1;

            //            playerDataMgr.saveData.staminas[info.id] = info.stamina;
            //            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //            Debug.Log($"{info.name} stamina : {info.stamina}");
            //        }
            //        else if (info.hp > 1)
            //        {
            //            info.hp -= 1;

            //            playerDataMgr.saveData.hps[info.id] = info.hp;
            //            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //            Debug.Log($"{info.name} hp : {info.hp}");
            //        }
            //        else
            //        {
            //            Debug.Log($"{info.name} die");
            //            //isGameOver = true;

            //            string str = $"Squad{key}";
            //            PlayerPrefs.SetString(str, null);

            //            playerDataMgr.characterInfos.Remove(playerDataMgr.currentSquad[key]);
            //            playerDataMgr.currentSquad.Remove(key);
                        
            //            playerDataMgr.saveData.ids.RemoveAt(info.id);
            //            playerDataMgr.saveData.names.RemoveAt(info.id);
            //            playerDataMgr.saveData.hps.RemoveAt(info.id);
            //            playerDataMgr.saveData.offensePowers.RemoveAt(info.id);
            //            playerDataMgr.saveData.willPowers.RemoveAt(info.id);
            //            playerDataMgr.saveData.staminas.RemoveAt(info.id);

            //            squadUI.RemoveSquad(key);
            //            squadUI.TurnOffWarning();

            //            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            //        }
            //    }
            //}
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("플레이어가 나갔습니다.");
            player = null;
            playerController = null;

            squadUI.TurnOffWarning();

            timer = 0f;
            decreaseTimer = 0f;
        }
    }
}
