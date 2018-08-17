using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsAccessible { get; set; }
    public Cell(int x, int y, bool isAccessible)
    {
        this.X = x;
        this.Y = y;
        this.IsAccessible = isAccessible;
    }
}
