using System;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIPresenter : BaseUIPresenter<MenuUIView>
    {
        private readonly IPlayerProgressService _playerProgressService;
        private readonly ILocalizationService _localizationService;
        private readonly ISoundsService _soundsService;
        public event Action OnStartBtnClicked;

        public MenuUIPresenter(MenuUIView baseUIViewPrefab, IPlayerProgressService playerProgressService,
            ILocalizationService localizationService, ISoundsService soundsService) : base(baseUIViewPrefab)
        {
            _playerProgressService = playerProgressService;
            _localizationService = localizationService;
            _soundsService = soundsService;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _view.OnStartBtnClicked += HandleStartBtnClicked;
        }

        public void Dispose()
        {
            _view.OnStartBtnClicked -= HandleStartBtnClicked;
            
            _view.Dispose();
        }

        public override void Show()
        {
            string levelWord = _localizationService.GetLocalized(LocalizationKeys.Level);
            _view.SetCurrentLevel($"{levelWord} {_playerProgressService.GetPassedLevelsCount() + 1}");
            
            _view.SetStartText(_localizationService.GetLocalized(LocalizationKeys.Start));
            
            base.Show();
        }

        private void HandleStartBtnClicked()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnStartBtnClicked?.Invoke();
        }
    }
}
