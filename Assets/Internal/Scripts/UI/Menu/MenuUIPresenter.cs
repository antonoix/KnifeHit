using System;
using Internal.Scripts.GamePlay.LevelsService;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIPresenter : BaseUIPresenter<MenuUIView>
    {
        private readonly IPersistentProgressService _playerProgressService;
        private readonly ILocalizationService _localizationService;
        private readonly ISoundsService _soundsService;
        private readonly ILevelsService _levelsService;
        public event Action OnStartBtnClicked;
        public event Action OnShopBtnClicked;

        public MenuUIPresenter(IPersistentProgressService playerProgressService,
            ILocalizationService localizationService, ISoundsService soundsService, ILevelsService levelsService)
        {
            _playerProgressService = playerProgressService;
            _localizationService = localizationService;
            _soundsService = soundsService;
            _levelsService = levelsService;
        }

        public override void Show()
        {
            _view.OnStartBtnClicked += HandleStartBtnClicked;
            _view.OnShopBtnClicked += HandleShopBtnClicked;
            
            string levelWord = _localizationService.GetLocalized(LocalizationKeys.Level);
            _view.SetCurrentLevel($"{levelWord} {_levelsService.GetCurrentLevelIndex() + 1}");
            
            _view.SetStartText(_localizationService.GetLocalized(LocalizationKeys.Start));
            _view.SetCurrentCoins(_playerProgressService.PlayerProgress.ResourcePack[ResourceType.Coin].Value);
            _view.SetCurrentStars(_playerProgressService.PlayerProgress.ResourcePack[ResourceType.Star].Value);
            
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
