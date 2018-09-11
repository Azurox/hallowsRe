using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Main game;
    private SocketManager socket;

	// Use this for initialization
	IEnumerator Start () {
      
        yield return new WaitForSeconds(1); // due to a lib bug, i have to wait a frame before being able to send anything
	    socket = GetComponent<SocketManager>();
		game = new Main(socket);
	}
	
	// Update is called once per frame
	void Update () {

    }
}
