using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool isActive;
    private void Update()
    {
        if (isActive)
        {
            //もしクリックされた場合は、カーソルをロック
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            //もしescキーが押された場合は、カーソルを表示
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }   
    }

    public void SetActive()
    {
        isActive = true;
    }

    public void SetNegative()
    {
        Cursor.lockState = CursorLockMode.None;
        isActive = false;
    }
}
