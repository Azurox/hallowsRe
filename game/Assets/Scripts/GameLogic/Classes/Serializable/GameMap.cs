using System;

public class GameMap {
    public string name { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public Cell[,] cells { get; set; }
    public string[] npcs { get; set; }
}