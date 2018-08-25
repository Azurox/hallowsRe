using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapReceiver {

    public MapDTG MapDTG;
    public FightMapHandler FightMapHandler;
    public FightMapDTG FightMapDTG; 
    public PlayerContainerDTG PlayerContainerDTG;
    public FighterContainerDTG FighterContainerDTG;
    public FighterHandler FighterHandler;
    public GlobalUIManager GlobalUIManager;
    public FightUIManager FightUIManager;
    private SocketIOComponent socket;
    private Fight Fight;

    public FightMapReceiver(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        FightMapDTG = MapDTG.GetComponent<FightMapDTG>();
        FightMapHandler = MapDTG.GetComponent<FightMapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
        FighterContainerDTG = Object.FindObjectOfType<FighterContainerDTG>();
        FighterHandler = FighterContainerDTG.GetComponent<FighterHandler>();
        GlobalUIManager = Object.FindObjectOfType<GlobalUIManager>();
        FightUIManager = GlobalUIManager.GetFightUIManager();

        FighterContainerDTG.gameObject.SetActive(false);
        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("fightStarted", FightStarted);
        socket.On("teleportPreFight", TeleportPreFight);
        socket.On("setReady", SetReady);
        socket.On("fightPhase1", FightPhase1);
        socket.On("nextTurn", NextTurn);
    }

    private void FightStarted(SocketIOEvent obj)
    {
        Debug.Log("FightStarted !");
        PlayerContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);
        Fight = new Fight(socket, obj);

        FighterContainerDTG.Init(Fight.GetFighters());
        FightMapDTG.Init();
        var mainFighterDTG = FighterContainerDTG.GetMainFighter();
        var mainFighterEmitter = mainFighterDTG.GetComponent<MainFighterEmitter>();
        mainFighterEmitter.Init(Fight.Id);

        FightMapHandler.Init(mainFighterDTG, FightMapDTG, Fight);
        FighterHandler.SetMainFighter(FighterContainerDTG.GetMainFighter());

        var blueCells = obj.data["blueCells"];
        for (var i = 0; i < blueCells.Count; i++)
        {
            Vector2 position = new Vector2(blueCells[i]["position"]["x"].n, blueCells[i]["position"]["y"].n);
            bool taken = blueCells[i]["taken"].b;
            FightMapDTG.SetSpawnCell(Side.blue, position, taken);
        }

        var redCells = obj.data["redCells"];
        for (var i = 0; i < blueCells.Count; i++)
        {
            Vector2 position = new Vector2(redCells[i]["position"]["x"].n, redCells[i]["position"]["y"].n);
            bool taken = redCells[i]["taken"].b;
            FightMapDTG.SetSpawnCell(Side.red, position, taken);

        }

        GlobalUIManager.SwitchToFightUI();
        FightUIManager.Init(mainFighterEmitter);
        FightUIManager.SetUIPhase0();
        FightUIManager.UpdateFightTimeline(Fight.GetFighters());
        FightUIManager.ShowFighterStats(Fight.GetMainFighter());
    }

    private void TeleportPreFight(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Vector2 position = new Vector2(obj.data["position"]["x"].n, obj.data["position"]["y"].n);

        foreach (var fighter in Fight.GetFighters())
        {
            if (fighter.Id == id)
            {
                Vector2 oldPosition = fighter.Position;
                FightMapDTG.SetCellAvailability(position, true);
                FightMapDTG.SetCellAvailability(oldPosition, false);
            }
        }

        if(FighterContainerDTG.GetMainFighter().GetFighter().Id == id)
        {
            FighterContainerDTG.TeleportMainFighter(position);
        }
        else
        {
            FighterContainerDTG.TeleportFighter(id, position);
        }
    }

    private void SetReady(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Debug.Log(id + " is ready");
        foreach (var fighter in Fight.GetFighters())
        {
            if(fighter.Id == id)
            {
                fighter.Ready = true;
                if (fighter.IsMainPlayer) FightUIManager.ActivateReadyButton(false);
            }
        }
    }

    private void FightPhase1(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Debug.Log("is " + id + " turn ");
        FightMapDTG.ResetDirtyCells();
        FightUIManager.SetUIPhase1();
        FightUIManager.HighlightFighter(id);
    }

    private void NextTurn(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Debug.Log("is " + id + " turn ");
        FightUIManager.HighlightFighter(id);
    }

    private void FightFinished()
    {
        PlayerContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);
    }
}
