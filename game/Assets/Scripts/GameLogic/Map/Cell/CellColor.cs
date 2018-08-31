using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColor {
    public Color color;
    public int priority;

    public CellColor(Color color, int priority)
    {
        this.color = color;
        this.priority = priority;
    }
}
