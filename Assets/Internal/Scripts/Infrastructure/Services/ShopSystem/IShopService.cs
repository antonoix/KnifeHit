using Internal.Scripts.Infrastructure.Services;

namespace Internal.Scripts.Infrastructure.ShopSystem
{
    public interface IShopService : IService
    {
        void Init();
        void StartWork();
        void StopWork();
    }
}