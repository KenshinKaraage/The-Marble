using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WaiPoint : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] Spawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }


        PhotonView view = other.GetComponent<PhotonView>();

        if (view.Owner != PhotonNetwork.LocalPlayer)
        {
            return;
        }

        
        spawner.SetWayPoint(pointValue);
    }
}
