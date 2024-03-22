using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class InitAdMob : MonoBehaviour
{
    public void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => {});
    }
}
