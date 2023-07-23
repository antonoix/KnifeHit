using UnityEngine;

public class AdvertisementService<T> where T : IAdsModule, new()
{
    private readonly T _adsModule;
    private const float _timeToWait = 3;
    private float _timeLastShow;

    public AdvertisementService()
    {
        _adsModule = new T();
        _adsModule.Init();
    }

    public void ShowAd()
    {
        bool canShow = Random.Range(0, 1) == 0;
        if (Time.time > _timeLastShow + _timeToWait && canShow)
        {
            _adsModule.ShowAd();
            _timeLastShow = Time.time;
        }
    }
}