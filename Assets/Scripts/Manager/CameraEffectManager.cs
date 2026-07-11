using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Realtime;
using Photon.Pun;
using DG.Tweening;

public class CameraEffectManager : MonoBehaviourPunCallbacks
{
    public static CameraEffectManager instance;
    [SerializeField] Camera effectCamera;
    [SerializeField] PostProcessVolume postProcessVolume;
    PostProcessProfile postProcessProfile;
    [Header("Boost Effect")] 
    [SerializeField] float boostEffectStrength;
    [SerializeField] float boostEffectTime;
    [SerializeField] float boostEffectStayTime;
    [SerializeField] float boostEffectCalmTime;
    [Header("Blur Effect")]
    [SerializeField] float blurTime;

    float defaultFieldOfView;
    Vignette vignette;
    DepthOfField depthOfField;

    private void Awake()
    {
        instance = this;

        postProcessProfile = postProcessVolume.profile;

        vignette = postProcessProfile.GetSetting<Vignette>();
        depthOfField = postProcessProfile.GetSetting<DepthOfField>();

        defaultFieldOfView = effectCamera.fieldOfView;
    }

    public void BoostEffect()
    {
        if (effectCamera == null)
        {
            return;
        }

        DOTween.Sequence()
               .Append(DOTween.To(() => defaultFieldOfView,
                   (x) => { effectCamera.fieldOfView = x; },
                   defaultFieldOfView + boostEffectStrength,
                   boostEffectTime)
                   .SetEase(Ease.OutQuad))
               .Join(DOTween.To(() => 0f,
                   (x) => { vignette.intensity.Override(x); },
                   0.5f,
                   boostEffectTime)
                   .SetEase(Ease.OutQuad))
               .AppendInterval(boostEffectStayTime)
               .Append(DOTween.To(() => defaultFieldOfView + boostEffectStrength,
                   (x) => { effectCamera.fieldOfView = x; },
                   defaultFieldOfView,
                   boostEffectCalmTime)
                   .SetEase(Ease.Linear))
               .Join(DOTween.To(() => 0.5f,
                   (x) => { vignette.intensity.Override(x); },
                   0f,
                   boostEffectCalmTime)
                  .SetEase(Ease.Linear));

        Bloom bloom = ScriptableObject.CreateInstance<Bloom>();
        bloom.enabled.Override(true);
        bloom.intensity.Override(20f);
    }

    public void ResultBlur()
    {
        if (effectCamera == null)
        {
            return;
        }

        DOTween.To(() => 5.0f, (x) => { depthOfField.focusDistance.Override(x); }, 0.0f, blurTime);
    }
}
