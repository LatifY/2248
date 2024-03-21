using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedManager : MonoBehaviour
{
    public static RewardedManager instance;

    private enum RewardedAdState
    {
        Release,
        Test
    }

    private RewardedAd rAd;

    [SerializeField]
    private RewardedAdState adState;

    [SerializeField]
    private string releaseId;

    [SerializeField]
    private string testId;
    private string id;

    public GameObject loading;
    private int rewardAmount = 2;

    public bool isAdRequested = false;

    private Button _button;

    public bool canRequestAd = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        id = releaseId;
        if (adState == RewardedAdState.Test)
        {
            id = testId;
        }
    }

    public void ChangeRewardAmount(int amount)
    {
        rewardAmount = amount;
    }

    public void LoadRewardedAd()
    {
        if (!canRequestAd)
        {
            return;
        }
        if (rAd != null)
        {
            rAd.Destroy();
            rAd = null;
        }
        loading.SetActive(true);
        canRequestAd = false;
        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(
            id,
            adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

                rAd = ad;
                loading.SetActive(false);
                try
                {
                    if (rAd.CanShowAd())
                    {
                        rAd.Show(
                            (Reward reward) =>
                            {
                                DiamondManager.instance.AddDiamond(rewardAmount);
                                canRequestAd = true;
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    loading.SetActive(false);
                    Debug.LogError(ex.Message);
                }
            }
        );
    }
}
