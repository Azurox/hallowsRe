using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterHandler : MonoBehaviour {
    private MainFighterHandler mainFighter;

    public void SetMainFighter(MainFighterDTG fighter)
    {
        mainFighter = fighter.GetComponent<MainFighterHandler>();
    }

    public void ClickOnFighter(Fighter fighter)
    {
        mainFighter.ClickOnFighter(fighter);
    }
}
