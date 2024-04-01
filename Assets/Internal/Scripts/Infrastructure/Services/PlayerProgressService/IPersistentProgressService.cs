namespace Internal.Scripts.Infrastructure.PlayerProgressService
{
    public interface IPersistentProgressService
    {
        PlayerProgress PlayerProgress { get; set; }
        void InitNewProgress();
    }
}