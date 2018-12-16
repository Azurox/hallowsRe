using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    public int X;
    public int Y;

    public Position(Vector2 vector)
    {
        X = (int)vector.x;
        Y = (int)vector.y;
    }
}
