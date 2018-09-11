using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

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
    public NpcContainerDTG NpcContainerDTG;
    private SocketManager socket;
    private Fight Fight;

    public FightMapReceiver(SocketManager socket)
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
        NpcContainerDTG = Object.FindObjectOfType<NpcContainerDTG>();
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
    
    private void FightStarted(string json)
    {
        FightStartedResponse data = JsonConvert.DeserializeObject<FightStartedResponse>(json);
        Debug.Log("FightStarted !");
        PlayerContainerDTG.gameObject.SetActive(false);
        NpcContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);
        Fight = new Fight(data);

        FighterContainerDTG.Init(Fight.GetFighters());
        FightMapDTG.Init();
        var mainFighterDTG = FighterContainerDTG.GetMainFighter();

        var mainFighterEmitter = mainFighterDTG.GetComponent<MainFighterEmitter>();
        mainFighterEmitter.Init(Fight.Id);

        FightMapHandler.Init(mainFighterDTG, Fight);
        FighterHandler.SetMainFighter(FighterContainerDTG.GetMainFighter());

        for (var i = 0; i < data.blueCells.Length; i++)
        {
            FightMapDTG.SetSpawnCell(Side.blue, data.blueCells[i].position, data.blueCells[i].taken);
        }


        for (var i = 0; i < data.redCells.Length; i++)
        {
            FightMapDTG.SetSpawnCell(Side.red, data.redCells[i].position, data.redCells[i].taken);

        }

        PlayerInformation.Instance.SetFighterGameObject(mainFighterDTG.gameObject);
        GlobalUIManager.SwitchToFightUI();
        FightUIManager.Init(mainFighterEmitter, FightMapHandler);
        FightUIManager.SetUIPhase0();
        FightUIManager.UpdateFightTimeline(Fight.GetFighters());
        FightUIManager.ShowFighterStats(Fight.GetMainFighter());
        FightUIManager.ShowSpells(Fight.GetMainFighter().GetSpells());
    }

    
    private void TeleportPreFight(string json)
    {

        TeleportPreFightResponse data = JsonConvert.DeserializeObject<TeleportPreFightResponse>(json);

        foreach (var fighter in Fight.GetFighters())
        {
            if (fighter.Id == data.playerId)
            {
                Vector2 oldPosition = fighter.Position;
                FightMapDTG.SetCellAvailability(data.position, true);
                FightMapDTG.SetCellAvailability(oldPosition, false);
            }
        }

        if(FighterContainerDTG.GetMainFighter().GetFighter().Id == data.playerId)
        {
            FighterContainerDTG.TeleportMainFighter(data.position);
        }
        else
        {
            FighterContainerDTG.TeleportFighter(data.playerId, data.position);
        }
    }

    
    private void SetReady(string json)
    {
        PlayerIdResponse data = JsonConvert.DeserializeObject<PlayerIdResponse>(json);

        foreach (var fighter in Fight.GetFighters())
        {
            if(fighter.Id == data.playerId)
            {
                fighter.Ready = true;
                if (fighter.IsMainPlayer) FightUIManager.ActivateReadyButton(false);
            }
        }
    }

    private void FightPhase1(string json)
    {
        PlayerIdResponse data = JsonConvert.DeserializeObject<PlayerIdResponse>(json);

        Fight.phase = 1;
        Fight.SetTurnId(data.playerId);
        FightMapDTG.ResetSpawnCells();
        FightUIManager.SetUIPhase1();
        FightUIManager.HighlightFighter(data.playerId);
    }

    private void NextTurn(string json)
    {
        PlayerIdResponse data = JsonConvert.DeserializeObject<PlayerIdResponse>(json);

        Fight.SetTurnId(data.playerId);
        FightUIManager.HighlightFighter(data.playerId);
    }

    private void FighterMove(string json)
    {
        FighterMoveResponse data = JsonConvert.DeserializeObject<FighterMoveResponse>(json);
        
        List<Vector2> path = data.path.ToList();

        if (FighterContainerDTG.GetMainFighter().GetFighter().Id == data.playerId)
        {
            FighterContainerDTG.MoveMainFighter(path);
        }
        else
        {
            FighterContainerDTG.MoveFighter(data.playerId, path);
        }
    }

    private void FighterUseSpell(string json)
    {
        FighterUseSpellResponse data = JsonConvert.DeserializeObject<FighterUseSpellResponse>(json);

        Fighter user = Fight.GetFighter(data.playerId);
        Spell spell = ResourcesLoader.Instance.GetSpell(data.spellId);
        List<Impact> impacts =  new List<Impact>();
        for (var i = 0; i < data.impacts.Length; i++)
        {
            Impact impact = new Impact(data.impacts[i]);
            impacts.Add(impact);
        }

        if(data.fightEnd != null)
        {
            FighterContainerDTG.FighterUseSpell(user, spell, data.position, impacts, ()=> { FightFinished(data.fightEnd); });
        }
        else
        {
            FighterContainerDTG.FighterUseSpell(user, spell, data.position, impacts, null);
        }
    }

    private void FightFinished(string json)
    {
        FightEndResponse data = JsonConvert.DeserializeObject<FightEndResponse>(json);
        FightFinished(data);
    }

    private void FightFinished(FightEndResponse data)
    {
        FighterContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.Clear();
        FightMapDTG.Clear();
        var afterFightStats = new AfterFightStats(data);
        GlobalUIManager.GetWorldUIManager().ShowAfterFightStats(afterFightStats);
        socket.Emit("loadMap");
    }
}
