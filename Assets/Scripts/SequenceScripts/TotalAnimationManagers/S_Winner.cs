using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_Winner : MonoBehaviour
{
    [SerializeField] S_AppearFadeIn winnerNameFadeIn;
    [SerializeField] S_AppearSlide winnerSlide;
    [SerializeField] GameObject particlePrefab;
    [SerializeField] Transform particleParent;
    public Sequence AppearWinner()
    {
        return DOTween.Sequence()
                      .Append(winnerNameFadeIn.GetAppearFadeInSequence())
                      .AppendInterval(0.5f)
                      .Append(winnerSlide.GetAppearSlideSequence())
                      .AppendCallback(() =>
                      {
                          foreach (Transform particleTransform in particleParent)
                          {
                              Instantiate(particlePrefab, particleTransform);
                          }
                      });
    }
}
