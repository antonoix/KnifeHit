using UnityEngine;
using YG;

namespace Internal.Scripts.Infrastructure.Services.LeaderBoards
{
    public class LeaderBoardsServiceYaGames : ILeaderBoardsService
    {
        private const string LEADERBOARD_NAME = "PassedLevels";
        
        public void RegisterRecord(int record)
        {
            YandexGame.NewLeaderboardScores(LEADERBOARD_NAME, record);
        }
    }
}