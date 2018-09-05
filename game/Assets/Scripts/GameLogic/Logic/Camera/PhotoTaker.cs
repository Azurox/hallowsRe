using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTaker : MonoBehaviour {
    [SerializeField]
    private RenderTexture texture;

    [SerializeField]
    private Camera cameraPicture;

    public Image TakePicture(GameObject go)
    {
        cameraPicture.gameObject.SetActive(true);
        return null;
    }

}
