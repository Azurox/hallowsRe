using Newtonsoft.Json;
using SocketIO;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldMapController {

    public MapDTG MapDTG;
    public MapHandler MapHandler;
    public PlayerContainerDTG PlayerContainerDTG;
    private SocketIOComponent socket;

    public WorldMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        MapHandler = MapDTG.gameObject.GetComponent<MapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
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

    private void LoadMap(SocketIOEvent obj)
    {
        Debug.Log("Load new map");
        string mapName = obj.data["mapName"].str;
        string jsonFile = File.ReadAllText("C:/Project/Public/hallowsRe/game/MapData/" + mapName);
        GameMap map = JsonConvert.DeserializeObject<GameMap>(jsonFile);
        MapDTG.SetMap(map);

    }

    private void SpawnMainPlayer(SocketIOEvent obj)
    {
        List<JSONObject> position = obj.data["position"].list;
        var player = PlayerContainerDTG.SpawnMainPlayer((int) position[0].n, (int) position[1].n);
        MapHandler.SetMainPlayer(player.GetComponent<MainPlayerHandler>());
    }

    private void SpawnPlayer(SocketIOEvent obj)
    {
        List<JSONObject> position = obj.data["position"].list;
        PlayerContainerDTG.SpawnPlayer((int)position[0].n, (int)position[1].n, obj.data["id"].str);
        if (!obj.data["path"].IsNull)
        {
            Debug.Log("player is currently moving");
            List<JSONObject> positions = obj.data["path"].list;
            List<Vector2> path = new List<Vector2>();

            foreach (var pos in positions)
            {
                Debug.Log(pos);
                path.Add(new Vector2(pos["x"].n, pos["y"].n));
                PlayerContainerDTG.MovePlayer(obj.data["id"].str, path);
            }

            PlayerContainerDTG.MovePlayer(obj.data["id"].str, path);

        }
    }

    private void DisconnectPlayer(SocketIOEvent obj)
    {
        PlayerContainerDTG.DestroyPlayer(obj.data["id"].str);
    }

    private void PlayerMove(SocketIOEvent obj)
    {
        List<Vector2> path = new List<Vector2>();
        List<JSONObject> positions = obj.data["path"].list;
        foreach(var position in positions) {
            Debug.Log(position);
            path.Add(new Vector2(position["x"].n, position["y"].n));
        }

        PlayerContainerDTG.MovePlayer(obj.data["id"].str, path);
    }
}
