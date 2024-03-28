using Internal.Scripts.Infrastructure.Services;

public interface IAdsManager : IService
{
    void Initialize();
    void LoadAd();
    void ShowAd();
    void TryShowAdAfterWin();
}
