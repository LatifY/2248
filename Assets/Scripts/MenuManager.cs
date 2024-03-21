using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup[] panels;
    public int currentIndex;

    public bool changingPanel = false;

    public float panelTransitionTime = 1f;

    private void Awake()
    {
        foreach (var panel in panels)
        {
            panel.alpha = 0;
            panel.interactable = false;
            panel.gameObject.SetActive(false);
        }

        currentIndex = 0;
    }

    private void Start()
    {
        DOTween.Init();
        ShowPanel(0);
    }

    public void RateGame(){
        Application.OpenURL("market://details?id=" + Application.productName);
    }

    public void ShowPanel(int panelIndex)
    {
        if (changingPanel)
        {
            return;
        }

        if (currentIndex != panelIndex)
        {
            PanelTransition(currentIndex, false);
        }

        PanelTransition(panelIndex, true);
        currentIndex = panelIndex;
    }

    public void PanelTransition(int index, bool open)
    {
        changingPanel = true;
        if(open){
            panels[index].gameObject.SetActive(true);
        }
        panels[index].interactable = open;
        panels[index]
            .DOFade(open ? 1 : 0, panelTransitionTime)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                changingPanel = false;
                if (!open)
                {
                    panels[index].gameObject.SetActive(false);
                }
            });
    }
}
