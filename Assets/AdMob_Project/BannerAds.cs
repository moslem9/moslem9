using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnitId;
    [SerializeField] string iosAdUnitId;

    string adUnitId;


    void Awake()
    {
        #if UNITY_IOS
                adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
    #elif UNITY_EDITOR
                adUnitId = androidAdUnitId;
    #endif

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }
    void Update()
    {
        
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions { 
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };
        Advertisement.Banner.Load(adUnitId, options);
    }

    private void BannerLoadedError(string message)
    {
        
    }

    private void BannerLoaded()
    {
        Debug.Log("Banner Ad Loaded");
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };
        Advertisement.Banner.Show(adUnitId, options);
    }

    private void BannerHidden()
    {
        
    }

    private void BannerClicked()
    {
        
    }

    private void BannerShown()
    {
        
    }

    public void HideBannerAd() {
        Advertisement.Banner.Hide();
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Banner ad loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

    }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Banner ad Completed");
    }
    #endregion
}
