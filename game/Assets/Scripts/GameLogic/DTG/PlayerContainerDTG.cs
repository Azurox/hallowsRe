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
            mainPlayerGo.transform.parent = gameObject.transform.parent;
            mainPlayerGo.name = "mainPlayer";
            mainPlayer = mainPlayerGo.GetComponent<MainPlayerDTG>();
            mainPlayer.SetPosition(x, y);
        }
    }

}
