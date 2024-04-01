using System;
using System.Collections.Generic;
using Internal.Scripts.GamePlay.ShopSystem;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.ProgressService
{
    public class PlayerPrefsProgressService : IPlayerProgressService, IInitializable
    {
        private const string PLAYER_COINS_KEY = "PlayerCoins";
        private const string PLAYER_PASSED_LEVEL_COUNT_KEY = "PlayerPassedLevelCount";
        private const string PLAYER_WEAPON_KEY = "PlayerCurrentWeapon";

        public void Initialize()
        {
            if (!PlayerPrefs.HasKey(PLAYER_WEAPON_KEY))
            {
                PlayerPrefs.SetString(PLAYER_WEAPON_KEY, WeaponType.Axe.ToString());
            }
            
            PlayerPrefs.SetInt(WeaponType.Axe.ToString(), 1);
        }

        public int GetCoinsCount()
            => PlayerPrefs.GetInt(PLAYER_COINS_KEY);

        public int GetPassedLevelsCount()
            => PlayerPrefs.GetInt(PLAYER_PASSED_LEVEL_COUNT_KEY);

        public void AddAvailableWeapon(WeaponType type)
        {
            PlayerPrefs.SetInt(type.ToString(), 1);
        }

        public void SetCurrentSelectedWeapon(WeaponType type)
        {
            PlayerPrefs.SetString(PLAYER_WEAPON_KEY, type.ToString());
        }

        public WeaponType GetCurrentSelectedWeapon()
        {
            if (PlayerPrefs.HasKey(PLAYER_WEAPON_KEY))
            {
                if (Enum.TryParse(PlayerPrefs.GetString(PLAYER_WEAPON_KEY), out WeaponType shopItem))
                {
                    return shopItem;
                }
            }

            return WeaponType.Axe;
        }

        public List<WeaponType> GetAllAvailableWeapons()
        {
            List<WeaponType> availableWeapons = new();
            foreach (WeaponType shopItem in Enum.GetValues(typeof(WeaponType)))
            {
                if (PlayerPrefs.GetInt(shopItem.ToString()) == 1)
                    availableWeapons.Add(shopItem);
            }

            return availableWeapons;
        }

        public void AddCoins(int addCount)
            => PlayerPrefs.SetInt(PLAYER_COINS_KEY, PlayerPrefs.GetInt(PLAYER_COINS_KEY) + addCount);
        
        public void RemoveCoins(int removeCount)
            => PlayerPrefs.SetInt(PLAYER_COINS_KEY, Mathf.Clamp(PlayerPrefs.GetInt(PLAYER_COINS_KEY) - removeCount, 0, Int32.MaxValue));
        
        public void IncreasePassedLevel()
            => PlayerPrefs.SetInt(PLAYER_PASSED_LEVEL_COUNT_KEY, PlayerPrefs.GetInt(PLAYER_PASSED_LEVEL_COUNT_KEY) + 1);
    }
}
