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
            _view.OnSettingsBtnClicked += HandleSettingsBtnClicked;
            _view.MenuSettings.OnExitClicked += HandleExitSettingsClicked;
            _view.MenuSettings.EnableSoundBtn.OnToggled += HandleSoundClicked;
            
            string levelWord = _localizationService.GetLocalized(LocalizationKeys.Level);
            _view.SetCurrentLevel($"{levelWord} {_levelsService.GetCurrentLevelIndex() + 1}");
            
            _view.MenuSettings.Activate(false);
            _view.SetStartText(_localizationService.GetLocalized(LocalizationKeys.Start));
            _view.SetCurrentCoins(_playerProgressService.PlayerProgress.ResourcePack[ResourceType.Coin].Value);
            _view.SetCurrentStars(_playerProgressService.PlayerProgress.ResourcePack[ResourceType.Star].Value);
            
            base.Show();
        }

        public override void Hide()
        {
            _view.OnStartBtnClicked -= HandleStartBtnClicked;
            _view.OnShopBtnClicked -= HandleShopBtnClicked;
            _view.OnSettingsBtnClicked -= HandleSettingsBtnClicked;
            _view.MenuSettings.OnExitClicked -= HandleExitSettingsClicked;
            _view.MenuSettings.EnableSoundBtn.OnToggled -= HandleSoundClicked;
            
            base.Hide();
        }

        private void HandleExitSettingsClicked()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            _view.MenuSettings.Activate(false);
        }

        private void HandleSoundClicked(bool isEnabled)
        {
            _soundsService.EnableAudio(isEnabled);
            _soundsService.PlaySound(SoundType.ButtonClick);
        }

        private void HandleSettingsBtnClicked()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            _view.MenuSettings.Activate(true);
            _view.MenuSettings.EnableSoundBtn.SwitchState(_soundsService.IsSoundEnabled);
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
