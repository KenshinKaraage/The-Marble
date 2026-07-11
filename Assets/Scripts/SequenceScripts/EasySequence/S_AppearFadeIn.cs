using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_AppearFadeIn : MonoBehaviour
{
    [SerializeField] Vector3 slideDistance;
    [SerializeField] float slideTime;
    [SerializeField] Ease slideEase;

    CanvasGroup canvasGroup;

    private void Awake()
    {        
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public Sequence GetAppearFadeInSequence()
    {
        return DOTween.Sequence()
                      .AppendCallback(() => {
                          transform.localPosition -= slideDistance;
                          canvasGroup.alpha = 0;
                      })
                      .Append(transform.DOLocalMove(slideDistance, slideTime)
                                       .SetRelative()
                                       .SetEase(slideEase))
                      .Join(canvasGroup.DOFade(1, slideTime)
                                       .SetEase(slideEase));
    }

    public Sequence GetDisappearFadeInSequence()
    {
        return DOTween.Sequence()
                      .AppendCallback(() => {
                          canvasGroup.alpha = 1;
                      })
                      .Append(transform.DOLocalMove(slideDistance, slideTime)
                                       .SetRelative()
                                       .SetEase(slideEase))
                      .Join(canvasGroup.DOFade(0, slideTime)
                                       .SetEase(slideEase));
    }
}
