using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounser : MonoBehaviour
{
    [SerializeField] float bounseStrength;
    private void OnCollisionEnter(Collision collision)
    {
        MarbleController marbleController = collision.gameObject.GetComponent<MarbleController>();
        if (marbleController == null)
        {
            return;
        }

        marbleController.Bound(transform.position, bounseStrength);
        AudioManager.I.PlaySE("Sponge");
    }
}
