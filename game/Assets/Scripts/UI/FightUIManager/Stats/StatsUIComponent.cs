using UnityEngine;
using UnityEngine.UI;

public class StatsUIComponent : MonoBehaviour {
    private Fighter fighter;

    public Text PlayerName;
    public Image PlayerImage;
    public Text PlayerLevel;
    public Text PlayerLife;
    public Text PlayerSpeed;
    public Text PlayerActionPoint;
    public Text PlayerMovementPoint;

    public void SetFighter(Fighter fighter)
    {
        if (this.fighter != null)
        {
            this.fighter.StatsChange -= RefreshStatsHUD; 
        }

        this.fighter = fighter;
        this.fighter.StatsChange += RefreshStatsHUD;
        RefreshStatsHUD();
    }

    public void RefreshStatsHUD()
    {
        PlayerName.text = fighter.Name;
        PlayerLevel.text = "Level: 0";
        PlayerLife.text = fighter.GetCurrentLife() + "/" + fighter.GetLife();
        PlayerSpeed.text = fighter.GetCurrentSpeed() + " Speed";
        PlayerActionPoint.text = fighter.GetCurrentActionPoint() + " AP";
        PlayerMovementPoint.text = fighter.GetCurrentMovementPoint() + " MP";
    }

}
