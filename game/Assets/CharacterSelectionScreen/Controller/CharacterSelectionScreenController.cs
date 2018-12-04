using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenController : MonoBehaviour {

    public SocketManager socket;

    private void Awake ()
    {
        socket = Object.FindObjectOfType<SocketManager>();
    }
	
	private void Start () {
	}
}
