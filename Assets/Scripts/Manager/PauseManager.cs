using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UniRx;
public class PauseManager : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    private void Start()
    {
        BGMSlider.value = AudioManager.I.BGMVolume;
        SESlider.value = AudioManager.I.SEVolume;

        BGMSlider.OnValueChangedAsObservable()
                 .Subscribe(x => WhenBGMValueChanged(x))
                 .AddTo(this.gameObject);

        SESlider.OnValueChangedAsObservable()
                 .Subscribe(x => WhenSEValueChanged(x))
                 .AddTo(this.gameObject);
    }

    void WhenBGMValueChanged(float x)
    {
        AudioManager.I.BGMVolume = x;
        x /= 5;
        x = Mathf.Clamp(Mathf.Log10(x) * 20f, -80f, 0f);
        AudioManager.I.ChangeBGMValue(x);
    }

    void WhenSEValueChanged(float x)
    {
        AudioManager.I.SEVolume = x;
        x /= 5;
        x = Mathf.Clamp(Mathf.Log10(x) * 20f, -80f, 0f);
        AudioManager.I.ChangeSEValue(x);
    }
}
