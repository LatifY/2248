using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class BannerManager : MonoBehaviour
{
    private enum IntersitialAdState
    {
        Release,
        Test
    }

    private BannerView _bannerView;

    [SerializeField]
    private IntersitialAdState adState;

    [SerializeField]
    private string releaseId;

    [SerializeField]
    private string testId;
    private string id;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        id = releaseId;
        if (adState == IntersitialAdState.Test)
        {
            id = testId;
        }

        if (_bannerView == null)
        {
            LoadAd();
        }
        else
        {
            _bannerView.Destroy();
        }
    }

    public void LoadAd()
    {
        if (_bannerView == null)
        {
            _bannerView = new BannerView(id, AdSize.Banner, AdPosition.Bottom);
        }

        AdRequest request = new AdRequest.Builder().Build();

        _bannerView.LoadAd(request);
    }
}
