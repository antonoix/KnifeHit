namespace Internal.Scripts.Infrastructure.Services.Sound
{
    public interface ISoundsService : IService
    {
        void PlaySound(SoundType soundType);
    }
}