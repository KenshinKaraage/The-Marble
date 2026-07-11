using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class IntervalManagerUI : MonoBehaviour
{
    [SerializeField] Image backImage;
    [SerializeField] Sprite laceTagSprite;
    [SerializeField] Sprite survivalTagSprite;
    [SerializeField] Sprite scoreTagSprite;
    [SerializeField] Image stageTagImage;
    [SerializeField] List<TMP_Text> stageNameList;
    [SerializeField] TMP_Text description;

    [SerializeField] S_Interval s_Interval;
    public void ChangeIntervalUI(SceneData data)
    {
        ChangeImageAndTexts(data);
        ChangeStageTagSprite(data);
    }

    void ChangeImageAndTexts(SceneData data)
    {
        foreach (TMP_Text stageName in stageNameList)
        {
            backImage.sprite = data.sprite;
            stageName.text = data.stageName;
            description.text = data.description;
        }
    }

    void ChangeStageTagSprite(SceneData data)
    {
        switch (data.type)
        {
            case SceneData.SceneType.LACE:
                stageTagImage.sprite = laceTagSprite;
                break;
            case SceneData.SceneType.SURVIVAL:
                stageTagImage.sprite = survivalTagSprite;
                break;
            case SceneData.SceneType.SCORE:
                stageTagImage.sprite = scoreTagSprite;
                break;
            default:
                break;
        }
    }

    public void AppearWindow()
    {
        s_Interval.AppearWindow();
    }
}
