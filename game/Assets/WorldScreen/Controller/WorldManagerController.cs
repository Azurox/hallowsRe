using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WorldScreen.CharacterReceiver;
using WorldScreen.MainCharacterReceiver;
using WorldScreen.MapReceiver;

public class WorldManagerController : MonoBehaviour {

    private SocketManager socket;
    private MapController mapController;
    private MainCharacterController MainCharacterController;
    private CharacterController CharacterController;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
        mapController = GetComponent<MapController>();
        MainCharacterController = GetComponent<MainCharacterController>();
        CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        LoadingProcess();
    }

    private void LoadingProcess()
    {
       /* mapController.LoadMap(new LoadMapReceiver()
        {
            mapName = "0-0",
            position = Vector2.zero
        });*/

        LoadingProcessMap();
    }

    private void LoadingProcessMap()
    {
        var guid = socket.Emit(MapRequestAlias.LOAD_MAP);
        socket.AwaitOneResponse(guid, MapReceiverAlias.LOAD_MAP, (string data) =>
        {
            mapController.LoadMap(JsonConvert.DeserializeObject<LoadMapReceiver>(data));
            LoadingProcessMainCharacterInformation();
        });
    }

    private void LoadingProcessMainCharacterInformation()
    {
        var guid = socket.Emit(MainCharacterRequestAlias.INFORMATION);
        socket.AwaitOneResponse(guid, MainCharacterReceiverAlias.INFORMATION, (string data) =>
        {
            var mainCharacter = JsonConvert.DeserializeObject<InformationReceiver>(data).character;
            MainCharacterController.Spawn(mainCharacter);
            LoadingProcessCharacters();
        });
    }

    private void LoadingProcessCharacters()
    {
        var guid = socket.Emit(CharacterRequestAlias.CHARACTERS_ON_MAP);
        socket.AwaitOneResponse(guid, CharacterReceiverAlias.CHARACTERS_ON_MAP, (string data) =>
        {
            var characters = JsonConvert.DeserializeObject<CharactersOnMapReceiver>(data).characters;
            CharacterController.Spawn(characters);
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
