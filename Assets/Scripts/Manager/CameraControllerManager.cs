using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerManager : MonoBehaviour
{
    public static CameraControllerManager instance;
    public bool IsAttractive { set; get; }

    private void Awake()
    {
        instance = this;
        IsAttractive = true;
    }
}
