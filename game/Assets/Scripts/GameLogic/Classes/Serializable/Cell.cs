using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell {
    [SerializeField]
    private int x { get; set; }
    [SerializeField]
    private int y { get; set; }
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
