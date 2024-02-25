namespace Internal.Scripts.Infrastructure.Services.ProgressService
{
    public interface IPlayerProgressService : IService
    {
        void AddCoins(int addCount);
        void RemoveCoins(int removeCount);
        void IncreasePassedLevel();
        int GetCoinsCount();
        int GetPassedLevelsCount();
    }
}
