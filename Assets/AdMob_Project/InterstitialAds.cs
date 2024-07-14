using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
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
    }

    void Update()
    {
        
    }

    public void LoadInterstitialAd() {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAd() {
        Advertisement.Show(adUnitId, this);
        LoadInterstitialAd();
    }

    #region LoadCallBacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial ad loaded");
    }
    
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }
    #endregion

    #region ShowCallBacks
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
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ad fully Watched");
        }
    }
    #endregion

}
