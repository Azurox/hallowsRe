using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
    public string name;
    public int level;
    public string monsterId;
    public Vector2 position;

    public Monster(string name, int level, string monsterId, Vector2 position)
    {
        this.name = name;
        this.level = level;
        this.monsterId = monsterId;
        this.position = position;
    }
	
}
