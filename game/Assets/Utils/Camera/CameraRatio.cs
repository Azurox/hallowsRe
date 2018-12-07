using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    private Vector2 resolution;
    private Camera currentCamera;
    private float initialSize; 

    private void Awake()
    {
        resolution = new Vector2(Screen.width, Screen.height);
        currentCamera = GetComponent<Camera>();
        initialSize = currentCamera.orthographicSize;
        SetCameraShape();
    }

    void Start()
    {
        SetCameraShape();
    }


    private void SetCameraShape()
    {
        currentCamera.orthographicSize = initialSize * (16 / 9f) / currentCamera.aspect;
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = currentCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            currentCamera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = currentCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            currentCamera.rect = rect;
        }
    }


    private void Update()
    {
      if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            SetCameraShape();
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }


}