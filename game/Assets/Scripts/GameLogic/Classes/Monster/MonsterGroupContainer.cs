using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupContainer : MonoBehaviour {
    public MonsterGroupDTG MonsterGroupDtg;
    public MainPlayerHandler MainPlayerHandler;
    private Dictionary<string, MonsterGroupDTG> monsterGroups = new Dictionary<string, MonsterGroupDTG>();

    public void Startup(MainPlayerHandler mainPlayerHandler)
    {
        MainPlayerHandler = mainPlayerHandler;
    }

    public void LoadMonsterGroup(MonsterGroup group)
    {
        var go = Instantiate(MonsterGroupDtg, transform).GetComponent<MonsterGroupDTG>();
        monsterGroups[group.id] = go;
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
