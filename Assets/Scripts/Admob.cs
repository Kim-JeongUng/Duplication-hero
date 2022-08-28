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
    public string frontTestID;
    public string frontID;

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadFrontAd();
    }

    void Update()
    {
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }


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
        IEnumerator showInterstitial()
        {
            int cnt = 0;
            while (!frontAd.IsLoaded() && cnt < 5)
            {
                cnt++;
                yield return new WaitForSeconds(0.2f);
            }
            if (cnt >= 5)
            {
                GameController.instance.Continue();
            }
            frontAd.Show();
            LoadFrontAd();
        }
    }

}
