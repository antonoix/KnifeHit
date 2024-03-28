using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Internal.Scripts.Infrastructure.Services.Localization
{
    public class JsonTranslator
    {
        private Dictionary<string, Dictionary<string, string>> _localization;
        private string _lang;

        public void SetLanguage(string lang)
        {
            _lang = lang;
        }

        public void Initialize()
        {
            string localizationJson = Resources.Load<TextAsset>("LocalizationTable").text;

            _localization  = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(localizationJson);

            if (_localization == null)
                throw new Exception("Cant parse localization JSON");
        }

        public string GetTranslation(LocalizationKeys key)
        {
            var stringKey = key.ToString();
            
            return _localization[_lang].ContainsKey(stringKey) ? _localization[_lang][stringKey] : string.Empty;
        }
    }
}