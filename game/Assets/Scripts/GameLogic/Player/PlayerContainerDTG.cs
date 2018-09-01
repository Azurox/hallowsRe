using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainerDTG : MonoBehaviour
{
    public GameObject MainPlayerGameObject;
    public GameObject PlayerGameObject;
    private Dictionary<string, PlayerDTG> players = new Dictionary<string, PlayerDTG>();
    private MainPlayerDTG mainPlayer;

    public GameObject SpawnMainPlayer(int x, int y)
    {
        if (mainPlayer != null)
        {
            Destroy(mainPlayer.gameObject);
        }

        var mainPlayerGo = Instantiate(MainPlayerGameObject);
        mainPlayerGo.transform.parent = gameObject.transform;
        mainPlayerGo.name = "mainPlayer";
        mainPlayer = mainPlayerGo.GetComponent<MainPlayerDTG>();
        mainPlayer.SetPosition(x, y);
        return mainPlayer.gameObject;
    }

    public void SpawnPlayer(int x, int y, string id)
    {
        var playerGo = Instantiate(PlayerGameObject);
        playerGo.transform.parent = gameObject.transform;
        playerGo.name = id;
        playerGo.GetComponent<PlayerDTG>().SetPosition(x, y);
        players.Add(id, playerGo.GetComponent<PlayerDTG>());
    }

    public void DestroyPlayer(string id)
    {
        if (players.ContainsKey(id))
        {
            var player = players[id];
            Destroy(player.gameObject);
            players.Remove(id);
        }
    }

    public void MovePlayer(string id, List<Vector2> path)
    {
        if (players.ContainsKey(id))
        {
            var player = players[id];
            player.GetComponent<Movable>().TakePath(new Vector2(player.transform.position.x, player.transform.position.z), path, null);
        }
    }

    public void Clear()
    {
        foreach (var player in players)
        {
            Destroy(player.Value.gameObject);
        }

        players.Clear();
    }

}
