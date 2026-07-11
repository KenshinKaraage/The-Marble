using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMarble : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    public bool Active { set; get; }

    private void Update()
    {
        if (Active)
        {
            transform.Rotate(0, Input.GetAxisRaw("Horizontal") * rotateSpeed, 0);
        }
    }
}
