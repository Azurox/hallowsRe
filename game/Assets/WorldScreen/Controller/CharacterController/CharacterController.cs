using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    /* Linked GameObject */
    public CharacterComponent CharacterComponent;


    private SocketManager socket;
    private CharacterComponent currentCharacterComponent;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void Spawn(Character character)
    {
        currentCharacterComponent = Instantiate(CharacterComponent, transform);
        currentCharacterComponent.Setup(character);
    }

    public void Spawn(HashSet<Character> characters)
    {
        foreach (var character in characters)
        {
            Spawn(character);
        }
    }
}
