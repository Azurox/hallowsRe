using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldScreen.MapReceiver;

public class MapController : MonoBehaviour
{
    /* Linked GameObject */
    public MapComponent MapComponent;


    private SocketManager socket;
    private MapComponent currentMapComponent;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void LoadMap(LoadMapReceiver data)
    {
        GameMap map = ResourcesLoader.Instance.GetGameMap(data.mapName);
        currentMapComponent = Instantiate(MapComponent, transform);
        currentMapComponent.Setup(map);
    }
}
