using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter {
    public string Name { get; set; }
    public string Id { get; set; }
    public int MaxLife { get; set; }
    public int Life { get; set; }
    public Vector2 Position { get; set; }
    public int Order { get; set; }
    public bool IsMainPlayer { get; set; }
    public Side Side { get; set; }

    public Fighter(JSONObject data)
    {
        Name = data["name"] != null ? data["name"].str : null;
        Id = data["id"] != null ? data["id"].str : null;
        MaxLife = data["maxLife"] != null ? (int)data["maxLife"].n : 0;
        Life = data["Life"] != null ? (int)data["Life"].n : 0;
        IsMainPlayer = data["isMainPlayer"] != null ? data["isMainPlayer"].b : false;
        Side = data["side"] != null ? (Side)System.Enum.Parse(typeof(Side), data["side"].str) : Side.blue;
        Position = new Vector2(data["position"]["x"].n, data["position"]["y"].n);
    }
}
