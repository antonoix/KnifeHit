using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Localization
{
    [CreateAssetMenu(fileName = "LocalizationServiceInjector", menuName = "Injectors/LocalizationServiceInjector")]
    public class LocalizationServiceInjector : ServiceInjector<ILocalizationService>
    {
        //[SerializeField] private SpecialEffectsConfig config;

        public override ILocalizationService Create()
        {
            _service = new JsonLocalizationService();
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _service.Initialize();
        }
    }
}