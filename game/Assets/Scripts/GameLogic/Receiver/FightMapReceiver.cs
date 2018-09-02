using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapReceiver {

    public GlobalMapDTG WorldMapDTG;
    public MapDTG MapDTG;
    public FightMapHandler FightMapHandler;
    public FightMapDTG FightMapDTG; 
    public PlayerContainerDTG PlayerContainerDTG;
    public FighterContainerDTG FighterContainerDTG;
    public FighterHandler FighterHandler;
    public MainFighterHandler MainFighterHandler;
    public GlobalUIManager GlobalUIManager;
    public FightUIManager FightUIManager;
    private SocketIOComponent socket;
    private Fight Fight;

    public FightMapReceiver(SocketIOComponent socket)
    {
        this.socket = socket;
        WorldMapDTG = Object.FindObjectOfType<GlobalMapDTG>();
        MapDTG = WorldMapDTG.GetComponent<MapDTG>();
        FightMapDTG = MapDTG.GetComponent<FightMapDTG>();
        FightMapHandler = MapDTG.GetComponent<FightMapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
        FighterContainerDTG = Object.FindObjectOfType<FighterContainerDTG>();
        FighterHandler = FighterContainerDTG.GetComponent<FighterHandler>();
        MainFighterHandler = FighterContainerDTG.GetComponent<MainFighterHandler>();
        GlobalUIManager = Object.FindObjectOfType<GlobalUIManager>();
        FightUIManager = GlobalUIManager.GetFightUIManager();


        /* Main Startups dependencies */
        MainFighterHandler.Startup(FightMapHandler);
        FighterHandler.Startup(FightMapHandler);
        FighterContainerDTG.Startup(FightUIManager, FightMapDTG);
        FightMapHandler.Startup(FighterContainerDTG);

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
        socket.On("fighterMove", FighterMove);
        socket.On("fighterUseSpell", FighterUseSpell);
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

        FightMapHandler.Init(mainFighterDTG, Fight);
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
        FightUIManager.Init(mainFighterEmitter, FightMapHandler);
        FightUIManager.SetUIPhase0();
        FightUIManager.UpdateFightTimeline(Fight.GetFighters());
        FightUIManager.ShowFighterStats(Fight.GetMainFighter());
        FightUIManager.ShowSpells(Fight.GetMainFighter().GetSpells());
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
        Fight.phase = 1;
        string id = obj.data["playerId"].str;
        Fight.SetTurnId(id);
        FightMapDTG.ResetSpawnCells();
        FightUIManager.SetUIPhase1();
        FightUIManager.HighlightFighter(id);
    }

    private void NextTurn(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Fight.SetTurnId(id);
        FightUIManager.HighlightFighter(id);
    }

    private void FighterMove(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;

        List<Vector2> path = new List<Vector2>();
        List<JSONObject> positions = obj.data["path"].list;
        foreach (var position in positions)
        {
            path.Add(new Vector2(position["x"].n, position["y"].n));
        }

        if (FighterContainerDTG.GetMainFighter().GetFighter().Id == id)
        {
            FighterContainerDTG.MoveMainFighter(path);
        }
        else
        {
            FighterContainerDTG.MoveFighter(id, path);
        }
    }

    private void FighterUseSpell(SocketIOEvent obj)
    {
        string id = obj.data["playerId"].str;
        Fighter user = Fight.GetFighter(id);
        Vector2 position = new Vector2(obj.data["position"]["x"].n, obj.data["position"]["y"].n);
        string spellId = obj.data["spellId"].str;
        Spell spell = ResourcesLoader.Instance.GetSpell(spellId);
        var rawImpacts = obj.data["impacts"];
        List<Impact> impacts =  new List<Impact>();
        for (var i = 0; i < rawImpacts.Count; i++)
        {
            Impact impact = new Impact(rawImpacts[i]);
            impacts.Add(impact);
        }

        var fightEnd = obj.data["fightEnd"];

        if(fightEnd != null)
        {
            FighterContainerDTG.FighterUseSpell(user, spell, position, impacts, ()=> { FightFinished(fightEnd); });
        }
        else
        {
            FighterContainerDTG.FighterUseSpell(user, spell, position, impacts, null);
        }
    }

    private void FightFinished(SocketIOEvent obj)
    {
        FightFinished(obj.data);
    }

    private void FightFinished(JSONObject data)
    {
        FighterContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.Clear();
        FightMapDTG.Clear();
        var afterFightStats = new AfterFightStats(data);
        GlobalUIManager.GetWorldUIManager().ShowAfterFightStats(afterFightStats);
        socket.Emit("loadMap");
    }
}
