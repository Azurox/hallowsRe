using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class ResourcesLoader : Singleton<ResourcesLoader>
{
    protected ResourcesLoader() { }

    private Dictionary<string, GameMap> gameMaps = new Dictionary<string, GameMap>();
    private Dictionary<string, Spell> spells = new Dictionary<string, Spell>();
    private Dictionary<string, Npc> npcs = new Dictionary<string, Npc>();
    private Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();
    private Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();





    public GameMap GetGameMap(string name)
    {
        if (gameMaps.ContainsKey(name))
        {
            return gameMaps[name];
        }
        else
        {
            var jsonTextFile = Resources.Load<TextAsset>("Map/" + name);
            if (jsonTextFile != null)
            {
                GameMap map = JsonConvert.DeserializeObject<GameMap>(jsonTextFile.text);
                gameMaps[name] = map;
                return map;
            }

            Debug.Log("Cannot find map named : " + name);
            return null;
        }
    }

    public Spell GetSpell(string id)
    {
        if (spells.ContainsKey(id))
        {
            return spells[id];
        }
        else
        {
            var jsonTextFile = Resources.Load<TextAsset>("Spell/" + id);
            if (jsonTextFile != null)
            {
                Spell spell = JsonConvert.DeserializeObject<Spell>(jsonTextFile.text);
                spells[id] = spell;
                return spell;
            }

            Debug.Log("Cannot find spell named : " + id);
            return null;
        }
    }

    public Npc GetNpc(string id)
    {
        if (npcs.ContainsKey(id))
        {
            return npcs[id];
        }
        else
        {
            var jsonTextFile = Resources.Load<TextAsset>("Npc/" + id);
            if (jsonTextFile != null)
            {
                Npc npc = JsonConvert.DeserializeObject<Npc>(jsonTextFile.text);
                npcs[id] = npc;
                return npc;
            }

            Debug.Log("Cannot find NPC named : " + id);
            return null;
        }
    }

    public Scenario GetScenario(string id)
    {
        if (scenarios.ContainsKey(id))
        {
            return scenarios[id];
        }
        else
        {
            var jsonTextFile = Resources.Load<TextAsset>("Scenario/" + id);
            if (jsonTextFile != null)
            {
                Scenario scenario = JsonConvert.DeserializeObject<Scenario>(jsonTextFile.text);
                scenarios[id] = scenario;
                return scenario;
            }

            Debug.Log("Cannot find Scenario named : " + id);
            return null;
        }
    }

    public Sprite GetImage(string id)
    {
        if (images.ContainsKey(id))
        {
            return images[id];
        }
        else
        {
            var image = Resources.Load<Sprite>("Image/" + id);
            if (image != null)
            {
                images[id] = image;
                return image;
            }

            Debug.Log("Cannot find Image named : " + id);
            return null;
        }
    }
}