using System;
using System.Collections.Generic;
using Internal.Scripts.GamePlay.ShopSystem;

namespace Internal.Scripts.Infrastructure.PlayerProgressService
{
    [Serializable]
    public class PlayerState
    {
        public string CurrentWeapon;
        public List<string> AvailableWeapons;
        public int LastCompletedLevelIndex;

        
        public PlayerState() { }
        
        public PlayerState(Settings settings)
        {
            CurrentWeapon = settings.DefaultWeaponType.ToString();

            LastCompletedLevelIndex = -1;
        }

        public void TryAddNewWeapon(WeaponType weaponType)
        {
            if (!AvailableWeapons.Contains(weaponType.ToString()))
                AvailableWeapons.Add(weaponType.ToString());
        }
    }
}