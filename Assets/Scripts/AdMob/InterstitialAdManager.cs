using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdManager : MonoBehaviour
{
    private enum IntersitialAdState
    {
        Release,
        Test
    }

    private InterstitialAd _interstitialAd;

    [SerializeField]
    private IntersitialAdState adState;

    [SerializeField]
    private string releaseId;

    [SerializeField]
    private GameObject adPanel;

    [SerializeField]
    private string testId;
    private string id;
    private bool adShowed = false;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("NoAds") == 1){
            Destroy(gameObject);
            return;
        }
        id = releaseId;
        if (adState == IntersitialAdState.Test)
        {
            id = testId;
        }
        int restart = PlayerPrefs.GetInt("Restart", 0);
        int adCount = PlayerPrefs.GetInt("AdCount", 0);

        if(restart == 1){
            PlayerPrefs.SetInt("Restart", 0);
            Request();
        }
        else{
            if(adCount == 3){
                PlayerPrefs.SetInt("AdCount", 0);
                Request();
            }
            else{
                PlayerPrefs.SetInt("AdCount", adCount + 1);
            }
        }
    }

    public void Request()
    {
        Time.timeScale = 0;
        adPanel.SetActive(true);
        AdRequest request = new AdRequest();
        InterstitialAd.Load(
            id,
            request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log(error);
                    return;
                }
                _interstitialAd = ad;
            }
        );
    }

    private void Update()
    {
        if (!adShowed && _interstitialAd != null)
        {
            if (_interstitialAd.CanShowAd())
            {
                Time.timeScale = 1;
                adShowed = true;
                adPanel.SetActive(false);
                _interstitialAd.Show();
            }
        }
    }
}
