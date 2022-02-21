using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleTurn
{
    None,
    Player,
    Enemy
}

public class BattleMgr : MonoBehaviour
{
    public static BattleMgr Instance;

    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayerMgr playerMgr;
    public BattleMonsterMgr monsterMgr;
    public WindowManager battleWindowMgr;
    public SightMgr sightMgr;
    public PathMgr pathMgr;
    public HintMgr hintMgr;
    public BattlePoolMgr battlePoolMgr;
    public MultiTouch touchMgr;
    public PlayerDataMgr playerDataMgr;     // 여기 저장하기 위해

    [Header("Turn")]
    public BattleTurn startTurn; // For Test
    public BattleTurn turn;
    public int turnCount;
    public int fieldVirusLevel;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    [Header("UI Info Mode")]
    public bool uiInfoMode;
    public void Awake()
    {
        Instance = this;
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        sightMgr = GameObject.FindWithTag("FogMgr").GetComponent<SightMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        battleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
        pathMgr = GameObject.FindWithTag("PathMgr").GetComponent<PathMgr>();
        hintMgr = GameObject.FindWithTag("HintMgr").GetComponent<HintMgr>();
        touchMgr = GameObject.FindWithTag("TouchMgr").GetComponent<MultiTouch>();
        battlePoolMgr = GameObject.FindWithTag("BattlePoolMgr").GetComponent<BattlePoolMgr>();
        playerDataMgr = GameObject.FindWithTag("PlayerDataMgr").GetComponent<PlayerDataMgr>();
    }

    public void Start()
    {
        Time.timeScale = 1f;
        var battleSetting = GetComponent<BattleSetting>();
        battleSetting.Init();
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        var window = battleWindowMgr.Open(0) as BattleBasicWindow;
        window.Init();
        sightMgr.Init();
        pathMgr.Init();
        hintMgr.Init();

        turn = startTurn;
        EventBusMgr.Subscribe(EventType.ChangeTurn, OnChangeTurn);
        EventBusMgr.Subscribe(EventType.DestroyChar, DestroyChar);


        window.SetSelectedChar(playerMgr.playerableChars[0]);
        
        window.StartTurn(turn);
        sightMgr.UpdateFog();
    }

    public void Update()
    {
        switch (turn)
        {
            case BattleTurn.Player:
                if (uiInfoMode)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit hit;
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hit))
                        {
                            var obj = hit.collider.gameObject;
                            monsterMgr.UpdateInfo(obj);
                            hintMgr.UpdateInfo(obj);
                        }
                    }

                    if (touchMgr.Tap)
                    {
                        RaycastHit hit;
                        var ray = Camera.main.ScreenPointToRay(touchMgr.curTouchPos);
                        if (Physics.Raycast(ray, out hit))
                        {
                            var obj = hit.collider.gameObject;
                            monsterMgr.UpdateInfo(obj);
                            hintMgr.UpdateInfo(obj);
                        }
                    }
                }
                break;
            case BattleTurn.Enemy:
                monsterMgr.UpdateTurn();
                break;
        }
    }

    public void OnChangeTurn(object empty)
    {
        Invoke("ChangeTurn", 0.001f);
    }

    private void ChangeTurn()
    {
        var window = battleWindowMgr.Open(0) as BattleBasicWindow;
        switch (turn)
        {
            case BattleTurn.Player:
                turn = BattleTurn.Enemy; 
                window.StartTurn(turn);
                break;
            case BattleTurn.Enemy:
                turn = BattleTurn.Player; 
                window.StartTurn(turn);
                window.SetSelectedChar(playerMgr.playerableChars[0]);
                window.UpdateUI();
                break;
        }
        
        if (startTurn == turn)
            turnCount++;
    }

    public void DestroyChar(object[] param)
    {
        var tempType = (int)param[1];
        var window = battleWindowMgr.GetWindow(0) as BattleBasicWindow;
        window.CheckRemoveUI();
        switch (tempType)
        {
            case 0:
                var player = (PlayerableChar)param[0];
                if (playerDataMgr != null)
                {
                    var currentSquad = playerDataMgr.currentSquad;
                    var boardingSquad = playerDataMgr.boardingSquad;
                    foreach (var pair in currentSquad)
                    {
                        if (pair.Value == player.characterStats)
                        {
                            foreach (var boardPair in boardingSquad)
                            {
                                if (boardPair.Value == pair.Key)
                                {
                                    boardingSquad.Remove(pair.Key);
                                    currentSquad.Remove(pair.Key);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                playerMgr.RemovePlayer(player);
                break;
            case 1:
                var monster = (MonsterChar)param[0];
                monsterMgr.RemoveMonster(monster);
                break;
        }
        CheckGameover();
    }


    private void CheckGameover()
    {
        var playerableChars = playerMgr.playerableChars;
        if (playerableChars.Count == 0 || monsterMgr.monsters.Count == 0)
        {
            turn = BattleTurn.None;

            var exp = ScriptableMgr.Instance.GetMissionExp($"MEP_{fieldVirusLevel}").exp;
            foreach (var player in playerableChars)
            {
                player.characterStats.GetExp(exp);
            }

            battleWindowMgr.Open(1);
        }
    }

    public void ChangeUIMode(bool enabled)
    {
        uiInfoMode = enabled;
        if (uiInfoMode)
        {

        }
        else
        {
            monsterMgr.NonSelectedMonster();
        }

    }
}