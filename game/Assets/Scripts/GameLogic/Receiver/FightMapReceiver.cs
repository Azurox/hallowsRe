using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

public class FightMapReceiver
{

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
    public MonsterGroupContainer MonsterGroupContainer;
    public MainFighterEmitter MainFighterEmitter; // Can be null
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
        MonsterGroupContainer = Object.FindObjectOfType<MonsterGroupContainer>();
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
        socket.On("fightResult", FightResult);
        socket.On("monsterCommand", MonsterCommand);
    }

    private void FightStarted(string json)
    {
        FightStartedResponse data = JsonConvert.DeserializeObject<FightStartedResponse>(json);
        Debug.Log("FightStarted !");
        PlayerContainerDTG.Clear();
        PlayerContainerDTG.gameObject.SetActive(false);
        NpcContainerDTG.gameObject.SetActive(false);
        MonsterGroupContainer.Clear();
        MonsterGroupContainer.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);
        Fight = new Fight(data);

        FighterContainerDTG.Init(Fight.GetFighters());
        FightMapDTG.Init();
        var mainFighterDTG = FighterContainerDTG.GetMainFighter();

        var mainFighterEmitter = mainFighterDTG.GetComponent<MainFighterEmitter>();
        mainFighterEmitter.Init(Fight.Id);
        MainFighterEmitter = mainFighterEmitter;

        FightMapHandler.Init(mainFighterDTG, Fight);
        FighterHandler.SetMainFighter(FighterContainerDTG.GetMainFighter());

        for (var i = 0; i < data.placementCells.Length; i++)
        {
            var cell = data.placementCells[i];
            if(cell.side == "blue")
            {
                FightMapDTG.SetSpawnCell(Side.blue, cell.position, cell.taken);
            }
            else
            {
                FightMapDTG.SetSpawnCell(Side.red, cell.position, cell.taken);
            }

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

        if (FighterContainerDTG.GetMainFighter().GetFighter().Id == data.playerId)
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
            if (fighter.Id == data.playerId)
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
        Debug.Log("Its now " + data.playerId + " turn");
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
        List<Impact> impacts = new List<Impact>();
        for (var i = 0; i < data.impacts.Length; i++)
        {
            Impact impact = new Impact(data.impacts[i]);
            impacts.Add(impact);
        }

        if (data.checkin != null)
        {
            FighterContainerDTG.FighterUseSpell(user, spell, data.position, impacts, () => { if (MainFighterEmitter != null) MainFighterEmitter.Checkin(data.checkin); });
        }
        else
        {
            FighterContainerDTG.FighterUseSpell(user, spell, data.position, impacts, null);
        }
    }

    private void MonsterCommand(string json)
    {
        Debug.Log("receive monster command");
        MonsterCommandResponse data = JsonConvert.DeserializeObject<MonsterCommandResponse>(json);
        ProcessCommand(data, 0);
    }

    private void ProcessCommand(MonsterCommandResponse data, int i)
    {
        if (data.commands != null && i < data.commands.Length && data.commands[i] != null)
        {
            if (data.commands[i].path != null && data.commands[i].path.Length > 0)
            {
                FighterContainerDTG.MoveFighter(data.monsterId, data.commands[0].path.ToList(), ()=> { ProcessCommand(data, i + 1); });
            }else if(data.commands[i].spellId != null)
            {
                Fighter user = Fight.GetFighter(data.monsterId);
                Spell spell = ResourcesLoader.Instance.GetSpell(data.commands[i].spellId);
                List<Impact> impacts = new List<Impact>();
                for (var j = 0; j < data.commands[i].spellImpacts.Length; j++)
                {
                    Impact impact = new Impact(data.commands[i].spellImpacts[j]);
                    impacts.Add(impact);
                }
                FighterContainerDTG.FighterUseSpell(user, spell, data.commands[i].targetPosition, impacts, () => { ProcessCommand(data, i + 1); });
            }
            else
            {
                ProcessCommand(data, i + 1);
            }
        }
        else
        {
            if (data.checkin != null)
            {
                Debug.Log("Checked monster action");
                MainFighterEmitter.Checkin(data.checkin);
            }
        }
    }

    private void FightResult(string json)
    {
        FightEndResponse data = JsonConvert.DeserializeObject<FightEndResponse>(json);
        FighterContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.Clear();
        FightMapDTG.Clear();
        var afterFightStats = new AfterFightStats(data);
        GlobalUIManager.GetWorldUIManager().ShowAfterFightStats(afterFightStats);
        socket.Emit("loadMap");
    }

}
