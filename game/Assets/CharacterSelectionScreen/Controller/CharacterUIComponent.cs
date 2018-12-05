using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIComponent : MonoBehaviour {
    public Text name;
    public Text level;
    private Character character;

    public void Setup(Character character)
    {
        this.character = character;
        name.text = character.Name;
        level.text = $"level : {character.Level}";
    }

    public void Connect()
    {
        GetComponentInParent<CharacterSelectionScreenController>().Connect(character.Name);
    }
}
