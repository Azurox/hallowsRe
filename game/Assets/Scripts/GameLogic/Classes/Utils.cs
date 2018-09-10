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
   
}
