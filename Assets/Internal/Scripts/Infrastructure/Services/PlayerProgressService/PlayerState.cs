using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.PlayerProgressService
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
            AvailableWeapons = new List<string>() { CurrentWeapon };

            LastCompletedLevelIndex = -1;
        }

        public void IncreasePassedLevel() 
            => LastCompletedLevelIndex++;

        public WeaponType GetCurrentWeaponType()
        {
            if (Enum.TryParse(CurrentWeapon, out WeaponType weaponType))
            {
                return weaponType;
            }
            else
            {
                Debug.LogError($"Can not parse {CurrentWeapon} to enum");
                return default;
            }
        }

        public void SetCurrentWeapon(WeaponType weaponType)
        {
            CurrentWeapon = weaponType.ToString();
        }

        public void TryAddNewWeapon(WeaponType weaponType)
        {
            if (!AvailableWeapons.Contains(weaponType.ToString()))
                AvailableWeapons.Add(weaponType.ToString());
        }

        public bool IsWeaponAvailable(WeaponType weaponType)
        {
            return AvailableWeapons.Contains(weaponType.ToString());
        }
    }
}