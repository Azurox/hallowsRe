using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupContainer : MonoBehaviour {
    public MonsterGroupDTG MonsterGroupDtg;
    public MainPlayerHandler MainPlayerHandler;
    public MapPathFinding MapPathFinding;
    private Dictionary<string, MonsterGroupDTG> monsterGroups = new Dictionary<string, MonsterGroupDTG>();

    public void Startup(MainPlayerHandler mainPlayerHandler, MapPathFinding mapPathFinding)
    {
        MainPlayerHandler = mainPlayerHandler;
        MapPathFinding = mapPathFinding;
    }

    public void LoadMonsterGroup(MonsterGroup group)
    {
        var go = Instantiate(MonsterGroupDtg, transform).GetComponent<MonsterGroupDTG>();
        monsterGroups[group.id] = go;
        go.Init(MapPathFinding);
        go.SetMonsterGroup(group);
    }

    internal void ClickOnMonster(Vector2 position)
    {
        MainPlayerHandler.TryMovement((int)position.x, (int)position.y, () =>
        {
            // MainPlayerHandler.StartFight();
        });
    }
}
