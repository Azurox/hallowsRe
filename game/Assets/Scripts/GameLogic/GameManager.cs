using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Main game;
    private SocketIOComponent socket;

	// Use this for initialization
	IEnumerator Start () {
        socket = GetComponent<SocketIOComponent>();
        yield return new WaitForSeconds(2); // due to a lib bug, i have to wait a frame before being able to send anything
		game = new Main(socket);
	}
	
	// Update is called once per frame
	void Update () {

    }
}
