using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerHandler : MonoBehaviour {

    public void TryMovement(int x, int y)
    {
       var path = gameObject.GetComponent<MainPlayerPathFinding>().FindPath(x, y);
       if(path != null)
        {
            gameObject.GetComponent<Movable>().TakePath(new Vector2(transform.position.x, transform.position.z), path, OnMove);
            gameObject.GetComponent<MainPlayerEmitter>().NewPath(path.ToArray());
        }
    }

    public void OnMove(int x, int y)
    {
        gameObject.GetComponent<MainPlayerEmitter>().NewPosition(x, y);
    }

    public void StartFight(string id)
    {
        gameObject.GetComponent<MainPlayerEmitter>().StartFight(id);
    }
}
