using YG;

namespace Internal.Scripts.Infrastructure.Services.Ads
{
    public class CustomYaGamesAdsService : IAdsService
    {
        public void Initialize()
        {
            
        }

        public void LoadAd()
        {
            
        }

        public void ShowAd()
        {
            YandexGame.FullscreenShow();
        }

        public void TryShowAdAfterWin()
        {
            ShowAd();
        }
    }
}