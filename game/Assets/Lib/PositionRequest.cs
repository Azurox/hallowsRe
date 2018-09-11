using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRequest
{
    public int x;
    public int y;

    public PositionRequest(Vector2 position)
    {
        x = (int) position.x;
        y = (int) position.y;
    }
}
