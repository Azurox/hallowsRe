using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class ResourcesLoader : Singleton<ResourcesLoader>
{
    protected ResourcesLoader() { }

    private Dictionary<string, GameMap> gameMaps = new Dictionary<string, GameMap>();
    private Dictionary<string, Spell> spells = new Dictionary<string, Spell>();


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
}