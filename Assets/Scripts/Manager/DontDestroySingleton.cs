using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySingleton<T> : MonoBehaviour where T : DontDestroySingleton<T>
{
    public static T I;

    protected void Awake()
    {
        if (I == null)
        {
            I = (T)this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
