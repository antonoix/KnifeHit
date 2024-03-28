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
        public event Action OnShopBtnClicked;

        public MenuUIPresenter(IPlayerProgressService playerProgressService,
            ILocalizationService localizationService, ISoundsService soundsService)
        {
            _playerProgressService = playerProgressService;
            _localizationService = localizationService;
            _soundsService = soundsService;
        }

        public override void Show()
        {
            _view.OnStartBtnClicked += HandleStartBtnClicked;
            _view.OnShopBtnClicked += HandleShopBtnClicked;
            
            string levelWord = _localizationService.GetLocalized(LocalizationKeys.Level);
            _view.SetCurrentLevel($"{levelWord} {_playerProgressService.GetPassedLevelsCount() + 1}");
            
            _view.SetStartText(_localizationService.GetLocalized(LocalizationKeys.Start));
            _view.SetCurrentCoins(_playerProgressService.GetCoinsCount());
            
            base.Show();
        }

        public override void Hide()
        {
            _view.OnStartBtnClicked -= HandleStartBtnClicked;
            _view.OnShopBtnClicked -= HandleShopBtnClicked;
            
            base.Hide();
        }
        
        private void HandleStartBtnClicked()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnStartBtnClicked?.Invoke();
        }
        
        private void HandleShopBtnClicked()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnShopBtnClicked?.Invoke();
        }
    }
}
