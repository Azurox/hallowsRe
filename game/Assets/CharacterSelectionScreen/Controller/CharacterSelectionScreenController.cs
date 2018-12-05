using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionScreenController : MonoBehaviour {

    private SocketManager socket;
    public GameObject NoCharacterFoundPanel;
    public GameObject CreateCharacterPanel;
    public Text Name;


    private void Awake ()
    {
        socket = FindObjectOfType<SocketManager>();
    }
	
	private void Start () {
        var guid = socket.Emit(CharacterRequestAlias.CHARACTERS);
        socket.AwaitOneResponse(guid, CharacterReceiverAlias.CHARACTERS, ShowCharacters);
	}

    private void ShowCharacters(string json)
    {
        Debug.Log(json);
        CharacterReceiverCharacters data = JsonConvert.DeserializeObject<CharacterReceiverCharacters>(json);
        if(data.characters.Count == 0)
        {
            NoCharacterFoundPanel.SetActive(true);
        }
    }

    public void ShowCreateCharacterPanel()
    {
        NoCharacterFoundPanel.SetActive(false);
        CreateCharacterPanel.SetActive(true);
    }

    public void CreateCharacter()
    {
        var guid = socket.Emit(CharacterRequestAlias.CREATE_CHARACTER, new CreateCharacterRequest()
        {
            name = Name.text
        });
        socket.AwaitOneResponse(guid, CharacterReceiverAlias.NAME_ALREADY_TAKEN, NameAlreadyTaken);
    }

    private void NameAlreadyTaken(string _)
    {
        Debug.Log("Name already taken");
    }
}
