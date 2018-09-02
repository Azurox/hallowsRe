using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterFightUIComponent : MonoBehaviour {

    public GameObject WinPanel;
    public GameObject LoosePanel;

    public Text Message;
    public Text Level;
    public Text xp;
    public Text gold;


    public void ShowAfterfightStats(AfterFightStats afterFightStats)
    {
        if (afterFightStats.win)
        {
            WinPanel.SetActive(true);
            LoosePanel.SetActive(false);
            Message.text = "You won !";
            xp.text = afterFightStats.xp + "xp";
            gold.text = afterFightStats.gold + "gold";

        }
        else
        {
            WinPanel.SetActive(false);
            LoosePanel.SetActive(true);
            Message.text = "You lost !";
        }
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
