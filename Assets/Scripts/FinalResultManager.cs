using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using Photon.Realtime;
using Photon.Pun;

public class FinalResultManager : MonoBehaviourPunCallbacks
{
    FinalResultManagerUI ui;
    [SerializeField] List<Transform> ballParents = new List<Transform>();
    [SerializeField] ResultBall ballPrefab;
    [SerializeField] S_FfnalResultCameraMovement ffnalResultCameraMovement;

    private void Awake()
    {
        ui = GetComponent<FinalResultManagerUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResultCoroutine());
    }

    IEnumerator ResultCoroutine()
    {
        ui.DisappearRanking();
        //BGMを流す
        AudioManager.I.PlayBGM("Ceremony");

        
        List<Player> players = PhotonNetwork.PlayerList.OrderBy(x => (int)x.CustomProperties["nowRank"]).ToList();
        //ネームプレートのテキストを1位プレイヤーの名前にあらかじめ変更
        ui.ChangeNameText(players[0].NickName);
        //ボールを作成
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Debug.Log("CreateBall");

            ResultBall nowBall = Instantiate(ballPrefab);
            nowBall.transform.SetParent(ballParents[i],false);
            nowBall.transform.position = nowBall.transform.parent.position;
            //ボールの名前をセット
            nowBall.ChangeNameText(players[i].NickName);
            //ボールの模様をセット
            MeshRenderer ren = nowBall.gameObject.GetComponent<MeshRenderer>();

            int patternCode = (int)players[i].CustomProperties["PatternCode"];
            int ColorCode = (int)players[i].CustomProperties["ColorCode"];


            ren.material.SetTexture("_MainTex", DatabaseManager.instance.patternDatas[patternCode].texture);
            ren.material.SetColor("_Color1", DatabaseManager.instance.colorDatas[ColorCode].colors[0]);
            ren.material.SetColor("_Color2", DatabaseManager.instance.colorDatas[ColorCode].colors[1]);
        }

        //カメラを動かす
        yield return ffnalResultCameraMovement.GetCameraSequence().WaitForCompletion();
        ui.AppearWinner();
        yield return new WaitForSeconds(2.0f);

        //動かし終わったらリザルトを表示
        ui.AppearRanking();
        ui.CreateTotalRankTag(PhotonNetwork.PlayerList);

        yield return new WaitForSeconds(5.0f);
        AudioManager.I.StopBGM();

        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }
}
