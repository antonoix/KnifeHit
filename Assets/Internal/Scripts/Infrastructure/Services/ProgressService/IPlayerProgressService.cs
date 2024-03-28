using System.Collections.Generic;
using Internal.Scripts.Infrastructure.Services.ShopSystem;

namespace Internal.Scripts.Infrastructure.Services.ProgressService
{
    public interface IPlayerProgressService : IService
    {
        void Initialize();
        void AddCoins(int addCount);
        void RemoveCoins(int removeCount);
        int GetCoinsCount();
        void IncreasePassedLevel();
        int GetPassedLevelsCount();
        ShopItemType GetCurrentSelectedWeapon();
        List<ShopItemType> GetAllAvailableWeapons();
        void AddAvailableWeapon(ShopItemType type);
        void SetCurrentSelectedWeapon(ShopItemType type);
    }
}
