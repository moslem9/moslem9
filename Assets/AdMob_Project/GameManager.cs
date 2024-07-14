using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int gamePlayed = 1;
    public bool isRewarded;

    public static GameManager Instance { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(DisplayBannerWithDelay());
    }

    IEnumerator DisplayBannerWithDelay() {
        yield return new WaitForSeconds(2);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    
    void Update()
    {
        if (score < 0) {
            score = 0;
            AdsManager.Instance.bannerAds.HideBannerAd();
            SceneManager.LoadScene("EndScene");
            isRewarded = false;
            if (gamePlayed % 3 == 0){
                AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            }
        }
        else if (score > 10)
        {
            score = 10;
            AdsManager.Instance.bannerAds.HideBannerAd();
            SceneManager.LoadScene("EndScene");
            isRewarded = false;
            if (gamePlayed % 3 == 0) {
                AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            }
        }
    }

    public void RestartGame() {
        score = 0;
        gamePlayed++;
        AdsManager.Instance.bannerAds.ShowBannerAd();
        SceneManager.LoadScene("Gameplay");
    }

    public void AddScore() {
        if (isRewarded)
        {
            score += 2;
        }
        else
        {
            score++;
        }
    }

    public void MinusScore() {
        score--;
    }
}
