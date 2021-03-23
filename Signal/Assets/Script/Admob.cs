using UnityEngine;
using GoogleMobileAds.Api;


public class Admob : MonoBehaviour
{
    static bool isAdsBannerLoaded = false;
    public string AndroidBannerID;

    private void Start()
    {
        if (!isAdsBannerLoaded)
        {
            RequestBanner();
        }
    }

    private void RequestBanner()
    {
        //Create Banner at the Screen
        BannerView bannerView = new BannerView(AndroidBannerID, AdSize.Banner, AdPosition.Bottom);

        //Create empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        //Load Banner
        bannerView.LoadAd(request);

        isAdsBannerLoaded = true;
    }
}