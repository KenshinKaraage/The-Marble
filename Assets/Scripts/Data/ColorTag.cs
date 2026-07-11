using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTag : MonoBehaviour
{
    public ColorData Data { set; get; }
    public MarbleMaterialManager MaterialManager { set; get; }

    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] GameObject checkOb;
    S_AppearZoom checkZoom;

    private void Awake()
    {
        checkZoom = checkOb.GetComponent<S_AppearZoom>();
    }

    public void SetData(ColorData data)
    {
        Data = data;

        for (int i = 0; i < 2; i++)
        {
            images[i].color = data.colors[i];
        }
    }

    public void WhenPushed()
    {
        MaterialManager.ChangeMaterialColor(Data);
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
