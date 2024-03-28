using Internal.Scripts.Infrastructure.Services.Ads;
using UnityEngine;
using UnityEngine.Advertisements;

public class CustomUnityAdsManager : IAdsManager, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private readonly UnityAdsConfig _config;

    public CustomUnityAdsManager(UnityAdsConfig config)
    {
        _config = config;
    }
    
    public void Initialize()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_config.AndroidGameId, _config.IsTestMode, this);
        }
    }
    
    public void LoadAd()
    {
        Advertisement.Load("Interstitial_Android", this);
    }
    
    public void ShowAd()
    {
        Advertisement.Show("Interstitial_Android", this);
    }

    public void TryShowAdAfterWin()
    {
        if (UnityEngine.Random.Range(0, 1f) < _config.ShowAdAfterWinChance)
            ShowAd();
    }

    public void OnInitializationComplete()
    {
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Show ad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        
    }
}
