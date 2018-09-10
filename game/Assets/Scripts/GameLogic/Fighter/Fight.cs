using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight {
    public FighterContainerDTG fighterContainerDTG;

    public string Id;
    public int phase = 0;
    private List<Fighter> fightersBlue = new List<Fighter>();
    private List<Fighter> fightersRed = new List<Fighter>();
    private Fighter mainFighter;
    private string turnId;



    public Fight()
    {
       // ExtractFightData(data);
    }
    /*
    private void ExtractFightData(SocketIOEvent data)
    {
        var playerList = data.data["players"];
        Id = data.data["id"].str;
        for(var  i = 0; i < playerList.Count(); i++)
        {
            Fighter fighter = new Fighter(playerList[i]);
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
    }*/

    public List<Fighter> GetFighters()
    {

        return fightersBlue.Concat(fightersRed).ToList();
    }

    public Fighter GetFighter(string id)
    {
        foreach(var fighter in fightersBlue.Concat(fightersRed).ToList())
        {
            if(fighter.Id == id)
            {
                return fighter;
            }
        }

        return null;
    }

    public Fighter GetMainFighter()
    {
        return mainFighter;
    }

    public void SetTurnId(string id)
    {
        /* Reset previous player stats */
        var fighter = GetFighter(turnId);
        if(fighter != null)
        {
            fighter.ResetTurnStats();
        }


        turnId = id;
    }

    public string GetTurnId()
    {
        return turnId;
    }

    public bool IsMainFighterTurn()
    {
        return turnId == mainFighter.Id;
    }

    public List<Vector2> GetFightersPositionsButMainFighter()
    {
        List<Vector2> positions = new List<Vector2>();
        foreach (var fighter in fightersBlue.Concat(fightersRed).ToList())
        {
            if (fighter.Id != mainFighter.Id)
            {
                positions.Add(fighter.Position);
            }
        }
        return positions;
    }
}
