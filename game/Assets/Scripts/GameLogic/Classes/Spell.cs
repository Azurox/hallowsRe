using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {
    public string id;
    public string name;
    public int actionPointCost;
    public int range;
    public Vector2[] hitArea;
    public int physicalDamage;
    public int magicDamage;
    public bool selfUse;
    public bool line;
    public bool heal;
}
