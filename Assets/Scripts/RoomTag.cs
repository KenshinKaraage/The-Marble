using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomTag : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text memberText;

    [SerializeField] Button joinButton;
    public void ChangeRoomNameText(string text)
    {
        roomNameText.text = text;
    }

    public void ChangeMemberText(int value)
    {
        memberText.text = string.Format("{0}/8", value);
    }

    public void AbleJoinButton(bool able)
    {
        joinButton.interactable = able;
    }

    public void OnPushed()
    {
        TitleManager.instance.JoinRoom(roomNameText.text);
    }
}
