using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell {
    public int X { get; set; }
    public int Y { get; set; }
    public Cell(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}
