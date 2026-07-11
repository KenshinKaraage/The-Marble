using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternTag : MonoBehaviour
{
    public PatternData Data { set; get; }
    public MarbleMaterialManager MaterialManager { set; get; }

    [SerializeField] Image image;
    [SerializeField] GameObject checkOb;
    S_AppearZoom checkZoom;

    private void Awake()
    {
        checkZoom = checkOb.GetComponent<S_AppearZoom>();
    }

    public void SetData(PatternData data)
    {
        Data = data;

        image.sprite = data.patternSprite;
    }

    public void WhenPushed()
    {
        MaterialManager.ChangeMaterialPattern(Data);
    }

    public void SetCheckObActive(bool active)
    {
        checkOb.SetActive(active);

        if (active)
        {
            checkZoom.GetAppearZoomSequence();
        }
    }
}
