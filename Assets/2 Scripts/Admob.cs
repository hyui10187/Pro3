using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour {

    private BannerView bannerView; // 배너광고
    private InterstitialAd interstitialAd; // 전면광고

    #if UNITY_ANDROID
        private string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
      private string adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
      private string adUnitId = "unused";
    #endif
    
    public void Start() {
        MobileAds.Initialize((InitializationStatus initStatus) => { });
        RequestBanner();
    }

    public void ShowInterstitialAd() {
        LoadInterstitialAd();
        StopCoroutine("ShowInterstitialAdCoroutine");
        StartCoroutine("ShowInterstitialAdCoroutine");
    }

    private IEnumerator ShowInterstitialAdCoroutine() { // 전면광고를 표시해주기 위한 코루틴
        while(!interstitialAd.CanShowAd()) {
            yield return new WaitForSeconds(0.2f);
        }

        if(interstitialAd != null) {
            interstitialAd.Show();   
        }
    }

    private void RequestBanner() { // 배너광고를 로드하는 메소드
        if(bannerView != null) { // 이미 배너가 존재할 경우
            bannerView.Destroy(); // 배너를 삭제해주기
            bannerView = null;
        }

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        AdSize adSize = new AdSize(300, 200);
        
        bannerView = new BannerView(adUnitId, adSize, 0, -400);

        AdRequest adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    private void LoadInterstitialAd() { // 전면광고를 로드하는 메소드
        if(interstitialAd != null) { // Clean up the old ad before loading a new one.
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest(); // create our request used to load the ad.

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) => {
                if(ad == null || error != null) // if error is not null, the load request failed.
                    return;
                interstitialAd = ad;
        });

        if(interstitialAd != null) {
            interstitialAd.OnAdFullScreenContentClosed += () => { CloseInterstitialAd(); };
        }
    }

    private void CloseInterstitialAd() { // 전면광고를 닫는 버튼을 클릭했을때 호출할 메소드
        
    }
    
}