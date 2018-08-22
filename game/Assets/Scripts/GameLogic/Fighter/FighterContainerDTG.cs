using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContainerDTG : MonoBehaviour {

    public GameObject FighterGameObject;
    public GameObject MainFighterGameObject;
    private Dictionary<string, FighterDTG> players = new Dictionary<string, FighterDTG>();
    private MainFighterDTG mainFighter;

    public void SpawnFighter(Fighter fighter)
    {
        GameObject playerGo;
        if (fighter.IsMainPlayer)
        {
            playerGo = Instantiate(MainFighterGameObject);
            mainFighter = playerGo.GetComponent<MainFighterDTG>();
            mainFighter.SetFighter(fighter);
            mainFighter.InitFighter();
        } else
        {
            playerGo = Instantiate(FighterGameObject);
            playerGo.GetComponent<FighterDTG>().SetFighter(fighter);
            playerGo.GetComponent<FighterDTG>().InitFighter();
            players.Add(fighter.Id, playerGo.GetComponent<FighterDTG>());
        }
        playerGo.transform.parent = gameObject.transform;
        playerGo.name = fighter.Id;

    }

    public void MoveFighter(string id, List<Vector2> path)
    {

    }

    public void TeleportFighter(string id, Vector2 position)
    {

    }

}
