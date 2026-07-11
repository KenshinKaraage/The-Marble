using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Coin : MonoBehaviourPunCallbacks
{
    public int Score;

    public void Destroy()
    {
        photonView.RPC(nameof(DestroyWithMasterClient), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void DestroyWithMasterClient()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
