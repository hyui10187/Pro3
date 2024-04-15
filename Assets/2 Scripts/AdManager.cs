using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdManager : MonoBehaviour {

    public Transform groceryStoreSlotHolder; // 잡화상점 슬롯들의 부모
    public Transform equipmentSlotHolder; // 장비상점 슬롯들의 부모
    public Transform turnTableSlotHolder; // 턴테이블 슬롯들의 부모

    private BannerView bannerAd; // 배너광고
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
    }
    
    public void ShowIBannerAd() {
        GenerateBannerAds();
    }

    public void ShowInterstitialAd() {
        LoadInterstitialAd();
        StopCoroutine("ShowInterstitialAdCoroutine");
        StartCoroutine("ShowInterstitialAdCoroutine");
    }

    private void AdjustInterstitialAds() { // 전면 광고의 위치와 크기를 수정해주는 메소드
        GameObject interstitialParent = GameObject.Find("1024x768(Clone)"); // 전면 광고의 부모 게임 오브젝트를 찾기
        interstitialParent.name = "InterstitialParent"; // 전면 광고의 부모 게임 오브젝트의 이름을 변경해주기

        Navigation navigationOption = new Navigation();
        navigationOption.mode = Navigation.Mode.None;

        Button[] buttons = interstitialParent.GetComponentsInChildren<Button>();
        buttons[0].navigation = navigationOption;
        Button closeButton = buttons[1];
        closeButton.navigation = navigationOption;
        Text closeButtonText = closeButton.GetComponentInChildren<Text>();
        closeButtonText.text = "광고 닫기"; // 광고 닫는 버튼의 Text를 수정해주기
    }

    private IEnumerator ShowInterstitialAdCoroutine() { // 전면 광고를 표시해주기 위한 코루틴
        while(!interstitialAd.CanShowAd()) {
            yield return new WaitForSeconds(0.2f);
        }

        if(interstitialAd != null) {
            interstitialAd.Show();
            AdjustInterstitialAds();
        }
    }

    private void GenerateBannerAds() { // 배너 광고를 생성하는 메소드
        // if(bannerAd != null) { // 이미 배너가 존재할 경우
        //     bannerAd.Destroy(); // 배너를 삭제해주기
        //     bannerAd = null;
        // }
        
        bannerAd = new BannerView(adUnitId, AdSize.Banner, AdPosition.Center);
        AdRequest adRequest = new AdRequest();
        bannerAd.LoadAd(adRequest);

        if(bannerAd != null) {
            AdjustBannerAds();   
        }
    }

    private void AdjustBannerAds() { // 배너 광고의 위치와 크기를 수정해주는 메소드
        GameObject bannerParent = GameObject.Find("BANNER(Clone)"); // 배너 광고의 부모 게임 오브젝트를 찾기
        bannerParent.name = "BannerParent"; // 배너 광고의 부모 게임 오브젝트의 이름을 변경해주기
        Canvas bannerCanvas = bannerParent.GetComponent<Canvas>();
        bannerCanvas.sortingOrder = 0; // 배너 광고가 속한 Canvas의 Sort Order를 0으로 변경하기
        
        Image bannerImage = bannerParent.GetComponentInChildren<Image>(); // 배너 광고의 이미지 컴포넌트를 가져오기
        RectTransform rectTransform = bannerImage.GetComponent<RectTransform>();
        //Instantiate(bannerImage.gameObject, bannerObjectParent.transform); // 배너 광고를 복제하기

        // Rect Transform 컴포넌트의 Pivot을 좌측 상단으로 설정하기
        rectTransform.pivot = new Vector2(0f, 1); // Rect Transform 컴포넌트의 Pivot X와 Y를 수정
        rectTransform.anchorMin = new Vector2(0f, 1); // Rect Transform 컴포넌트의 Anchors의 Min X와 Y를 수정
        rectTransform.anchorMax = new Vector2(0f, 1); // Rect Transform 컴포넌트의 Anchors의 Max X와 Y를 수정
        
        rectTransform.anchoredPosition = new Vector2(15, -260); // Rect Transform 컴포넌트의 Pos X와 Pos Y를 수정
        rectTransform.sizeDelta = new Vector2(250, 50); // Rect Transform 컴포넌트의 Width와 Height를 수정

        Button bannerButton = bannerImage.GetComponent<Button>();
        Navigation navigationOption = new Navigation();
        navigationOption.mode = Navigation.Mode.None;
        bannerButton.navigation = navigationOption;

        GameObject groceryStoreBanner = Instantiate(bannerImage.gameObject, groceryStoreSlotHolder);
        groceryStoreBanner.name = "GroceryStoreBanner";
        GameObject equipmentStoreBanner = Instantiate(bannerImage.gameObject, equipmentSlotHolder);
        equipmentStoreBanner.name = "EquipmentStoreBanner";
        GameObject turnTableBanner = Instantiate(bannerImage.gameObject, turnTableSlotHolder);
        turnTableBanner.name = "TurnTableBanner";

        RectTransform groceryStoreBannerTransform = groceryStoreBanner.GetComponent<RectTransform>();
        groceryStoreBannerTransform.pivot = new Vector2(0, 0.8f);
        groceryStoreBannerTransform.localScale = new Vector3(1, 0.85f, 1);
        
        RectTransform equipmentStoreBannerTransform = equipmentStoreBanner.GetComponent<RectTransform>();
        equipmentStoreBannerTransform.pivot = new Vector2(0, 0.8f);
        equipmentStoreBannerTransform.localScale = new Vector3(1, 0.85f, 1);
        
        RectTransform turnTableBannerTransform = turnTableBanner.GetComponent<RectTransform>();
        turnTableBannerTransform.pivot = new Vector2(0, 0.8f);
        turnTableBannerTransform.localScale = new Vector3(1, 0.85f, 1);
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

    private void CloseInterstitialAd() { // 전면 광고를 닫는 버튼을 클릭했을때 호출할 메소드
        
    }
    
}