using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class O_Booster : MonoBehaviourPunCallbacks
{
    [SerializeField] float multiVelocityValue;
    [SerializeField] float multiVelocityTime;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag != "Player")
        {
            return;
        }

        MarbleController marble = other.gameObject.GetComponent<MarbleController>();
        marble.MuntiSpeed = multiVelocityValue;
        marble.MuntiSpeedTime = multiVelocityTime;

        PhotonView view = marble.gameObject.GetComponent<PhotonView>();

        if (view.Owner != PhotonNetwork.LocalPlayer)
        {
            return;
        }

        //SE‚ð—¬‚·
        AudioManager.I.PlaySE("Boost");

        if (CameraEffectManager.instance != null)
        {
            CameraEffectManager.instance.BoostEffect();

        }
    }

    public void Effect()
    {

    }
}
