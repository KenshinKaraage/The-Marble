using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class TitleManager : MonoBehaviourPunCallbacks
{
    static public TitleManager instance;

    TitleManagerUI ui;

    [SerializeField] MarbleMaterialManager materialManager;
    [SerializeField] TitleMarble titleMarble;

    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField roomNameInputField;
    private bool isSearchingRoom;
    private bool isGoingToPlay;

    private PhotonView m_photonView;

    ExitGames.Client.Photon.Hashtable hashtable;

    private void Awake()
    {
        //static変数に格納
        instance = this;
        //uiに格納
        ui = GetComponent<TitleManagerUI>();
        //photonViewを設定
        m_photonView = GetComponent<PhotonView>();

        hashtable = new ExitGames.Client.Photon.Hashtable();
    }

    private void Start()
    {
        isSearchingRoom = false;

        hashtable["IsReady"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

        if (!PhotonNetwork.IsConnected)
        {
            //タイトル画面
            AudioManager.I.PlayBGM("Title");

            ui.CloseAllScreen();
            ui.ShowTitleScreen();
        }
        else
        {
            ui.CloseAllScreen();
            ui.ShowMenuScreen();
        }
    }

    public void WhenStartButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        //ネットにつなげる
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ui.FadeOutTitleScreen();

        // マスターと一緒に移動するように設定
        PhotonNetwork.AutomaticallySyncScene = true;

        //名前がもし登録されていなかったら、登録するようにする
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("name")))
        {
            AudioManager.I.PlayBGM("Menu");
            ui.ShowNameInputPanel();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if (!isSearchingRoom)
        {
            ShowMenu();
        }
    }

    public void WhenNameSubmitButtonPushed()
    {
        AudioManager.I.PlaySE("Select");
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            Debug.Log("名前を入力してください");
            return;
        }

        PlayerPrefs.SetString("name", nameInputField.text);
        PlayerPrefs.Save();

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            //まず、PlayerPrefsから名前を取得し、それをニックネームにする
            PhotonNetwork.NickName = PlayerPrefs.GetString("name");

            ui.ChangeNameTagText();
            ui.CloseAllScreen();
            ui.ShowMenuScreen();
        }
    }

    private void ShowMenu()
    {
        AudioManager.I.PlayBGM("Menu");

        titleMarble.Active = true;

        isSearchingRoom = false;

        //まず、PlayerPrefsから名前を取得し、それをニックネームにする
        PhotonNetwork.NickName = PlayerPrefs.GetString("name");

        ui.ChangeNameTagText();
        ui.CloseAllScreen();
        ui.ShowMenuScreen();
    }

    public void WhenPauseButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        ui.ShowPausePanel();
    }

    public void WhenPauseCloseBUttonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        ui.CloseAllScreen();
        ui.ShowMenuScreen();
    }

    public void WhenNamePlatePushed()
    {
        ui.CloseAllScreen();
        ui.ShowNameInputPanel();
    }

    public void WhenRankPushed()
    {
        AudioManager.I.PlaySE("Select");

        ui.CloseAllScreen();
        ui.ShowRankScreen();
    }

    public void WhenRankCloseButtonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        ui.CloseAllScreen();
        ui.ShowMenuScreen();
    }

    public void WhenPaintButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        ui.CloseAllScreen();
        ui.ShowPaintScreen();

        materialManager.ResetTags();
        materialManager.GeneratePatternTags();
        materialManager.GenerateColorTags();
    }

    public void WhenPatternTagPushed()
    {
        ui.PushPatternWindow();
    }

    public void WhenColorTagPushed()
    {
        ui.PushColorWindow();
    }

    public void WhenPaintCloseButtonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        ui.CloseAllScreen();
        ui.ShowMenuScreen();
    }

    public void WhenPlayButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        titleMarble.Active = false;

        isSearchingRoom = true;

        ui.CloseAllScreen();
        ui.ShowRoomListPanel();
    }

    public void WhenCreateRoomButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        ui.CloseAllScreen();
        ui.ShowRoomNameInputPanel();
    }

    public void WhenRoomNameQuitButtonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        WhenPlayButtonPushed();
    }

    public void WhenRoomNameDecideButtonPushed()
    {
        AudioManager.I.PlaySE("Select");

        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.Log("名前を入力してださい");
            return;
        }

        ui.CloseAllScreen();

        //最大八人まで遊べる
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;

        PhotonNetwork.CreateRoom(roomNameInputField.text, options);
        
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        //部屋も入れるようにする
        hashtable["NowPlaying"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    public override void OnJoinedRoom()
    {
        //isGoingToPlayを初期化
        isGoingToPlay = false;
        hashtable["IsReady"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

        ui.ChangeRoomNameText(PhotonNetwork.CurrentRoom.Name);
        ui.ShowRoomPanel();
        GetAllPlayer();
    }

    //ルームにいるプレイヤー情報を取得する
    private void GetAllPlayer()
    {
        //名前リストを初期化
        ui.InitializeNameTags();
        
        //名前リストを作成
        GeneratePlayersTag();
    }

    private void GeneratePlayersTag()
    {
        //プレイヤータグを作る
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            ui.GenerateNameTag(player);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ui.GenerateNameTag(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //ルームリスト初期化
        ui.InitializeRoomTags();

        List<RoomInfo> newRoomList = new List<RoomInfo>();
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.PlayerCount != 0)
            {
                newRoomList.Add(roomInfo);
            }
        }

        ui.GenerateRoomTag(newRoomList);

    }

    public void WhenRoomQuitButtonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        if (!isGoingToPlay)
        {
            ui.CloseAllScreen();
            ui.ShowRoomListPanel();
        }
    }

    public void WhenRoomListQuitButtonPushed()
    {
        AudioManager.I.PlaySE("Cancel");

        titleMarble.Active = true;

        ui.CloseAllScreen();
        ui.ShowMenuScreen();
    }

    public void WhenGoButtonPushed()
    {
        if (isGoingToPlay)
        {
            return;
        }

        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["IsReady"])
        {
            hashtable["IsReady"] = false;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        else
        {
            hashtable["IsReady"] = true;

            hashtable["TotalScore"] = 0;
            //finalresultのマーブル処理を楽にするために、CustomPropertyを使う
            hashtable["PatternCode"] = DatabaseManager.instance.nowPatternCode;
            hashtable["ColorCode"] = DatabaseManager.instance.nowColorCode;
            //
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
    }

    //プレイヤープロパティーが変化したら
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        ui.ChangeFlag(targetPlayer);
        if ((bool)targetPlayer.CustomProperties["IsReady"])
        {
            AudioManager.I.PlaySE("Select");
        }
        SetReady();
    }

    //ルームにいるプレイヤー全員に発動
    public void SetReady()
    {
        //すべてのプレイヤーが準備完了か確認
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["IsReady"])
            {
                Debug.Log("playerName" + player.NickName);
                return;
            }
        }

        //全員準備完了
        isGoingToPlay = true;
        //マスターならシーンに移動する命令を行う
        if (PhotonNetwork.IsMasterClient)
        {
            //TODO:NowSceneをランダムに決める。
            DatabaseManager.instance.ResetGoneList();
            DatabaseManager.instance.NowSceneNum = 0;
            int nowSceneIndex = DatabaseManager.instance.GetSceneRandomly();
            hashtable["NowScene"] = nowSceneIndex;
            //ゲーム中であることも伝える
            hashtable["NowPlaying"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (isGoingToPlay)
        {
            StartCoroutine(GoCoroutine());
        }
    }

    IEnumerator GoCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        AudioManager.I.PlaySE("Go");
        AudioManager.I.FadeOutBGM(0.5f);

        yield return new WaitForSeconds(1.5f);

        if (PhotonNetwork.IsMasterClient)
        {
            //マスターならシーンに移動する命令を行う
            PhotonNetwork.LoadLevel("Interval");
        }
    }
}
