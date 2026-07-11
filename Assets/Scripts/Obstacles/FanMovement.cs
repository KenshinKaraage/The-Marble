using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime , 0);
    }
}
