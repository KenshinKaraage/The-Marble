using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField] TMP_Text topleftNameText;
    [SerializeField] TMP_Text clearedMemberText;
    [SerializeField] TMP_Text timeLimitText;
    [SerializeField] Image timeLimitImage;
    [SerializeField] List<Sprite> timeLimitSpriteList;

    /*
    [SerializeField] Transform rankTagParent;
    [SerializeField] RankTag rankTagPrefab;
    [SerializeField] Transform totalRankTagParent;
    [SerializeField] RankTag totalRankTagPrefab;
    */
    [SerializeField] RankingBoard rankBoard;
    [SerializeField] RankingBoard totalRankBoard;

    [SerializeField] CanvasGroup countdownCanvas;
    [SerializeField] CanvasGroup playCanvas;
    [SerializeField] CanvasGroup timeLimitCanvas;

    [SerializeField] S_Ready ready;
    [SerializeField] S_Goal goal;
    [SerializeField] S_Finish finish;

    [SerializeField] PlayerNewsTagList newsList;

    private void Start()
    {
        rankBoard.SetActiveFalse();
        totalRankBoard.SetActiveFalse();
    }

    public void ChangeStatusWindow(SceneData.SceneType type)
    {
        switch (type)
        {
            case SceneData.SceneType.LACE:
                topleftNameText.text = "GOAL";
                break;
            case SceneData.SceneType.SURVIVAL:
                topleftNameText.text = "ELIMINATED";
                break;
            case SceneData.SceneType.SCORE:
                topleftNameText.text = "SCORE";
                break;
            default:
                break;
        }
    }

    public void SetTimeLimitActive(bool active)
    {
        timeLimitCanvas.alpha = active ? 1 : 0;
        timeLimitImage.sprite = timeLimitSpriteList[0];
    }

    public void ChangeStatusText(int nowMember, int maxMember)
    {
        clearedMemberText.text = string.Format("{0}/{1}", nowMember, maxMember);
    }

    public void ChangeStatusText(float score)
    {
        int intScore = (int)score;
        clearedMemberText.text = string.Format("{0}", score);
    }

    public void ChangeTimeLimitText(float time)
    {
        timeLimitText.text = string.Format("{0:00}:{1:00}", (int)(time / 60), (int)(time % 60));

    }

    public void ChangeTimeLimitToEnd()
    {
        timeLimitImage.sprite = timeLimitSpriteList[1];
    }

    
    public void CreateTimeRankTag(Player[] players)
    {
        rankBoard.CreateRankTag(players, x => (float)x.CustomProperties["Score"], RankTag.FormatMode.TIME);
    }

    public void CreateScoreRankTag(Player[] players)
    {
        rankBoard.CreateRankTag(players, x => (float)x.CustomProperties["Score"], RankTag.FormatMode.SCORE);
    }

    public void CreateTotalRankTag(Player[] players)
    {
        totalRankBoard.CreateRankTag(players, x => (int)x.CustomProperties["TotalScore"], RankTag.FormatMode.SCORE, x => (int)x.CustomProperties["addScore"]);
    }

    public void AppeaRankCanvas()
    {
        rankBoard.Appear();
    }

    public void AppearTotalRankCanvas()
    {
        totalRankBoard.Appear();
    }

    public void DisappearRankCanvas()
    {
        rankBoard.Disappear();
    }

    public void DisappeaTotalRankCanvas()
    {
        totalRankBoard.Disappear();
    }

    public Sequence GetReadyAppearSequence()
    {
        return ready.Appear();
    }

    public Sequence GetReadyDisappearSequence()
    {
        return ready.Disappear();
    }

    public Sequence GetGoalAppearSequence()
    {
        return goal.Appear();
    }

    public Sequence GetGoalDisappearSequence()
    {
        return goal.Disappear();
    }

    public void AppearPlay()
    {
        playCanvas.alpha = 1;
    }

    public void DisappearPlay()
    {
        playCanvas.alpha = 0;
    }

    public Sequence GetFinishAppearSequence()
    {
        return finish.Appear();
    }

    public Sequence GetFinishDisappearSequence()
    {
        return finish.Disappear();
    }

    public void GenerateNews(Player player)
    {
        newsList.GeneratePlayerNewsTag(player.NickName);
    }
}
