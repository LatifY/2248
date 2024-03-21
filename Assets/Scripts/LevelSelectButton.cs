using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LevelSelectButtonState
{
    Locked,//0
    Unlocked,//1
    Completed//2
}

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;//Level_1
    public LevelSelectButtonState state;
    public TextMeshProUGUI levelNumberText;

    public Sprite lockedSprite, unlockedSprite, completedSprite;

    private Image image;
    private Button button;

    private void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void Init(){
        levelNumberText.text = levelName.Split('_')[1];
        state = (LevelSelectButtonState)PlayerPrefs.GetInt(levelName, 0);
        switch (state)
        {
            case LevelSelectButtonState.Locked:
                image.sprite = lockedSprite;
                button.interactable = false;
                break;
            case LevelSelectButtonState.Unlocked:
                image.sprite = unlockedSprite;
                button.interactable = true;
                break;
            case LevelSelectButtonState.Completed:
                image.sprite = completedSprite;
                button.interactable = false;
                break;
        }
    }
}