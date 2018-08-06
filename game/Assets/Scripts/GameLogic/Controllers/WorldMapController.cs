using Newtonsoft.Json;
using SocketIO;
using System.IO;
using UnityEngine;

public class WorldMapController {

    public MapDTG MapDTG;
    private SocketIOComponent socket;

    public WorldMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("loadMap", LoadMap);
    }

    private void LoadMap(SocketIOEvent obj)
    {
        Debug.Log("Load new map");
        string mapName = obj.data["mapName"].str;
        string jsonFile = File.ReadAllText(Application.dataPath + "/Data/Map/" + mapName);
        GameMap map = JsonConvert.DeserializeObject<GameMap>(jsonFile);
        MapDTG.SetMap(map);
    }

}
