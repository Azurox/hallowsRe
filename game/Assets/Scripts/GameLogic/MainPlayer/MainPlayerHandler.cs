using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MainPlayerHandler : MonoBehaviour
{

    private MapPathFinding MapPathFinding;

    private void Start()
    {
        MapPathFinding = FindObjectOfType<MapPathFinding>();
    }

    public void TryMovement(int x, int y, Action endCallBack = null)
    {
        var path = MapPathFinding.FindPath(new Vector2(transform.position.x, transform.position.z), new Vector2(x, y));
       if(path != null)
        {
            gameObject.GetComponent<Movable>().TakePath(new Vector2(transform.position.x, transform.position.z), path, OnMove, endCallBack);
            gameObject.GetComponent<MainPlayerEmitter>().NewPath(path.ToArray());
        }
    }

    public void OnMove(int x, int y)
    {
        GetComponent<MainPlayerEmitter>().NewPosition(x, y);
    }

    public void StartFight(string id)
    {
        GetComponent<MainPlayerEmitter>().StartFight(id);
    }

    public void SelectScenarioResponse(Scenario scenario, int responseIndex, Npc npc)
    {
        GetComponent<MainPlayerEmitter>().SelectScenarioResponse(scenario, responseIndex, npc);
    }

    public void StartMonsterFight(string id)
    {
        GetComponent<MainPlayerEmitter>().StartMonsterFight(id);
    }
}
