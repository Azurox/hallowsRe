using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManagerController : MonoBehaviour {

    private SocketManager socket;
    private MapController mapController;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    private void Start()
    {
        LoadingProcess();
    }

    private void LoadingProcess()
    {
        LoadingProcessMap();
    }

    private void LoadingProcessMap()
    {
        var guid = socket.Emit(MapRequestAlias.LOAD_MAP);
        socket.AwaitOneResponse(guid, MapReceiverAlias.LOAD_MAP, (string data) =>
        {
            mapController.LoapMap(data);
            LoadingProcessMainCharacterInformation();
        });
    }

    private void LoadingProcessMainCharacterInformation()
    {
        var guid = socket.Emit(MainCharacterRequestAlias.INFORMATION);
        socket.AwaitOneResponse(guid, MainCharacterReceiverAlias.INFORMATION, (string data) =>
        {

        });
    }

    private void LoadingProcessMonsters()
    {

    }

    private void LoadingProcessPlayers()
    {
        ActivateListeners();
    }

    private void ActivateListeners()
    {

    }
}
