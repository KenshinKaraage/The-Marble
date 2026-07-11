using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using DG.Tweening;

public class FinalResultManagerUI : MonoBehaviour
{
    [SerializeField] S_Winner s_winner;

    [SerializeField] TMP_Text nameText;

    [SerializeField] CanvasGroup resultRankingCanvasGroup;
    [SerializeField] RankTag totalRankTagPrefab;
    [SerializeField] Transform totalRankTagParent;

    [SerializeField] RankingBoard rankingBoard;
    public void DisappearRanking()
    {
        resultRankingCanvasGroup.alpha = 0;
        resultRankingCanvasGroup.blocksRaycasts = false;
    }

    public void AppearRanking()
    {
        rankingBoard.Appear();
    }

    public void CreateTotalRankTag(Player[] players)
    {
        rankingBoard.CreateRankTag(players, x => (int)x.CustomProperties["TotalScore"], RankTag.FormatMode.SCORE);
    }

    public void AppearWinner()
    {
        s_winner.AppearWinner();
    }

    public void ChangeNameText(string playerName)
    {
        nameText.text = playerName;
    }
}
