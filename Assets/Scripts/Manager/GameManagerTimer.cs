using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerTimer : MonoBehaviour
{
    public bool IsActive { get; set; }
    public bool IsTimeUp { get; set; }

    public float Timer { set; get; }

    public void Initialized(float time)
    {
        IsTimeUp = false;
        IsActive = false;
        Timer = time;
    }

    private void Update()
    {
        if (IsActive)
        {
            Timer -= Time.deltaTime;
        }
    }
}
