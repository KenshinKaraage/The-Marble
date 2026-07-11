using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PlayerNewsTag : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text newsText;
    [SerializeField] float listTime;
    [SerializeField] float slideDistance;
    [SerializeField] float slideTime;

    float nowTime;

    private void Awake()
    {
        nowTime = 0;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime > listTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeTexts(string playerName, string news)
    {
        nameText.text = playerName;
        newsText.text = news;
    }

    public void SlideUp()
    {
        transform.DOLocalMoveY(slideDistance, slideTime)
                 .SetRelative();
    }
}
