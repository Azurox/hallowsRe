using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Main game;
    private SocketIOComponent socket;

	// Use this for initialization
	void Start () {
        socket = GetComponent<SocketIOComponent>();
		game = new Main(socket);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
