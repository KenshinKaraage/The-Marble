using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PlayerTag : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] GameObject flagOb;
    S_AppearZoom checkZoom;

    public Player player;

    private void Awake()
    {
        checkZoom = flagOb.GetComponent<S_AppearZoom>();
    }

    public void ChangeNameText(string text)
    {
        nameText.text = text;
    }

    public void SetFlag(bool set)
    {
        flagOb.SetActive(set);

        if (set)
        {
            checkZoom.GetAppearZoomSequence();
        }
    }
}
