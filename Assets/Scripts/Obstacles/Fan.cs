using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float forceValue;
    Vector3 addForceDirection;

    private void Update()
    {
        addForceDirection = transform.forward;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        rb.AddForce(addForceDirection * forceValue);
    }
}
