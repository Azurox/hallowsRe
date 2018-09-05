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

}
