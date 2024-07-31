using UnityEngine;
using UnityEngine.Advertisements;

namespace Internal.Scripts.Infrastructure.Advertisement
{
    public class UnityAdsManager : IAdsModule, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string UNITY_AD_GAME_ID = "5333409";
        private const string INTERSTITIAL_ANDROID = "Interstitial_Android";

        public void Init()
        {
            UnityEngine.Advertisements.Advertisement.Initialize(UNITY_AD_GAME_ID, false, this);
            UnityEngine.Advertisements.Advertisement.Load(INTERSTITIAL_ANDROID, this);
        }

        public void ShowAd()
        {
            UnityEngine.Advertisements.Advertisement.Show(INTERSTITIAL_ANDROID, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("UnityAds Inited");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogWarning(message);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogWarning(message);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
        }
    }
}
