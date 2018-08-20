using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight {
    private SocketIOComponent socket;
    public FighterContainerDTG fighterContainerDTG;
    public List<Fighter> fightersBlue = new List<Fighter>();
    public List<Fighter> fightersRed = new List<Fighter>();


    public Fight(SocketIOComponent socket, FighterContainerDTG fighterContainerDTG, SocketIOEvent data)
    {
        this.socket = socket;
        this.fighterContainerDTG = fighterContainerDTG;
        ExtractFightData(data);
    }

    private void ExtractFightData(SocketIOEvent data)
    {
        var playerList = data.data["players"];

        for(var  i = 0; i < playerList.Count; i++)
        {
            Fighter fighter = new Fighter(playerList[i]);
            Debug.Log(playerList[i]);
            if(fighter.Side == Side.blue)
            {
                fightersBlue.Add(fighter);
            }else
            {
                fightersRed.Add(fighter);
            }

            fighterContainerDTG.SpawnFighter(fighter);
        }

    }


}
