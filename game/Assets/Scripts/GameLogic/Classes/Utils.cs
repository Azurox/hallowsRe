using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Singleton<Utils> {

    protected Utils() { }

    public PhotoTaker PhotoTaker;
    
    public PhotoTaker GetPhotoTaker()
    {
        return PhotoTaker;
    }
   
}
