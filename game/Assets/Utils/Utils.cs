using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils> {

    protected Utils() { }

    public PhotoTaker PhotoTaker;

    public System.Random Random = new System.Random();

    public PhotoTaker GetPhotoTaker()
    {
        return PhotoTaker;
    }

    public void DelayCoroutine(float timeSecond, Action callback)
    {
        StartCoroutine(Delay(timeSecond, callback));
    }

    private IEnumerator Delay(float timeSecond, Action callback)
    {
        yield return new WaitForSeconds(timeSecond);
        callback();
    }

}
