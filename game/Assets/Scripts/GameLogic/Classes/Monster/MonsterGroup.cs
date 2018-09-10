using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup
{
    public string id;
    public List<Monster> monsters = new List<Monster>();
    public Vector2 position;

    public MonsterGroup(string id, List<Monster> monsters, Vector2 position)
    {
        this.position = position;
        this.monsters = monsters;
        this.id = id;
    }
}
