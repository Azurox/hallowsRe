using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterComponent : MonoBehaviour {

    private Character character;

    public void Setup(Character character)
    {
        this.character = character;
        transform.position = new Vector3(character.Position.x, transform.position.y, character.Position.y);
        name = character.Name;
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetPosition(int x, int y)
    {
        SetPosition(new Vector2(x, y));
    }

    public void SetPosition(Vector2 pos)
    {
        character.Position = pos;
        transform.position = new Vector3(character.Position.x, transform.position.y, character.Position.y);
    }

    public void Move(List<Vector2> path, Action<int, int> callback = null, Action endCallBack = null)
    {
        GetComponent<Movable>().TakePath(character.Position, path, callback, endCallBack);
    }
}
