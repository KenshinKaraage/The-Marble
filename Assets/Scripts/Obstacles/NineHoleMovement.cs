using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NineHoleMovement : MonoBehaviour
{
    [SerializeField] float turnPeriod;
    [SerializeField] ValueSaucer valueSaucer;

    float turnTimer;

    private void Start()
    {
        turnTimer = 0;
    }

    private void Update()
    {
        turnTimer += Time.deltaTime;

        transform.eulerAngles = new Vector3(Mathf.Cos((turnTimer/turnPeriod) * 2 * Mathf.PI) * valueSaucer.nowValue, transform.eulerAngles.y, Mathf.Sin((turnTimer / turnPeriod) * 2 * Mathf.PI) * valueSaucer.nowValue);

        if (turnTimer > turnPeriod)
        {
            turnTimer = 0;
        }
    }
}
