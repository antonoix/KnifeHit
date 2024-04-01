using System.Collections.Generic;
using Internal.Scripts.GamePlay.ShopSystem;

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
        WeaponType GetCurrentSelectedWeapon();
        List<WeaponType> GetAllAvailableWeapons();
        void AddAvailableWeapon(WeaponType type);
        void SetCurrentSelectedWeapon(WeaponType type);
    }
}
