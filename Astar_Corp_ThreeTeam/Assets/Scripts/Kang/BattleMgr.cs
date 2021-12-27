using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMgr : Singleton<BattleMgr>
{
    public CommandMgr commandMgr;
    public TileMgr tileMgr;
    public BattlePlayerMgr playerMgr;
    public BattleMonsterMgr monsterMgr;
    public WindowManager BattleWindowMgr;
    public FogMgr fogMgr;
    public AStar aStar;
    public int turn;

    public override void Awake()
    {
        base.Awake();
        commandMgr = new CommandMgr();

        tileMgr = GameObject.FindWithTag("TileMgr").GetComponent<TileMgr>();
        fogMgr = GameObject.FindWithTag("FogMgr").GetComponent<FogMgr>();
        playerMgr = GameObject.FindWithTag("Player").GetComponent<BattlePlayerMgr>();
        monsterMgr = GameObject.FindWithTag("BattleMonster").GetComponent<BattleMonsterMgr>();
        BattleWindowMgr = GameObject.FindWithTag("BattleWindow").GetComponent<WindowManager>();
    }

    public void Start()
    {
        tileMgr.Init();
        playerMgr.Init();
        monsterMgr.Init();
        aStar.Init();
        fogMgr.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OnTurnEnd();
    }
    public void OnTurnEnd()
    {
        if (turn < 5)
        {
            EventBusMgr.Publish(EventType.TurnEnd);
            turn++;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
