using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_Finish : MonoBehaviour
{
    [SerializeField] CanvasGroup finishCanvas;
    [SerializeField] S_AppearZoom finishZoom;

    public Sequence Appear()
    {
        return DOTween.Sequence()
                      .AppendCallback(() =>
                      {
                          AudioManager.I.PlaySE("FinishWhistle");
                          finishCanvas.alpha = 1;
                          finishCanvas.blocksRaycasts = true;
                      })
                      .Join(finishZoom.GetAppearZoomSequence())
                      .AppendInterval(1.5f);


    }

    public Sequence Disappear()
    {
        return DOTween.Sequence()
                      .AppendCallback(() =>
                      {
                          finishCanvas.alpha = 0;
                          finishCanvas.blocksRaycasts = false;
                      });


    }
}
