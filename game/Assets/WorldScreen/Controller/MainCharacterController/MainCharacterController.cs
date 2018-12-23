using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldScreen.MainCharacterReceiver;
using WorldScreen.MapReceiver;

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

    public Character GetMainCharacter()
    {
        return currentMainCharacterComponent.GetCharacter();
    }

    public void TryToMove(List<Vector2> path)
    {
        currentMainCharacterComponent.Move(path, (int x, int y) =>
        {
            currentMainCharacterComponent.SetPosition(x, y);
        });

        List<Position> normalizePath = new List<Position>();
        foreach (var pos in path)
        {
            normalizePath.Add(new Position(pos));
        }

        var guid = socket.Emit(MainCharacterRequestAlias.MOVE_TO, new MoveToRequest()
        {
            path = normalizePath
        });

        socket.AwaitOneResponse(guid, MainCharacterReceiverAlias.ILLEGAL_MOVEMENT, (data) =>
        {
            Debug.Log("illegal");
        });

        socket.AwaitOneResponse(guid, MainCharacterReceiverAlias.LEGAL_MOVEMENT, (data) =>
        {
            Debug.Log("legal");
        });
    }
}
