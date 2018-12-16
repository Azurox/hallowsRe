using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsAccessible { get; set; }
    public bool Obstacle { get; set; }
    public bool OffScreen { get; set; }

    public Cell(int x, int y, bool isAccessible, bool obstacle, bool offScreen)
    {
        this.X = x;
        this.Y = y;
        this.IsAccessible = isAccessible;
        this.Obstacle = obstacle;
        this.OffScreen = offScreen;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(X, Y);
    }

}
