using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameMap {
    public string name { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public Cell[,] cells { get; set; }
}