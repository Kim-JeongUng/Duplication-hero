using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Admob : MonoBehaviour
{
    public bool isTestMode;
    public Button FrontAdsBtn;

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        LoadFrontAd();
    }


    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    const string frontTestID = "ca-app-pub-3940256099942544/1033173712";
    const string frontID = "ca-app-pub-4992780716235419/3478235803";
    InterstitialAd frontAd;

    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        if(null!=GameObject.Find("Ad"))
            GameObject.Find("Ad").transform.parent.GetComponent<Canvas>().sortingOrder = 101;
        frontAd.OnAdClosed += (sender, e) =>
        {
            GameController.instance.Continue();
        };
    }

    public void ShowFrontAd()
    {
        StartCoroutine(showInterstitial());
        
    }
    public IEnumerator showInterstitial()
    {
        int cnt = 0;
        LoadFrontAd();
        while (!frontAd.IsLoaded() && cnt < 10)
        {
            cnt++;
            yield return new WaitForSeconds(0.2f);
        }
        if (frontAd.IsLoaded())
        {
            frontAd.Show();
        }
        GameController.instance.Continue();
    }
}
