using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class TitleManagerUI : MonoBehaviour
{
    [SerializeField] CanvasGroup titleScreenCanvasGroup;
    [SerializeField] CanvasGroup nameInputPanelCanvasGroup;
    [SerializeField] CanvasGroup menuScreenCanvasGroup;
    [SerializeField] CanvasGroup rankCanvasGroup;
    [SerializeField] CanvasGroup paintCanvasGroup;
    [SerializeField] CanvasGroup pauseCanvasGroup;
    [SerializeField] CanvasGroup roomListPanelCanvasGroup;
    [SerializeField] CanvasGroup roomNameInputPanelCanvasGroup;
    [SerializeField] CanvasGroup roomPanelCanvasGroup;

    [SerializeField] Transform patternWindownTransform;
    [SerializeField] Transform colorWindownTransform;

    [SerializeField] TMP_Text roomNameText;

    [SerializeField] TMP_Text nameTagText;

    [SerializeField] PlayerTag playerTagPrefab;
    [SerializeField] Transform playerTagContentTransform;

    [SerializeField] RoomTag roomTagPrefab;
    [SerializeField] Transform roomTagContentTransform;

    List<PlayerTag> playerTags = new List<PlayerTag>();

    [SerializeField] S_Title s_title;
    public void ShowTitleScreen()
    {
        titleScreenCanvasGroup.alpha = 1;
        titleScreenCanvasGroup.blocksRaycasts = true;

        s_title.TitleAppear();
    }

    public void FadeOutTitleScreen()
    {
        titleScreenCanvasGroup.alpha = 0;
        titleScreenCanvasGroup.interactable = false;
    }

    public void CloseAllScreen()
    {
        titleScreenCanvasGroup.alpha = 0;
        titleScreenCanvasGroup.blocksRaycasts = false;

        paintCanvasGroup.alpha = 0;
        paintCanvasGroup.blocksRaycasts = false;

        rankCanvasGroup.alpha = 0;
        rankCanvasGroup.blocksRaycasts = false;

        pauseCanvasGroup.alpha = 0;
        pauseCanvasGroup.blocksRaycasts = false;

        nameInputPanelCanvasGroup.alpha = 0;
        nameInputPanelCanvasGroup.blocksRaycasts = false;

        roomListPanelCanvasGroup.alpha = 0;
        roomListPanelCanvasGroup.blocksRaycasts = false;

        roomNameInputPanelCanvasGroup.alpha = 0;
        roomNameInputPanelCanvasGroup.blocksRaycasts = false;

        roomPanelCanvasGroup.alpha = 0;
        roomPanelCanvasGroup.blocksRaycasts = false;

        menuScreenCanvasGroup.alpha = 0;
        menuScreenCanvasGroup.blocksRaycasts = false;
    }

    public void ShowNameInputPanel()
    {
        nameInputPanelCanvasGroup.alpha = 1;
        nameInputPanelCanvasGroup.blocksRaycasts = true;
    }

    public void ShowMenuScreen()
    {
        menuScreenCanvasGroup.alpha = 1;
        menuScreenCanvasGroup.blocksRaycasts = true;

        if (!s_title.ShowingMenu)
        {
            s_title.MenuAppear();
        }
    }

    public void ShowRankScreen()
    {
        rankCanvasGroup.alpha = 1;
        rankCanvasGroup.blocksRaycasts = true;
    }

    public void ShowPaintScreen()
    {
        paintCanvasGroup.alpha = 1;
        paintCanvasGroup.blocksRaycasts = true;
    }

    public void PushPatternWindow()
    {
        colorWindownTransform.SetAsFirstSibling();
    }

    public void PushColorWindow()
    {
        patternWindownTransform.SetAsFirstSibling();
    }

    public void ShowPausePanel()
    {
        pauseCanvasGroup.alpha = 1;
        pauseCanvasGroup.blocksRaycasts = true;
    }

    public void ChangeNameTagText()
    {
        nameTagText.text = PlayerPrefs.GetString("name");
    }

    public void ShowRoomListPanel()
    {
        roomListPanelCanvasGroup.alpha = 1;
        roomListPanelCanvasGroup.blocksRaycasts = true;
    }

    public void ShowRoomNameInputPanel()
    {
        roomNameInputPanelCanvasGroup.alpha = 1;
        roomNameInputPanelCanvasGroup.blocksRaycasts = true;
    }

    public void ShowRoomPanel()
    {
        roomPanelCanvasGroup.alpha = 1;
        roomPanelCanvasGroup.blocksRaycasts = true;
    }

    public void ChangeRoomNameText(string text)
    {
        roomNameText.text = text;
    }

    public void GenerateNameTag(Player player)
    {
        PlayerTag playerTag = Instantiate(playerTagPrefab);
        playerTag.transform.SetParent(playerTagContentTransform, false);
        playerTag.ChangeNameText(player.NickName);
        playerTag.player = player;

        if(player.CustomProperties["IsReady"] == null)
        {
            Debug.Log("null");
        }
        else
        {
            if ((bool)player.CustomProperties["IsReady"])
            {
                playerTag.SetFlag(true);
            }
        }
        
        playerTags.Add(playerTag);
    }

    public void InitializeNameTags()
    {
        playerTags.Clear();

        foreach (Transform tagOb in playerTagContentTransform)
        {
            Destroy(tagOb.gameObject);
        }
    }

    public void GenerateRoomTag(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            RoomTag roomTag = Instantiate(roomTagPrefab);
            roomTag.transform.SetParent(roomTagContentTransform, false);
            roomTag.ChangeRoomNameText(room.Name);
            roomTag.ChangeMemberText(room.PlayerCount);

            if (room.PlayerCount >= 8)
            {
                roomTag.AbleJoinButton(false);
            }
            else
            {
                roomTag.AbleJoinButton(true);
            }
        }
    }

    public void InitializeRoomTags()
    {
        foreach (Transform tagOb in roomTagContentTransform)
        {
            Destroy(tagOb.gameObject);
        }
    }

    public void ChangeFlag(Player player)
    {
        foreach (PlayerTag tag in playerTags)
        {
            if (tag.player == player)
            {
                tag.SetFlag((bool)player.CustomProperties["IsReady"]);
            }
        }
    }
}
