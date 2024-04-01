using Internal.Scripts.Infrastructure.PlayerProgressService;

namespace Internal.Scripts.Infrastructure.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}