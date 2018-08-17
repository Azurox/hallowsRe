using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapController {

    public MapDTG MapDTG;
    public MapHandler MapHandler;
    public PlayerContainerDTG PlayerContainerDTG;
    public PlayerHandler PlayerHandler;
    public FighterContainerDTG FighterContainerDTG;
    private SocketIOComponent socket;

    public FightMapController(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        MapHandler = MapDTG.GetComponent<MapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
        PlayerHandler = PlayerContainerDTG.GetComponent<PlayerHandler>();
        FighterContainerDTG = Object.FindObjectOfType<FighterContainerDTG>();
        FighterContainerDTG.gameObject.SetActive(false);
        InitSocket();
    }

    private void InitSocket()
    {
        socket.On("fightStarted", FightStarted);
    }

    private void FightStarted(SocketIOEvent obj)
    {
        Debug.Log("FightStarted !");
        PlayerContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);

    }
}
