using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : Singleton<PlayerInformation>
{
    protected PlayerInformation() { }

    private Texture2D playerImage;
    private bool playerImageIsDirty = true;

    private GameObject playerGameObject;
    private GameObject fighterGameObject;

    public Texture2D GetPlayerImage()
    {
        if(playerImage == null || playerImageIsDirty)
        {
            playerImage = Utils.Instance.GetPhotoTaker().TakePicture(playerGameObject);
            playerImageIsDirty = false;
        }
        return playerImage;
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
