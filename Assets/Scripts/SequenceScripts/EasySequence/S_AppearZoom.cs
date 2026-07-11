using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class S_AppearZoom : MonoBehaviour
{
    [SerializeField] float firstSize;
    [SerializeField] float zoomTime;
    [SerializeField] Ease zoomEase;
    Vector3 afterSize;

    private void Awake()
    {
        afterSize = transform.localScale;
    }

    public Sequence GetAppearZoomSequence()
    {
        return DOTween.Sequence()
                      .AppendCallback(() => {
                          transform.localScale = afterSize * firstSize;
                      })
                      .Append(transform.DOScale(afterSize, zoomTime)
                                       .SetEase(zoomEase));
    }

    public Sequence GetDisappearZoomSequence()
    {
        return DOTween.Sequence()
                      .Append(transform.DOScale(Vector3.zero, zoomTime)
                                       .SetEase(zoomEase));

    }
}
