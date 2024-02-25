using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    [CreateAssetMenu(fileName = "UiServiceInjector", menuName = "Injectors/UiServiceInjector")]
    public class UiServiceInjector : ServiceInjector<IUiService>
    {
        [SerializeField] private UiConfig config;

        private IPlayerProgressService _playerProgressService;
        private ILocalizationService _localizationService;
        private ISoundsService _soundsService;

        public void Construct(IPlayerProgressService playerProgressService, ILocalizationService localizationService,
            ISoundsService soundsService)
        {
            _playerProgressService = playerProgressService;
            _localizationService = localizationService;
            _soundsService = soundsService;
        }
        
        public override IUiService Create()
        {
            _service = new UIService(config, _playerProgressService, _localizationService, _soundsService);
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _service.Initialize();
        }
    }
}