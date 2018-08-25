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
        this.fighter = fighter;
        RefreshStatsHUD();
    }

    public void RefreshStatsHUD()
    {
        PlayerName.text = fighter.Name;
        PlayerLevel.text = "Level: 0";
        PlayerLife.text = fighter.CurrentLife + "/" + fighter.CurrentLife;
        PlayerSpeed.text = fighter.CurrentSpeed + " Speed";
        PlayerActionPoint.text = fighter.CurrentActionPoint + " AP";
        PlayerMovementPoint.text = fighter.CurrentMovementPoint + " MP";
    }

}
