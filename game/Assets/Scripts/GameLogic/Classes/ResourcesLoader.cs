using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class ResourcesLoader : Singleton<ResourcesLoader>
{
    protected ResourcesLoader() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Dictionary<string, GameMap> gameMaps = new Dictionary<string, GameMap>();

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
}