using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSaucer : MonoBehaviour
{
    [SerializeField] float fromValue;
    [SerializeField] float toValue;
    [SerializeField] float changeTime;

    public float nowValue { set; get; }
    float timer;

    private void Start()
    {
        nowValue = fromValue;
        timer = 0;
    }

    private void Update()
    {
        if(timer <= changeTime)
        {
            timer += Time.deltaTime;
            nowValue = Mathf.Lerp(fromValue, toValue, (timer / changeTime));
        }
    }
}
