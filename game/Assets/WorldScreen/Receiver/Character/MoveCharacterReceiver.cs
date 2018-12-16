using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterReceiver : IReceiver {
    public string characterId;
    public Vector2 startPosition;
    public List<Vector2> path;
}
