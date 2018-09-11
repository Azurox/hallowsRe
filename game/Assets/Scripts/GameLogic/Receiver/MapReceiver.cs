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
    public NpcHandler NpcHandler;
    private readonly SocketManager socket;

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

        NpcHandler.Startup(WorldUIManager);

        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("loadMap", LoadMap);
        socket.On("spawnMainPlayer", SpawnMainPlayer);
        socket.On("spawnPlayer", SpawnPlayer);
        socket.On("disconnectPlayer", DisconnectPlayer);
        socket.On("playerMove", PlayerMove);
    }
    
    private void LoadMap(string json)
    {
        LoadMapResponse data = JsonConvert.DeserializeObject<LoadMapResponse>(json);
        Debug.Log("Load new map");
        string mapName = data.mapName;
        PlayerContainerDTG.gameObject.SetActive(true);
        PlayerContainerDTG.Clear();
        GameMap map = ResourcesLoader.Instance.GetGameMap(mapName);
        WorldMapDTG.SetMap(map);
        WorldMapDTG.ActivateCell();
        MapDTG.Init();
        NpcContainerDTG.gameObject.SetActive(true);
        NpcContainerDTG.LoadNpcs(map.npcs);
        GlobalUIManager.SwitchToWorldUI();
    }
    
    private void SpawnMainPlayer(string json)
    {
        SpawnMainPlayerResponse data = JsonConvert.DeserializeObject<SpawnMainPlayerResponse>(json);
        var player = PlayerContainerDTG.SpawnMainPlayer((int) data.position.x, (int) data.position.y);
        MapHandler.SetMainPlayer(player.GetComponent<MainPlayerHandler>());
        PlayerHandler.SetMainPlayer(player.GetComponent<MainPlayerHandler>());
        WorldUIManager.Init(player.GetComponent<MainPlayerHandler>());
        PlayerInformation.Instance.SetPlayerGameObject(player);
    }

 
    private void SpawnPlayer(string json)
    {

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
}
