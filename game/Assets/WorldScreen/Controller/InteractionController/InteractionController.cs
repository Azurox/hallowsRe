using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    private MapController MapController;
    private MainCharacterController MainCharacterController;
    private CharacterController CharacterController;

    private void Awake()
    {
        MapController = GetComponent<MapController>();
        MainCharacterController = GetComponent<MainCharacterController>();
        CharacterController = GetComponent<CharacterController>();
    }

    public void PlayerClickOnCell(Cell cell)
    {
        var position = MainCharacterController.GetMainCharacter().Position;
        var path = MapController.FindPath(position, cell.GetPosition());
        if (path != null) MainCharacterController.TryToMove(path);
    }
}
