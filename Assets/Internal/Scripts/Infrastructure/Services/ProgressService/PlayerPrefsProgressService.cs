using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.ProgressService
{
    public class PlayerPrefsProgressService : IPlayerProgressService
    {
        private const string PLAYER_COINS_KEY = "PlayerCoins";
        private const string PLAYER_PASSED_LEVEL_COUNT_KEY = "PlayerPassedLevelCount";

        public int GetCoinsCount()
            => PlayerPrefs.GetInt(PLAYER_COINS_KEY);
        
        public int GetPassedLevelsCount()
            => PlayerPrefs.GetInt(PLAYER_PASSED_LEVEL_COUNT_KEY);

        public void AddCoins(int addCount)
            => PlayerPrefs.SetInt(PLAYER_COINS_KEY, PlayerPrefs.GetInt(PLAYER_COINS_KEY) + addCount);
        
        public void RemoveCoins(int removeCount)
            => PlayerPrefs.SetInt(PLAYER_COINS_KEY, PlayerPrefs.GetInt(PLAYER_COINS_KEY) - removeCount);
        
        public void IncreasePassedLevel()
            => PlayerPrefs.SetInt(PLAYER_PASSED_LEVEL_COUNT_KEY, PlayerPrefs.GetInt(PLAYER_PASSED_LEVEL_COUNT_KEY) + 1);
    }
}
