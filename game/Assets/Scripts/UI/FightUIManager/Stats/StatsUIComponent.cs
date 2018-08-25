using UnityEngine;
using UnityEngine.UI;

public class StatsUIComponent : MonoBehaviour {
    private Fighter fighter;

    public Text PlayerName;
    public Image PlayerImage;
    public Text PlayerLevel;
    public Text PlayerLife;
    public Text PlayerSpeed;
    public Text PlayerArmor;
    public Text PlayerMR;

    public void SetFighter(Fighter fighter)
    {
        this.fighter = fighter;
        RefreshStatsHUD();
    }

    public void RefreshStatsHUD()
    {
        PlayerName.text = fighter.Name;
        PlayerLevel.text = "Level 0";
        PlayerLife.text = fighter.Life + "/" + fighter.MaxLife;
        PlayerSpeed.text = "0 Speed";
        PlayerArmor.text = "0 Armor";
        PlayerMR.text = "0 MR";
    }

}
