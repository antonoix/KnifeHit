using Internal.Scripts.Infrastructure.Services;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public interface IShopService : IService
    {
        void Init();
        void StartWork();
        void StopWork();
    }
}