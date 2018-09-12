using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupContainer : MonoBehaviour
{
    public MonsterGroupDTG MonsterGroupDtg;
    private MainPlayerHandler MainPlayerHandler;
    private MapPathFinding MapPathFinding;
    private Dictionary<string, MonsterGroupDTG> monsterGroups = new Dictionary<string, MonsterGroupDTG>();

    public void Startup(MapPathFinding mapPathFinding)
    {
        MapPathFinding = mapPathFinding;
    }

    public void Init(MainPlayerHandler mainPlayerHandler)
    {
        MainPlayerHandler = mainPlayerHandler;
    }

    public void LoadMonsterGroup(MonsterGroupResponse group)
    {
        Debug.Log("LoadMonsterGroup");
        var monsterGroup = new MonsterGroup(group.id, group.position);
        foreach (var monsterResponse in group.monsters)
        {
            monsterGroup.monsters.Add(new Monster(monsterResponse.name, monsterResponse.level ,monsterResponse.id, group.position));
        }
        
        var go = Instantiate(MonsterGroupDtg, transform).GetComponent<MonsterGroupDTG>();
        monsterGroups[monsterGroup.id] = go;
        go.Init(MapPathFinding);
        go.SetMonsterGroup(monsterGroup);
        go.UpdateMonsterGroupPosition(monsterGroup.position, true);
    }

    public void ClickOnMonster(Vector2 position, string id)
    {
        MainPlayerHandler.TryMovement((int)position.x, (int)position.y, () =>
        {
            MainPlayerHandler.StartMonsterFight(id);
        });
    }

}
