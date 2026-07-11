using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class S_AppearSlide : MonoBehaviour
{
    [SerializeField] Vector3 slideDistance;
    [SerializeField] float slideTime;
    [SerializeField] Ease slideEase;
    [SerializeField] bool isPositionSetting;

    public Sequence GetAppearSlideSequence()
    {
        return DOTween.Sequence()
                      .AppendCallback(() => {
                          if (!isPositionSetting)
                          {
                              transform.localPosition -= slideDistance;
                          }
                      })
                      .Append(transform.DOLocalMove(slideDistance, slideTime)
                                       .SetRelative()
                                       .SetEase(slideEase));

    }
}
