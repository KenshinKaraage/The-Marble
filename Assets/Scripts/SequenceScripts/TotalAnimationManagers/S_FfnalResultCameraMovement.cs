using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class S_FfnalResultCameraMovement : MonoBehaviour
{
    [SerializeField] Vector3 rotateValue;
    [SerializeField] float moveValue;
    [SerializeField] float cameraMoveTime;
    [SerializeField] Ease moveEase;
    [SerializeField] GameObject cameraOb;
    [SerializeField] float cameraFieldOfView;
    [SerializeField] float cameraFieldTime;
    Camera finalResultCamera;

    private void Awake()
    {
        finalResultCamera = cameraOb.GetComponent<Camera>();
    }

    public Sequence GetCameraSequence()
    {
        return DOTween.Sequence()
                      .Append(transform.DORotate(rotateValue, cameraMoveTime)
                                       .SetRelative()
                                       .SetEase(moveEase))
                      .Join(cameraOb.transform.DOMoveY(moveValue, cameraMoveTime)
                                              .SetRelative()
                                              .SetEase(moveEase))
                      .Append(DOTween.To(()=> finalResultCamera.fieldOfView, (x) => finalResultCamera.fieldOfView = x, cameraFieldOfView, cameraFieldTime))
                      .AppendInterval(0.75f);
    }
}
