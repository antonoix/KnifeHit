namespace Internal.Scripts.Infrastructure.Services.PlayerProgressService
{
    public interface IPersistentProgressService
    {
        PlayerProgress PlayerProgress { get; set; }
        void InitNewProgress();
    }
}