using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : Singleton<PlayerInformation>
{
    protected PlayerInformation() { }

    private Image playerImage;

    private GameObject playerGameObject;
    private GameObject fighterGameObject;

    public Image GetPlayerImage()
    {
        return Utils.Instance.GetPhotoTaker().TakePicture(playerGameObject);
    }

    public void SetPlayerGameObject(GameObject gameObject)
    {
        playerGameObject = gameObject;
    }

    public void SetFighterGameObject(GameObject gameObject)
    {
        fighterGameObject = gameObject;
    }

}
