using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UniRx;
[System.Serializable]
class Sound
{
    public string name;
    public AudioClip clip;
    public bool isLooping;
}

public class AudioManager : DontDestroySingleton<AudioManager>
{
    [SerializeField] List<Sound> BGM = new List<Sound>();
    [SerializeField] List<Sound> SE = new List<Sound>();
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float fadeInValue;
    bool isFadeOut;
    bool isFadeIn;
    float fadeValue;
    float fadeTime;
    float fadeTimer;

    int nowPlayingBGMCode;

    [SerializeField] float defaultBGMVolume;
    [SerializeField] float defaultSEVolume;

    public float BGMVolume { set; get; }
    public float SEVolume { set; get; }

    public bool isPlaying()
    {
        return BGMSource.volume != 0;
    }
    

    private new void Awake()
    {
        base.Awake();
        BGMVolume = defaultBGMVolume;
        SEVolume = defaultSEVolume;
    }

    private void Start()
    {
        Observable.EveryUpdate()
                  .Subscribe(_ => UpdateMethod());
    }

    void UpdateMethod()
    {
        if (isFadeOut)
        {
            fadeTimer += Time.deltaTime;
            if(fadeTimer > fadeTime)
            {
                fadeTimer = fadeTime;
                isFadeOut = false;
            }
            BGMSource.volume = 1 - (fadeTimer / fadeTime) * (1 - fadeValue);
        }

        if (isFadeIn) {
            fadeTimer += Time.deltaTime;
            if (fadeTimer > fadeTime)
            {
                fadeTimer = fadeTime;
                isFadeIn = false;
            }
            BGMSource.volume = fadeValue + (fadeTimer / fadeTime) * (1 - fadeValue);
        }
    }

    public bool ChangeBGMClip(string name)
    {
        if (!BGM.Exists(x => x.name == name))
        {
            Debug.Log("BGM–¼‚ª‘¶Ý‚µ‚Ü‚¹‚ñ");
            return false;
        }
        Sound sound = BGM.FirstOrDefault(x => x.name == name);
        BGMSource.clip = sound.clip;
        return true;
    }

    public int GetBGMCodeFromName(string name)
    {
        if (!BGM.Exists(x => x.name == name))
        {
            Debug.Log("BGM–¼‚ª‘¶Ý‚µ‚Ü‚¹‚ñ");
            return -1;
        }

        Sound sound = BGM.FirstOrDefault(x => x.name == name);
        return BGM.IndexOf(sound);
    }

    public void PlayBGM(string name)
    {
        BGMSource.volume = 1;
        int code = GetBGMCodeFromName(name);
        PlayBGM(code);
    }

    public void PlayBGM(int code)
    {
        if (code < 0 || code >= BGM.Count) return;
        BGMSource.loop = BGM[code].isLooping;
        BGMSource.clip = BGM[code].clip;
        BGMSource.Play();
    }

    public void PauseBGM()
    {
        BGMSource.Pause();
    }

    public void FadeInBGM(float time)
    {
        if (BGMSource.volume > 0.0f) return;

        isFadeIn = true;
        fadeValue = fadeInValue;
        fadeTimer = 0;
        fadeTime = time;
    }

    public void FadeBGM(float time)
    {
        isFadeOut = true;
        fadeValue = fadeInValue;
        fadeTimer = 0;
        fadeTime = time;
    }

    public void FadeOutBGM(float time)
    {
        if (BGMSource.volume < 1.0f) return;

        isFadeOut = true;
        fadeValue = 0;
        fadeTimer = 0;
        fadeTime = time;
    }
    
    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void ChangeBGMValue(float value)
    {
        audioMixer.SetFloat("BGM", value);
    }

    public void PlaySE(string name)
    {
        if (!SE.Exists(x => x.name == name)) return;
        Sound sound = SE.FirstOrDefault(x => x.name == name);
        SESource.PlayOneShot(sound.clip);
    }
    public void PlaySE(int code)
    {
        if (code < -1 || code >= SE.Count) return;
        SESource.clip = SE[code].clip;
        SESource.Play();
    }

    public void ChangeSEValue(float value)
    {
        audioMixer.SetFloat("SE", value);
    }
}
