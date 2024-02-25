using System;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Localization
{
    public class JsonLocalizationService : ILocalizationService
    {
        private readonly Translator _translator;

        public JsonLocalizationService()
        {
            _translator = new Translator();
        }

        public void Initialize()
        {
            _translator.SetLanguage(GetLanguageKey());
            _translator.Initialize();
        }

        public string GetLocalized(LocalizationKeys key)
            => _translator.GetTranslation(key);

        private string GetLanguageKey()
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.English => "EN",
                SystemLanguage.Russian => "RU",
                _ => "EN"
            };
        }
    }
}