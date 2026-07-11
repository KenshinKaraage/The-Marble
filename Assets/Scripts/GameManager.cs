using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [SerializeField] GameManagerUI ui;
    private GameManagerTimer timer;
    private InputManager inputManager;
    private CameraControllerAfterGoal cameraAfterGoal;
    private CameraControllerManager cameraControllerManager;
    private CursorController cursorController;

    public enum GameMode
    {
        READY,
        PLAY,
        FINISH,
        CALCULATE,
        NEXT,
    }
    public GameMode gameMode;

    public float startTime;
    public bool isGoaled;
    public int numberOfClearedMember;

    ExitGames.Client.Photon.Hashtable hashtable;

    private void Awake()
    {
        instance = this;

        timer = GetComponent<GameManagerTimer>();

        inputManager = GetComponent<InputManager>();

        cursorController = GetComponent<CursorController>();

        cameraAfterGoal = Camera.main.GetComponent<CameraControllerAfterGoal>();

        cameraControllerManager = GetComponent<CameraControllerManager>();

        hashtable = new ExitGames.Client.Photon.Hashtable();
    }

    private void Start()
    {
        hashtable["Score"] = 0f;
        hashtable["nowRank"] = 0;
        hashtable["addScore"] = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties["TotalScore"] == null)
        {
            hashtable["TotalScore"] = 0;
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

        timer.Initialized(DatabaseManager.instance.GetNowSceneData().timeLimit);

        StartCoroutine(GameCoroutine());

    }

    IEnumerator GameCoroutine()
    {
        gameMode = GameMode.READY;
        inputManager.IsAttractive = false;
        cameraControllerManager.IsAttractive = false;

        cursorController.SetActive();
        isGoaled = false;
        numberOfClearedMember = 0;

        ui.SetTimeLimitActive(DatabaseManager.instance.GetNowSceneData().type == SceneData.SceneType.SCORE);
        ui.ChangeTimeLimitText(timer.Timer);
        ui.ChangeStatusWindow(DatabaseManager.instance.sceneDataList[(int)PhotonNetwork.CurrentRoom.CustomProperties["NowScene"]].type);
        if (DatabaseManager.instance.GetNowSceneData().type == SceneData.SceneType.SCORE)
        {
            ui.ChangeStatusText(0);
        }
        else
        {
            ui.ChangeStatusText(numberOfClearedMember, PhotonNetwork.CurrentRoom.PlayerCount);
        }
        yield return null;

        cameraControllerManager.IsAttractive = true;

        yield return ui.GetReadyAppearSequence().WaitForCompletion();
        ui.GetReadyDisappearSequence();

        gameMode = GameMode.PLAY;
        startTime = Time.time;

        AudioManager.I.PlayBGM(DatabaseManager.instance.sceneDataList[(int)PhotonNetwork.CurrentRoom.CustomProperties["NowScene"]].BGMName);
        inputManager.IsAttractive = true;
        timer.IsActive = true;

        int nowTime = (int)timer.Timer;
        while (numberOfClearedMember < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            ui.ChangeTimeLimitText(timer.Timer);

            if (timer.Timer < 0)
            {
                ClearForcely();
            }
            else if (timer.Timer < DatabaseManager.instance.GetNowSceneData().visualizeTime)
            {
                ui.SetTimeLimitActive(true);
                if (timer.Timer < 5f)
                {
                    ui.ChangeTimeLimitToEnd();
                    if (nowTime != (int)timer.Timer)
                    {
                        AudioManager.I.PlaySE("TimeLimitCountDown");
                    }   
                }
            }

            nowTime = (int)timer.Timer;

            yield return null;
        }

        gameMode = GameMode.FINISH;
        timer.IsActive = false;
        ui.SetTimeLimitActive(false);
        ui.DisappearPlay();
        inputManager.IsAttractive = false;


        CameraEffectManager.instance.ResultBlur();
        AudioManager.I.FadeOutBGM(1.0f);

        yield return ui.GetFinishAppearSequence().WaitForCompletion();
        yield return ui.GetFinishDisappearSequence().WaitForCompletion();

        AudioManager.I.PlayBGM("Result");

        if (DatabaseManager.instance.GetNowSceneData().type == SceneData.SceneType.SCORE)
        {
            Debug.Log(DatabaseManager.instance.GetNowSceneData().type == SceneData.SceneType.SCORE);

            ui.CreateScoreRankTag(PhotonNetwork.PlayerList);
        }
        else
        {
            Debug.Log(DatabaseManager.instance.GetNowSceneData().type == SceneData.SceneType.SCORE);
            ui.CreateTimeRankTag(PhotonNetwork.PlayerList);
        }
        ui.AppeaRankCanvas();


        gameMode = GameMode.CALCULATE;
        CalculateTotalScore();

        yield return new WaitForSeconds(3.0f);

        ui.CreateTotalRankTag(PhotonNetwork.PlayerList);
        ui.DisappearRankCanvas();

        ui.AppearTotalRankCanvas();

        yield return new WaitForSeconds(3.0f);
        AudioManager.I.StopBGM();

        gameMode = GameMode.FINISH;
        cursorController.SetNegative();

        if (PhotonNetwork.IsMasterClient)
        {
            DatabaseManager.instance.NowSceneNum++;
            int sceneIndex = DatabaseManager.instance.GetSceneRandomly();
            if (sceneIndex != -1)
            {
                hashtable["NowScene"] = sceneIndex;
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }

    void CalculateTotalScore()
    {
        hashtable["addScore"] = PhotonNetwork.CurrentRoom.PlayerCount - (int)(PhotonNetwork.LocalPlayer.CustomProperties["nowRank"]);
        hashtable["TotalScore"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["TotalScore"] + (PhotonNetwork.CurrentRoom.PlayerCount - (int)(PhotonNetwork.LocalPlayer.CustomProperties["nowRank"]));
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public void  WhenCleared(GameObject gameObject)
    {
        Debug.Log("Clear");

        if (!isGoaled)
        {
            isGoaled = true;

            switch (DatabaseManager.instance.GetNowSceneData().type)
            {
                case SceneData.SceneType.LACE:
                    hashtable["nowRank"] = numberOfClearedMember;
                    hashtable["Score"] = Time.time - startTime;
                    AudioManager.I.PlaySE("Goal");
                    break;
                case SceneData.SceneType.SURVIVAL:
                    hashtable["nowRank"] = (int)PhotonNetwork.CurrentRoom.PlayerCount - numberOfClearedMember - 1;
                    hashtable["Score"] = Time.time - startTime;
                    AudioManager.I.PlaySE("Eliminated");
                    break;
                case SceneData.SceneType.SCORE:
                    Debug.Log("Score");
                    hashtable["nowRank"] = GetRankFromScore();
                    break;
                default:
                    break;
            }
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

            if (gameObject != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            StartCoroutine(ClearCoroutine());

            //�j���[�X�^�O�����
            photonView.RPC(nameof(GenerateNewsTag), RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
    }

    [PunRPC] void GenerateNewsTag(Player player)
    {
        ui.GenerateNews(player);
    }

    void ClearForcely()
    {
        if (!isGoaled)
        {
            isGoaled = true;

            switch (DatabaseManager.instance.GetNowSceneData().type)
            {
                case SceneData.SceneType.LACE:
                    hashtable["nowRank"] = numberOfClearedMember;
                    hashtable["Score"] = Time.time - startTime;
                    break;
                case SceneData.SceneType.SURVIVAL:
                    hashtable["nowRank"] = (int)PhotonNetwork.CurrentRoom.PlayerCount - numberOfClearedMember - 1;
                    hashtable["Score"] = Time.time - startTime;
                    break;
                case SceneData.SceneType.SCORE:
                    Debug.Log("Score");
                    hashtable["nowRank"] = GetRankFromScore();
                    break;
                default:
                    break;
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

            photonView.RPC(nameof(AddClearedMember), RpcTarget.All);
        }
    }

    public int GetRankFromScore()
    {
        Player[] players = PhotonNetwork.PlayerList;
        players = players.OrderByDescending(x => (float)x.CustomProperties["Score"]).ToArray();
        int rank = players.Count(x => (float)PhotonNetwork.LocalPlayer.CustomProperties["Score"] < (float)x.CustomProperties["Score"]);
        return rank;
    }

    IEnumerator ClearCoroutine()
    {
        yield return ui.GetGoalAppearSequence().WaitForCompletion();
        ui.GetGoalDisappearSequence();
        photonView.RPC(nameof(AddClearedMember), RpcTarget.All);
        //�S�[����ɃJ�����𓮂�����悤�ɂ���
        cameraAfterGoal.IsActive = true;
        cameraAfterGoal.ResetPosition();
    }

    [PunRPC]
    public void AddClearedMember()
    {
        numberOfClearedMember++;
        if (DatabaseManager.instance.GetNowSceneData().type != SceneData.SceneType.SCORE)
        {
            ui.ChangeStatusText(numberOfClearedMember, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    public void AddPlayerScore(int value)
    {
        float nowScore = (float)PhotonNetwork.LocalPlayer.CustomProperties["Score"];
        ui.ChangeStatusText(nowScore + (float)value);

        hashtable["Score"] = nowScore + (float)value;

        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        //�܃|�C���g�ȏ�Ȃ�j���[�X�^�O��\��
        if (value >= 5)
        {
            //�j���[�X�^�O�����
            photonView.RPC(nameof(GenerateNewsTag), RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (gameMode == GameMode.FINISH)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (DatabaseManager.instance.NowSceneNum == DatabaseManager.instance.MaxPlaySceneNum)
                {
                    //�����Ȃ胊�U���g�V�[����
                    PhotonNetwork.LoadLevel("FinalResult");
                }
                else
                {
                    PhotonNetwork.LoadLevel("Interval");
                }
            }
        }
    }

    //�v���C���[���������Ƃ��΍�(�e�L�X�g��ς��邾��)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ui.ChangeStatusText(numberOfClearedMember, PhotonNetwork.CurrentRoom.PlayerCount);
    }
}
