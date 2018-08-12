using Newtonsoft.Json;
using SocketIO;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldMapController {

    public MapDTG MapDTG;
    public PlayerContainerDTG PlayerContainerDTG;
    private SocketIOComponent socket;

    public WorldMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("loadMap", LoadMap);
        socket.On("spawnMainPlayer", SpawnMainPlayer);
        socket.On("spawnPlayer", SpawnPlayer);
        socket.On("disconnectPlayer", DisconnectPlayer);
    }

    private void LoadMap(SocketIOEvent obj)
    {
        Debug.Log("Load new map");
        string mapName = obj.data["mapName"].str;
        string jsonFile = File.ReadAllText(Application.dataPath + "/Data/Map/" + mapName);
        GameMap map = JsonConvert.DeserializeObject<GameMap>(jsonFile);
        MapDTG.SetMap(map);
    }

    private void SpawnMainPlayer(SocketIOEvent obj)
    {
        List<JSONObject> position = obj.data["position"].list;
        PlayerContainerDTG.SpawnMainPlayer((int) position[0].n, (int) position[1].n);
    }

    private void SpawnPlayer(SocketIOEvent obj)
    {
        List<JSONObject> position = obj.data["position"].list;
        PlayerContainerDTG.SpawnPlayer((int)position[0].n, (int)position[1].n, obj.data["id"].str);
    }

    private void DisconnectPlayer(SocketIOEvent obj)
    {
        PlayerContainerDTG.DestroyPlayer(obj.data["id"].str);
    }
}
