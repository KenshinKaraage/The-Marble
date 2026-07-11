using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Roller : MonoBehaviour, I_VelocityAdder
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float addSpeedValue;

    public Vector3 GetAddSpeed(Vector3 playerPosition)
    {
        float angle = Mathf.Atan2(playerPosition.z - transform.position.z, playerPosition.x - transform.position.x);
        Vector3 addSpeedDir = new Vector3(Mathf.Sin(angle),0,-Mathf.Cos(angle));
        return addSpeedDir * addSpeedValue * Mathf.Clamp(rotationSpeed, -1, 1);
    }

    void Update()
    {
        if (InputManager.instance.IsAttractive)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
