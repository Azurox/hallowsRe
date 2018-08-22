using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapReceiver {

    public MapDTG MapDTG;
    public FightMapHandler FightMapHandler;
    public FightMapDTG FightMapDTG; 
    public PlayerContainerDTG PlayerContainerDTG;
    public FighterContainerDTG FighterContainerDTG;
    private SocketIOComponent socket;
    private Fight Fight;

    public FightMapReceiver(SocketIOComponent socket)
    {
        this.socket = socket;
        MapDTG = Object.FindObjectOfType<MapDTG>();
        FightMapDTG = MapDTG.GetComponent<FightMapDTG>();
        FightMapHandler = MapDTG.GetComponent<FightMapHandler>();
        PlayerContainerDTG = Object.FindObjectOfType<PlayerContainerDTG>();
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
        Fight = new Fight(socket, FighterContainerDTG, obj);
        FightMapDTG.Init();


        var blueCells = obj.data["blueCells"];
        for (var i = 0; i < blueCells.Count; i++)
        {
            Vector2 position = new Vector2(blueCells[i]["position"]["x"].n, blueCells[i]["position"]["y"].n);
            bool taken = blueCells[i]["taken"].b;
            FightMapDTG.SetSpawnCell(Side.blue, position, taken);
        }

        var redCells = obj.data["redCells"];
        for (var i = 0; i < blueCells.Count; i++)
        {
            Vector2 position = new Vector2(redCells[i]["position"]["x"].n, redCells[i]["position"]["y"].n);
            bool taken = redCells[i]["taken"].b;
            FightMapDTG.SetSpawnCell(Side.red, position, taken);

        }

        FightMapHandler.Init(FightMapDTG, Fight);

    }

    private void FightFinished()
    {
        PlayerContainerDTG.gameObject.SetActive(false);
        FighterContainerDTG.gameObject.SetActive(true);
    }
}
