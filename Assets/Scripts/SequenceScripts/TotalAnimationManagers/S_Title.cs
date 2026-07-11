using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class S_Title : MonoBehaviour
{
    [SerializeField] S_AppearZoom titleLogoZoom;
    [SerializeField] S_AppearZoom titleStartButtonZoom;

    [SerializeField] S_AppearSlide menuNameTagSlide;
    [SerializeField] S_AppearSlide menuPaintSlide;
    [SerializeField] S_AppearSlide menuPlaySlide;
    [SerializeField] S_AppearSlide menuPauseSlide;
    public bool ShowingMenu;

    private void Awake()
    {
        ShowingMenu = false;   
    }

    public void TitleAppear()
    {
        titleLogoZoom.GetAppearZoomSequence();
        titleStartButtonZoom.GetAppearZoomSequence();
    }

    public void MenuAppear()
    {
        menuNameTagSlide.GetAppearSlideSequence();
        menuPaintSlide.GetAppearSlideSequence();
        menuPlaySlide.GetAppearSlideSequence();
        menuPauseSlide.GetAppearSlideSequence();
        ShowingMenu = true;
    }
}
