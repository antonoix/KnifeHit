using System;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.Localization
{
    public class JsonLocalizationService : ILocalizationService, IInitializable
    {
        private readonly JsonTranslator _jsonTranslator;

        public JsonLocalizationService()
        {
            _jsonTranslator = new JsonTranslator();
        }

        public void Initialize()
        {
            _jsonTranslator.SetLanguage(GetLanguageKey());
            _jsonTranslator.Initialize();
        }

        public string GetLocalized(LocalizationKeys key)
            => _jsonTranslator.GetTranslation(key);

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