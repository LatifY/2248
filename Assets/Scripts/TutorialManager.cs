using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform imagePanel;
    public List<GameObject> images;

    public GameObject resumeButton,
        navigateButton;

    [TextArea]
    public string[] texts;

    public TextMeshProUGUI text;

    private int pageIndex;

    private void Awake()
    {
        return;
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            Destroy(gameObject);
            return;
        }
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    private void Start()
    {
        for (int i = 0; i < imagePanel.childCount; i++)
        {
            images.Add(imagePanel.GetChild(i).gameObject);
            images[i].SetActive(false);
        }
        images[0].SetActive(true);
        pageIndex = 0;
    }

    public void ChangePage(int by)
    {
        if (pageIndex + by < 0 || pageIndex + by > images.Count - 1)
        {
            return;
        }

        pageIndex += by;
        foreach (var item in images)
        {
            item.SetActive(false);
        }
        images[pageIndex].SetActive(true);
        text.text = texts[pageIndex];
    }

    private void Update()
    {
        resumeButton.SetActive(pageIndex == images.Count - 1);
        navigateButton.SetActive(pageIndex != images.Count - 1);
    }
}
