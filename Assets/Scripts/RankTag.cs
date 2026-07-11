using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RankTag : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text addScoreText;

    public enum FormatMode
    {
        TIME,
        SCORE,
    }

    public void ChangeNameText(string str)
    {
        nameText.text = str;
    }

    public void ChangeScoreText(float score, FormatMode mode)
    {
        switch (mode)
        {
            case FormatMode.TIME:
                scoreText.text = string.Format("{0:00}:{1:00}:{2:00}", (int)(score / 60), (int)(score % 60), (int)((score * 100f) % 100));
                break;
            case FormatMode.SCORE:
                scoreText.text = string.Format("{0}", (int)score);
                break;
            default:
                break;
        }
    }

    public void ChangeAddScoreText(int score)
    {
        if (addScoreText != null)
        {
            addScoreText.text = string.Format("+{0}", score);
        }
    }

    public void DisappearAddScoreText()
    {
        if (addScoreText != null)
        {
            addScoreText.text = "";
        }
    }
}
