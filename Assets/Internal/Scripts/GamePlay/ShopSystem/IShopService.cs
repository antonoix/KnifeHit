using Internal.Scripts.Infrastructure.Services;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public interface IShopService : IService
    {
        void StartWork();
        void StopWork();
    }
}