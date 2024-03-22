using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Image musicCloseImage, audioCloseImage;
    public Button musicButton, audioButton;

    private void Awake() {
        int music = PlayerPrefs.GetInt("music", 1);
        int audio = PlayerPrefs.GetInt("audio", 1);
        musicCloseImage.gameObject.SetActive(music == 0);
        audioCloseImage.gameObject.SetActive(audio == 0);
    }

    private void Start() {
        MusicManager.Instance.ChangeVolume();
    }

    public void ChangeAudio(){
        bool wasOn = PlayerPrefs.GetInt("audio", 1) == 1;
        PlayerPrefs.SetInt("audio", wasOn ? 0 : 1);
        audioCloseImage.gameObject.SetActive(wasOn);
    }

    public void ChangeMusic(){
        bool wasOn = PlayerPrefs.GetInt("music", 1) == 1;
        PlayerPrefs.SetInt("music", wasOn ? 0 : 1);
        musicCloseImage.gameObject.SetActive(wasOn);
        MusicManager.Instance.ChangeVolume();
    }


}
