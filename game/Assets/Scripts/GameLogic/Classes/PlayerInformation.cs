using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : Singleton<PlayerInformation>
{
    protected PlayerInformation() { }



    private Texture2D playerImage;
    private Texture2D fighterImage;

    private bool playerImageIsDirty = true;
    private bool fighterImageIsDirty = true;


    private GameObject playerGameObject;
    private GameObject fighterGameObject;

    public Texture2D GetPlayerImage(bool forceRetake = false)
    {
        if(playerImage == null || playerImageIsDirty || forceRetake)
        {
            playerImage = Utils.Instance.GetPhotoTaker().TakePicture(playerGameObject);
            playerImageIsDirty = false;
        }
        return playerImage;
    }

    public Texture2D GetFighterImage(bool forceRetake = false)
    {
        if (fighterImage == null || fighterImageIsDirty || forceRetake)
        {
            fighterImage = Utils.Instance.GetPhotoTaker().TakePicture(fighterGameObject);
            fighterImageIsDirty = false;
        }
        return fighterImage;
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
