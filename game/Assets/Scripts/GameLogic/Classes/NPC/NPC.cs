using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc {
    public string[] scenariosId;
    public Vector2 position;
    public int orientation;
    public string imageId;
    public Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();
}
