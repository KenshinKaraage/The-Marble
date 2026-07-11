using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_Ready : MonoBehaviour
{
    [SerializeField] List<GameObject> numberList = new List<GameObject>();
    [SerializeField] GameObject goOb;

    List<S_AppearZoom> numberZoomList = new List<S_AppearZoom>();
    S_AppearZoom goZoom;

    private void Awake()
    {
        for (int i = 0; i < numberList.Count; i++)
        {
            numberZoomList.Add(numberList[i].GetComponent<S_AppearZoom>());
        }

        goZoom = goOb.GetComponent<S_AppearZoom>();
    }

    public Sequence Appear()
    {
        return DOTween.Sequence()
                      .AppendCallback(() =>
                      {
                          foreach (GameObject numberOb in numberList)
                          {
                              goOb.SetActive(false);
                              numberOb.SetActive(false);
                          }
                      })
                      .AppendCallback(() =>
                      {
                          AudioManager.I.PlaySE("Countdown");

                          numberList[2].SetActive(true);
                      })
                      .AppendInterval(1.0f)
                      .Join(numberZoomList[2].GetAppearZoomSequence())
                      .AppendCallback(() =>
                      {
                          AudioManager.I.PlaySE("Countdown");
                          numberList[1].SetActive(true);
                      })
                      .AppendInterval(1.0f)
                      .Join(numberZoomList[1].GetAppearZoomSequence())
                      .Join(numberZoomList[2].GetDisappearZoomSequence())
                      .AppendCallback(() =>
                      {
                          AudioManager.I.PlaySE("Countdown");

                          numberList[1].SetActive(false);
                          numberList[0].SetActive(true);
                      })
                      .AppendInterval(1.0f)
                      .Join(numberZoomList[0].GetAppearZoomSequence())
                      .Join(numberZoomList[1].GetDisappearZoomSequence())
                      .AppendCallback(() =>
                      {
                          AudioManager.I.PlaySE("Whistle");

                          numberList[0].SetActive(false);
                          goOb.SetActive(true);
                      })
                      .Join(goZoom.GetAppearZoomSequence())
                      .Join(numberZoomList[0].GetDisappearZoomSequence());


    }

    public Sequence Disappear()
    {
        return DOTween.Sequence()
                      .AppendInterval(1.0f)
                      .AppendCallback(() =>
                      {
                          goOb.SetActive(false);
                      });


    }
}
