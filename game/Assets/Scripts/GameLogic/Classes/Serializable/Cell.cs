using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell {
    private int x { get; set; }
    private int y { get; set; }
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
