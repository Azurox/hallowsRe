using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup
{
    public string id;
    public List<Monster> monsters;
    public Vector2 position;

    public MonsterGroup(string id, Vector2 position)
    {
        this.position = position;
        this.id = id;
        this.monsters = new List<Monster>();
    }
}
