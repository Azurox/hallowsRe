using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcContainerDTG : MonoBehaviour {

    public NpcDTG NpcDTG;
    private WorldUIManager WorldUIManager;
    private List<NpcDTG> npcs = new List<NpcDTG>();

    public void Init(WorldUIManager worldUIManager)
    {
        this.WorldUIManager = worldUIManager;
    }

    public void LoadNPC(string id)
    {
        var npc = ResourcesLoader.Instance.GetNpc(id);
        var go = Instantiate(NpcDTG, transform);
        NpcDTG npcDtg = go.GetComponent<NpcDTG>();
        npcDtg.SetNpc(npc);
        npcs.Add(npcDtg);
    }

    public void ShowScenario(Scenario scenario)
    {
        WorldUIManager.ShowScenario(scenario);
    }

    public void Clear()
    {
        foreach (var npc in npcs)
        {
            Destroy(npc.gameObject);
        }
        npcs.Clear();
    }
	
}
