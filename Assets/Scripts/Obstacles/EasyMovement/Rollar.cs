using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollar : MonoBehaviour
{
    [SerializeField] Vector3 totalRotateSpeed;

    void Update()
    {
        if (InputManager.instance.IsAttractive)
        {
            transform.Rotate(totalRotateSpeed * Time.deltaTime, Space.Self);
            //transform.localEulerAngles += totalRotateSpeed * Time.deltaTime;
        }
    }
}
