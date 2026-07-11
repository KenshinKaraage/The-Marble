using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class IntervalManager : MonoBehaviour
{
    IntervalManagerUI ui;

    private void Awake()
    {
        ui = GetComponent<IntervalManagerUI>();
    }

    private void Start()
    {
        StartCoroutine(IntervalCoroutine());
    }

    IEnumerator IntervalCoroutine()
    {
        //BGMを流す
        AudioManager.I.PlayBGM("Interval");

        ui.AppearWindow();
        //現在どのステージにいるかを取得
        int nowStage = (int)PhotonNetwork.CurrentRoom.CustomProperties["NowScene"];

        SceneData nowScene = DatabaseManager.instance.sceneDataList[nowStage];

        //まずインターバルUIをRoomのカスタムプロパティーに基づいて調整
        ui.ChangeIntervalUI(nowScene);

        //ウェイトの後、シーンに移動
        yield return new WaitForSeconds(5.0f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(nowScene.sceneName);
        }
    }
}
