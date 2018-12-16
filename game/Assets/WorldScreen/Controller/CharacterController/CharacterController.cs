using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldScreen.CharacterReceiver;

public class CharacterController : MonoBehaviour {

    /* Linked GameObject */
    public CharacterComponent CharacterComponent;


    private SocketManager socket;
    private Dictionary<string,CharacterComponent> currentCharactersComponent = new Dictionary<string, CharacterComponent>();

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void ActivateListeners()
    {
        socket.On(CharacterReceiverAlias.SPAWN_CHARACTER, SpawnCharacter);
        socket.On(CharacterReceiverAlias.REMOVE_CHARACTER, RemoveCharacter);
        socket.On(CharacterReceiverAlias.MOVE_CHARACTER, MoveCharacter);
    }

    public void Spawn(Character character)
    {
        currentCharactersComponent[character.Id] = Instantiate(CharacterComponent, transform);
        currentCharactersComponent[character.Id].Setup(character);
    }

    public void Spawn(HashSet<Character> characters)
    {
        foreach (var character in characters)
        {
            Spawn(character);
        }
    }

    public void Remove(string id)
    {
        if (currentCharactersComponent.ContainsKey(id))
        {
            var character = currentCharactersComponent[id];
           currentCharactersComponent.Remove(id);
            Destroy(character.gameObject);
        } else
        {
            Debug.Log($"tried to remove {id} but character wasn't found");
        }
    }

    private void Move(string id, Vector2 startPosition, List<Vector2> path)
    {
        if (currentCharactersComponent.ContainsKey(id))
        {
            var character = currentCharactersComponent[id];
            character.Move(path, (int x, int y) =>
            {
                character.SetPosition(x, y);
            });

        }
        else
        {
            Debug.Log($"tried to move {id} but character wasn't found");
        }
    }

    private void SpawnCharacter(string data)
    {
        var character = JsonConvert.DeserializeObject<SpawnCharacterReceiver>(data).character;
        Spawn(character);
    }

    private void RemoveCharacter(string data)
    {
        var id = JsonConvert.DeserializeObject<RemoveCharacterReceiver>(data).characterId;
        Remove(id);
    }

    private void MoveCharacter(string data)
    {
        var request = JsonConvert.DeserializeObject<MoveCharacterReceiver>(data);
        Move(request.characterId, request.startPosition, request.path);
    }
}
