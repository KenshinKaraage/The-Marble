using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Realtime;
using Photon.Pun;
public class RankingBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] RankTag rankTagPrefab;
    [SerializeField] Transform rankTagParent;

    CanvasGroup canvasGroup;
    S_AppearFadeIn rankBoardAppear;

    List<RankTag> totalRankTags = new List<RankTag>();

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        rankBoardAppear = GetComponent<S_AppearFadeIn>();
    }

    public void CreateRankTag(Player[] players, Func<Player, float> func, RankTag.FormatMode mode)
    {
        this.CreateRankTag(players, func, mode, null);
    }

    public void CreateRankTag(Player[] players, Func<Player, float> func, RankTag.FormatMode mode, Func<Player, int> additionalFunc)
    {
        List<Player> nowPlayerList = players.OrderBy(player => (int)player.CustomProperties["nowRank"]).ToList<Player>();
        foreach (Player player in nowPlayerList)
        {
            RankTag nowRankTag = Instantiate(rankTagPrefab, rankTagParent);
            totalRankTags.Add(nowRankTag);
            nowRankTag.ChangeNameText(player.NickName);
            nowRankTag.ChangeScoreText(func(player), mode);

            if (additionalFunc != null)
            {
                nowRankTag.ChangeAddScoreText(additionalFunc(player));
            }
            else
            {
                nowRankTag.DisappearAddScoreText();
            }

            Debug.Log("2:TotalScore = " + func(player));
        }
    }

    public void CreateRankTag(Player[] players, Func<Player, int> func, RankTag.FormatMode mode, Func<Player, int> additionalFunc)
    {
        List<Player> nowPlayerList = players.OrderBy(player => (int)player.CustomProperties["nowRank"]).ToList<Player>();
        foreach (Player player in nowPlayerList)
        {
            RankTag nowRankTag = Instantiate(rankTagPrefab, rankTagParent);
            totalRankTags.Add(nowRankTag);
            nowRankTag.ChangeNameText(player.NickName);
            nowRankTag.ChangeScoreText((float)func(player), mode);


            if (additionalFunc != null)
            {
                nowRankTag.ChangeAddScoreText(additionalFunc(player));
            }
            else
            {
                nowRankTag.DisappearAddScoreText();
            }

            Debug.Log("2:TotalScore = " + func(player));
        }
    }

    public void SetActiveFalse()
    {
        canvasGroup.alpha = 0;
    }

    public void Disappear()
    {
        rankBoardAppear.GetDisappearFadeInSequence();
    }

    public void Appear()
    {
        canvasGroup.alpha = 1;
        rankBoardAppear.GetAppearFadeInSequence();
    }
}
