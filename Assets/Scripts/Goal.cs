using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class Goal : MonoBehaviour
{
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

        GameManager.instance.WhenCleared(other.gameObject);
    }
}
