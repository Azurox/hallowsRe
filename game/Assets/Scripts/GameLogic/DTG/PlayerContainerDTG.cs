using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainerDTG : MonoBehaviour {
    public GameObject MainPlayerGameObject; 
    public GameObject PlayerGameObject;
    private List<PlayerDTG> players = new List<PlayerDTG>();
    private MainPlayerDTG mainPlayer;

    public void SpawnMainPlayer(int x, int y)
    {
        if(mainPlayer == null)
        {
            var mainPlayerGo = Instantiate(MainPlayerGameObject);
            mainPlayerGo.transform.parent = gameObject.transform;
            mainPlayerGo.name = "mainPlayer";
            mainPlayer = mainPlayerGo.GetComponent<MainPlayerDTG>();
            mainPlayer.SetPosition(x, y);
        }
    }

    public void SpawnPlayer(int x, int y, string name)
    {
        var playerGo = Instantiate(PlayerGameObject);
        playerGo.transform.parent = gameObject.transform;
        playerGo.name = name;
        playerGo.GetComponent<PlayerDTG>().SetPosition(x, y);
        players.Add(playerGo.GetComponent<PlayerDTG>());
    }

}
