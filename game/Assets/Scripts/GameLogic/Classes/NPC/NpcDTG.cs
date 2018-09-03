using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcDTG : MonoBehaviour {
    private Npc npc;
    private Scenario scenario;
    private Sprite sprite;

    private void OnMouseDown()
    {
        transform.parent.GetComponent<NpcContainerDTG>().ShowScenario(GetMostSuitedScenario()) ;
    }

    private Scenario GetMostSuitedScenario()
    {
        return npc.scenarios.ElementAt(0).Value;   
    }

    public void SetNpc(Npc npc)
    {
        this.npc = npc;
    }

    public void InitNpc()
    {
        foreach (var id in npc.scenariosId)
        {
            npc.scenarios[id] = ResourcesLoader.Instance.GetScenario(id);
        }
        sprite = ResourcesLoader.Instance.GetImage(npc.imageId);
    }

    public Npc GetNpc()
    {
        return npc;
    }
}
