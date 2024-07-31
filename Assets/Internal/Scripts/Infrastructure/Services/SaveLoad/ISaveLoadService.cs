using Internal.Scripts.Infrastructure.Services.PlayerProgressService;

namespace Internal.Scripts.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}