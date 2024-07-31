using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;

namespace Internal.Scripts.GamePlay.LevelsService
{
    public interface ILevelsService
    {
        UniTask Initialize();
        int GetAllLevelsCount();
        int GetCurrentLevelIndex();
        LevelContext CreateLevelContext();
        LevelContext CurrentLevel { get; }
        void DestroyCurrentLevel();
    }
}