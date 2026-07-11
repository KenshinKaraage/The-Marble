using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool IsAttractive { set; get; }

    private void Awake()
    {
        instance = this;
        IsAttractive = true;
    }
}
