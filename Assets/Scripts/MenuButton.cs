using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private Vector3 originalScale;
    private Button button;
    public Ease easeType = Ease.OutBack;

    public bool animate = true;

    public string playInstead = "";

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(Click);
        originalScale = transform.localScale;
    }

    public void Click()
    {

        AudioManager.Instance.PlayClipWithRandomPitch(playInstead != "" ? playInstead : "button", 0.8f, 1);

        if (animate)
        {
            transform
                .DOScale(originalScale * 1.1f, 0.07f)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, 0.07f).SetEase(easeType).SetUpdate(true);
                }).SetUpdate(true);
        }
    }
}
