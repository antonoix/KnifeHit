namespace Internal.Scripts.Infrastructure.PlayerProgressService
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly Settings _settings;
        
        public PlayerProgress PlayerProgress { get; set; }

        public PersistentProgressService(Settings settings)
        {
            _settings = settings;
        }
        
        public void InitNewProgress()
        {
            PlayerProgress = new PlayerProgress(_settings);
        }

    }
}