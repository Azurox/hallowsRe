using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDTG : MonoBehaviour {
    private Npc npc;
    private Scenario scenario;
    private Sprite sprite;

    private void OnMouseDown()
    {
        Debug.Log("click on NPC");
    }

    public Scenario GetScenario()
    {
        return scenario;
    }

    public void SetScenario(Scenario scenario)
    {
        this.scenario = scenario;
    }

    public void SetNpc(Npc npc)
    {
        this.npc = npc;
        foreach (var id in this.npc.scenariosId)
        {
            this.npc.scenarios[id] = ResourcesLoader.Instance.GetScenario(id);
        }
        sprite = ResourcesLoader.Instance.GetImage(npc.imageId);
    }

    public Npc GetNpc()
    {
        return npc;
    }
}
