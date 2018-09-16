using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebSocketSharp;

public class MapReceiver {

    public GlobalMapDTG WorldMapDTG;
    public MapDTG MapDTG;
    public MapHandler MapHandler;
    public PlayerContainerDTG PlayerContainerDTG;
    public PlayerHandler PlayerHandler;
    public GlobalUIManager GlobalUIManager;
    public WorldUIManager WorldUIManager;
    public NpcContainerDTG NpcContainerDTG;
    public MonsterGroupContainer MonsterGroupContainer;
    public NpcHandler NpcHandler;
    private readonly SocketManager socket;
    private bool needToReload = false;

    public MapReceiver(SocketManager socket)
    {
        this.socket = socket;
        WorldMapDTG = Object.FindObjectOfType<GlobalMapDTG>();
        MapDTG = WorldMapDTG.GetComponent<MapDTG>();
        MapHandler = MapDTG.GetComponent<MapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
        PlayerHandler = PlayerContainerDTG.GetComponent<PlayerHandler>();
        GlobalUIManager = Object.FindObjectOfType<GlobalUIManager>();
        WorldUIManager = GlobalUIManager.GetWorldUIManager();
        NpcContainerDTG = Object.FindObjectOfType<NpcContainerDTG>();
        NpcHandler = NpcContainerDTG.GetComponent<NpcHandler>();
        MonsterGroupContainer = Object.FindObjectOfType<MonsterGroupContainer>();

        NpcHandler.Startup(WorldUIManager);
        MonsterGroupContainer.Startup(MapDTG.GetComponent<MapPathFinding>());

        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("loadMap", LoadMap);
        socket.On("spawnMainPlayer", SpawnMainPlayer);
        socket.On("spawnPlayer", SpawnPlayer);
        socket.On("disconnectPlayer", DisconnectPlayer);
        socket.On("playerMove", PlayerMove);
        socket.On("spawnMonsterGroup", SpawnMonsterGroup);
        socket.On("removeMonsterGroup", RemoveMonsterGroup);
        socket.On("moveMonsterGroup", MoveMonsterGroup);
        socket.On("removePlayer", RemovePlayer);
        socket.On("fightStarted", (json) => needToReload = true);
    }


    private void ClearMap()
    {
        if (needToReload)
        {
            NpcContainerDTG.Clear();
            PlayerContainerDTG.Clear();
            MonsterGroupContainer.Clear();
            needToReload = false;
        }
    }

    private void LoadMap(string json)
    {
        ClearMap();
        LoadMapResponse data = JsonConvert.DeserializeObject<LoadMapResponse>(json);
        Debug.Log("Load new map");
        string mapName = data.mapName;
        PlayerContainerDTG.gameObject.SetActive(true);
        GameMap map = ResourcesLoader.Instance.GetGameMap(mapName);
        WorldMapDTG.SetMap(map);
        WorldMapDTG.ActivateCell();
        MapDTG.Init();
        NpcContainerDTG.gameObject.SetActive(true);
        NpcContainerDTG.LoadNpcs(map.npcs);
        MonsterGroupContainer.gameObject.SetActive(true);
        GlobalUIManager.SwitchToWorldUI();
    }
    
    private void SpawnMainPlayer(string json)
    {
        ClearMap();
        SpawnMainPlayerResponse data = JsonConvert.DeserializeObject<SpawnMainPlayerResponse>(json);
        var player = PlayerContainerDTG.SpawnMainPlayer((int) data.position.x, (int) data.position.y);
        MapHandler.SetMainPlayer(player.GetComponent<MainPlayerHandler>());
        PlayerHandler.SetMainPlayer(player.GetComponent<MainPlayerHandler>());
        WorldUIManager.Init(player.GetComponent<MainPlayerHandler>());
        MonsterGroupContainer.Init(player.GetComponent<MainPlayerHandler>());
        PlayerInformation.Instance.SetPlayerGameObject(player);
    }

 
    private void SpawnPlayer(string json)
    {
        ClearMap();
        SpawnPlayerResponse data = JsonConvert.DeserializeObject<SpawnPlayerResponse>(json);
        PlayerContainerDTG.SpawnPlayer((int)data.position.x, (int)data.position.y, data.id);
        if (data.path != null)
        {
            Debug.Log("player is currently moving");
            PlayerContainerDTG.MovePlayer(data.id, data.path);
        }
    }
    
    private void DisconnectPlayer(string json)
    {
        DisconnectPlayerResponse data = JsonConvert.DeserializeObject<DisconnectPlayerResponse>(json);
        PlayerContainerDTG.DestroyPlayer(data.id);
    }

    
    private void PlayerMove(string json)
    {
        PlayerMoveResponse data = JsonConvert.DeserializeObject<PlayerMoveResponse>(json);
        PlayerContainerDTG.MovePlayer(data.id, data.path);
    }

    private void SpawnMonsterGroup(string json)
    {
        ClearMap();
        SpawnMonsterGroupResponse data = JsonConvert.DeserializeObject<SpawnMonsterGroupResponse>(json);
        foreach (MonsterGroupResponse monsterGroupResponse in data.monsterGroups)
        {
            MonsterGroupContainer.LoadMonsterGroup(monsterGroupResponse);
        }
    }

    private void RemoveMonsterGroup(string json)
    {
        RemoveMonsterGroupResponse data = JsonConvert.DeserializeObject<RemoveMonsterGroupResponse>(json);
        MonsterGroupContainer.RemoveGroup(data.id);
    }

    private void MoveMonsterGroup(string json)
    {
        MoveMonsterGroupResponse data = JsonConvert.DeserializeObject<MoveMonsterGroupResponse>(json);
        MonsterGroupContainer.MoveMonsterGroup(data.id, data.position);
    }

    private void RemovePlayer(string json)
    {
        DisconnectPlayerResponse data = JsonConvert.DeserializeObject<DisconnectPlayerResponse>(json);
        PlayerContainerDTG.DestroyPlayer(data.id);
    }
}
