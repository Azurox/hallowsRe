using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerHandler : MonoBehaviour {

    public void TryMovement(int x, int y)
    {
       var path = gameObject.GetComponent<MainPlayerPathFinding>().FindPath(x, y);
       if(path != null)
        {
            gameObject.GetComponent<Movable>().TakePath(path);
        }
    }
}
