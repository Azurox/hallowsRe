using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterHandler : MonoBehaviour {
    private MainFighterHandler mainFighter;

    public void SetMainFighter(MainFighterHandler fighter)
    {
        mainFighter = fighter;
    }

    public void ClickOnFighter(Fighter fighter)
    {
        mainFighter.ClickOnFighter(fighter);
    }
}
