namespace Internal.Scripts.Infrastructure.Services.Localization
{
    public interface ILocalizationService : IService
    {
        void Initialize();
        string GetLocalized(LocalizationKeys key);
    }
}