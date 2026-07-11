using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class MarbleMaterialManager : MonoBehaviour
{
    [SerializeField] Material material;

    [SerializeField] TMP_Text patternNameText;
    [SerializeField] TMP_Text colorNameText;

    [SerializeField] PatternTag patternTagPrefab;
    [SerializeField] ColorTag colorTagPrefab;
    [SerializeField] Transform patternTagParent;
    [SerializeField] Transform colorTagParent;

    List<PatternTag> patternTags = new List<PatternTag>();
    List<ColorTag> colorTags = new List<ColorTag>();


    ExitGames.Client.Photon.Hashtable hashtable;

    private void Awake()
    {
        hashtable = new ExitGames.Client.Photon.Hashtable();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("PatternCode"))
        {
            DatabaseManager.instance.nowPatternCode = PlayerPrefs.GetInt("PatternCode");
            material.SetTexture("_MainTex", DatabaseManager.instance.GetPatternData().texture);
        }
        else
        {
            material.SetTexture("_MainTex", DatabaseManager.instance.patternDatas[0].texture);
        }

        if (PlayerPrefs.HasKey("ColorCode"))
        {
            DatabaseManager.instance.nowColorCode = PlayerPrefs.GetInt("ColorCode");
            for (int i = 0; i < 2; i++)
            {
                material.SetColor("_Color" + (i + 1), DatabaseManager.instance.GetColorData().colors[i]);
                Debug.Log(DatabaseManager.instance.GetColorData().colors[i]);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                material.SetColor("_Color" + (i + 1), DatabaseManager.instance.colorDatas[0].colors[i]);
            }
        }
    }

    public void ChangeMaterialPattern(PatternData data)
    {
        AudioManager.I.PlaySE("Select");

        material.SetTexture("_MainTex", data.texture);

        DatabaseManager.instance.nowPatternCode = data.ID;

        PlayerPrefs.SetInt("PatternCode", data.ID);
        PlayerPrefs.Save();

        WhenPatternChanged();
        ChangePatternText(data);
    }

    public void ChangeMaterialColor(ColorData data)
    {
        AudioManager.I.PlaySE("Select");

        for (int i = 0; i < 2; i++)
        {
            material.SetColor("_Color" + (i + 1), data.colors[i]);
        }

        DatabaseManager.instance.nowColorCode = data.ID;

        PlayerPrefs.SetInt("ColorCode", data.ID);
        PlayerPrefs.Save();

        WhenColorChanged();
        ChangeColorText(data);
    }

    public void ChangePatternText(PatternData data)
    {
        patternNameText.text = data.patternName;
    }

    public void ChangeColorText(ColorData data)
    {
        colorNameText.text = data.colorName;
    }

    public void ResetTags()
    {
        foreach (Transform item in patternTagParent)
        {
            Destroy(item.gameObject);
        }

        patternTags.Clear();

        foreach (Transform item in colorTagParent)
        {
            Destroy(item.gameObject);
        }

        colorTags.Clear();
    }

    public void GeneratePatternTags()
    {
        for (int i = 0; i < DatabaseManager.instance.patternDatas.Count; i++)
        {
            if (DatabaseManager.instance.havePattern[i])
            {
                PatternTag nowTag = Instantiate(patternTagPrefab);
                nowTag.transform.SetParent(patternTagParent, false);
                nowTag.SetData(DatabaseManager.instance.patternDatas[i]);
                nowTag.MaterialManager = this;
                patternTags.Add(nowTag);
            }
        }

        ChangePatternText(DatabaseManager.instance.GetPatternData());
        WhenPatternChanged();
    }

    public void GenerateColorTags()
    {
        for (int i = 0; i < DatabaseManager.instance.colorDatas.Count; i++)
        {
            if (DatabaseManager.instance.haveColor[i])
            {
                ColorTag nowTag = Instantiate(colorTagPrefab);
                nowTag.transform.SetParent(colorTagParent, false);
                nowTag.SetData(DatabaseManager.instance.colorDatas[i]);
                nowTag.MaterialManager = this;
                colorTags.Add(nowTag);
            }
        }

        ChangeColorText(DatabaseManager.instance.GetColorData());
        WhenColorChanged();
    }

    void WhenPatternChanged()
    {
        for (int i = 0; i < patternTags.Count; i++)
        {
            patternTags[i].SetCheckObActive(DatabaseManager.instance.nowPatternCode == i);
        }
    }

    void WhenColorChanged()
    {
        for (int i = 0; i < colorTags.Count; i++)
        {
            colorTags[i].SetCheckObActive(DatabaseManager.instance.nowColorCode == i);
        }
    }
}
