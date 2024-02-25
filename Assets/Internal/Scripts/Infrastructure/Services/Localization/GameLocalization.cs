using System;
using System.Collections.Generic;

namespace Internal.Scripts.Infrastructure.Services.Localization
{
    [Serializable]
    public class GameLocalization
    {
        public Dictionary<string, Dictionary<string, string>> LangAndTranslations;
    }

    [Serializable]
    public class KeysAndTranslations
    {
        public string Level { get; set; }
        public string Win { get; set; }
        public string Lose { get; set; }
    }
}