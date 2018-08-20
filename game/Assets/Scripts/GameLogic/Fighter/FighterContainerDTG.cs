using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterContainerDTG : MonoBehaviour {

    public GameObject FighterGameObject;

    public void SpawnFighter(Fighter fighter)
    {
        var playerGo = Instantiate(FighterGameObject);
        playerGo.transform.parent = gameObject.transform;
        playerGo.name = fighter.Id;
        playerGo.GetComponent<FighterDTG>().SetFighter(fighter);
        playerGo.GetComponent<FighterDTG>().InitFighter();

    }

}
