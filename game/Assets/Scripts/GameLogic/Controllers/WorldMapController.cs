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
        Debug.Log("Spawn main Player");
        List<JSONObject> position = obj.data["position"].list;
        Debug.Log("x " + position[0].n);
        Debug.Log("y " + position[1].n);
        PlayerContainerDTG.SpawnMainPlayer((int) position[0].n, (int) position[1].n);
    }


}
