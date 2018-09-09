using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTaker : MonoBehaviour
{
    [SerializeField]
    private RenderTexture texture;

    [SerializeField]
    private Camera cameraPicture;

    public Texture2D TakePicture(GameObject go)
    {
        RenderTexture.active = null;
        var oldPosition = go.transform.position;
        cameraPicture.gameObject.SetActive(true);
        var tmp = Instantiate(go, transform);
        tmp.transform.localPosition = new Vector3(0, 0, 2);
        cameraPicture.Render();
        RenderTexture.active = texture;
        Texture2D tex = new Texture2D(300, 500, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        tex.Apply();
        cameraPicture.gameObject.SetActive(false);
        tmp.transform.localPosition = new Vector3(10, 10, 2);
        Destroy(tmp);
        return tex;
    }

}
