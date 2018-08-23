using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimelineUIComponent : MonoBehaviour {

    public GameObject FighterMiniature;

    private FightUIManager FightUIManager;
    private List<FighterMiniatureUIComponent> Timeline = new List<FighterMiniatureUIComponent>();

    private void Start()
    {
        FightUIManager = GetComponentInParent<FightUIManager>();
    }

    public void UpdateFightTimeline(List<Fighter> fighters)
    {
        List<Fighter> sortedFighters = fighters.OrderBy(f => f.Order).ToList();
        foreach(var go in Timeline)
        {
            Destroy(go);
        }
        Timeline.Clear();
        foreach(var fighter in sortedFighters)
        {
            var go = Instantiate(FighterMiniature);
            go.transform.SetParent(transform, false);
            var min = go.GetComponent<FighterMiniatureUIComponent>();
            min.Init(FightUIManager);
            min.SetFighter(fighter);
            Timeline.Add(min);
        }
    }
}
