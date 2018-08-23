using SocketIO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight {
    private SocketIOComponent socket;
    public FighterContainerDTG fighterContainerDTG;

    public string Id;
    public int phase = 0;
    private List<Fighter> fightersBlue = new List<Fighter>();
    private List<Fighter> fightersRed = new List<Fighter>();
    private Fighter mainFighter;



    public Fight(SocketIOComponent socket, SocketIOEvent data)
    {
        this.socket = socket;
        ExtractFightData(data);
    }

    private void ExtractFightData(SocketIOEvent data)
    {
        var playerList = data.data["players"];
        Id = data.data["id"].str;
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

            if (fighter.IsMainPlayer)
            {
                mainFighter = fighter;
            }
        }
    }

    public List<Fighter> GetFighters()
    {

        return fightersBlue.Concat(fightersRed).ToList();
    }

    public Fighter GetMainFighter()
    {
        return mainFighter;
    }
}
