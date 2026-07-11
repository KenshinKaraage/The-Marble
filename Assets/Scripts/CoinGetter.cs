using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CoinGetter : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            Coin coin = other.gameObject.GetComponent<Coin>();
            if (coin == null)
            {
                return;
            }

            AudioManager.I.PlaySE("Coin");
            GameManager.instance.AddPlayerScore(coin.Score);
            coin.Destroy();
        }
    }

}
