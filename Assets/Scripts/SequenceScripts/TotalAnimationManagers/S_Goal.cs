using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class S_Goal : MonoBehaviour
{
    [SerializeField] CanvasGroup goalCanvas;
    [SerializeField] S_AppearFadeIn goalFadeIn;
    public Sequence Appear()
    {
        return DOTween.Sequence()
                      .AppendCallback(() => {
                          goalCanvas.alpha = 1;
                          goalCanvas.blocksRaycasts = true;
                      })
                      .Join(goalFadeIn.GetAppearFadeInSequence())
                      .AppendInterval(2f);
    }

    public Sequence Disappear()
    {
        return DOTween.Sequence()
                      .Append(goalFadeIn.GetDisappearFadeInSequence())
                      .AppendCallback(() =>
                      {
                          goalCanvas.alpha = 0;
                          goalCanvas.blocksRaycasts = false;
                      });

    }
}
