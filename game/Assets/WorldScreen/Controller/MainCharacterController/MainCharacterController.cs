using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour {

    /* Linked GameObject */
    public MainCharacterComponent MainCharacterComponent;


    private SocketManager socket;
    private MainCharacterComponent currentMainCharacterComponent;

    private void Awake()
    {
        socket = FindObjectOfType<SocketManager>();
    }

    public void Spawn(Character character)
    {
        currentMainCharacterComponent = Instantiate(MainCharacterComponent, transform);
        currentMainCharacterComponent.Setup(character);
    }
}
