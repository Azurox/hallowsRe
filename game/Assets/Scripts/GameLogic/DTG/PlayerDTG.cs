﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDTG : MonoBehaviour {

    public void SetPosition(int x, int y)
    {
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, y);
    }
}
