using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerMaterialManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Renderer ren;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //データの送信
            stream.SendNext(DatabaseManager.instance.nowPatternCode);
            stream.SendNext(DatabaseManager.instance.nowColorCode);
        }
        else
        {
            //データの受信
            int patternCode = (int)stream.ReceiveNext();
            int colorCode = (int)stream.ReceiveNext();

            ren.material.SetTexture("_MainTex", DatabaseManager.instance.patternDatas[patternCode].texture);
            ren.material.SetColor("_Color1", DatabaseManager.instance.colorDatas[colorCode].colors[0]);
            ren.material.SetColor("_Color2", DatabaseManager.instance.colorDatas[colorCode].colors[1]);
        }
    }

    private void Start()
    {
        ren.material.SetTexture("_MainTex", DatabaseManager.instance.GetPatternData().texture);
        ren.material.SetColor("_Color1", DatabaseManager.instance.GetColorData().colors[0]);
        ren.material.SetColor("_Color2", DatabaseManager.instance.GetColorData().colors[1]);
    }
}
