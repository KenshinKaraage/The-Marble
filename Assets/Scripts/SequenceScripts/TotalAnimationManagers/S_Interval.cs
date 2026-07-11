using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Interval : MonoBehaviour
{
    [SerializeField] S_AppearFadeIn windowFadeIn;

    public void AppearWindow()
    {
        windowFadeIn.GetAppearFadeInSequence();
    }
}
