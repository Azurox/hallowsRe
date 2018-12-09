using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterComponent : MonoBehaviour {

    internal void Setup(Character character)
    {
        transform.position = new Vector3(character.Position.x, transform.position.y, character.Position.y);
    }
}
