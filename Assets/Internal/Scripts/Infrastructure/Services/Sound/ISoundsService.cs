namespace Internal.Scripts.Infrastructure.Services.Sound
{
    public interface ISoundsService : IService
    {
        void Initialize();
        void PlaySound(SoundType soundType);
    }
}