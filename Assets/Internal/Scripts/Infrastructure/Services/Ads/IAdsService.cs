namespace Internal.Scripts.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        void Initialize();
        void LoadAd();
        void ShowAd();
        void TryShowAdAfterWin();
    }
}
