using System;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using UnityEngine;

namespace Internal.Scripts.UI.ShopUI
{
    public class ShopUIPresenter : BaseUIPresenter<ShopUIView>
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISoundsService _soundsService;
        
        public event Action OnNextClicked;
        public event Action OnPrevClicked;
        public event Action OnBuyClicked;
        public event Action OnSelectClicked;
        public event Action OnMenuClicked;
        
        public ShopUIPresenter(ILocalizationService localizationService, ISoundsService soundsService)
        {
            _localizationService = localizationService;
            _soundsService = soundsService;
        }

        public override void Initialize(GameObject view)
        {
            base.Initialize(view);
            _view.Construct(_localizationService);
        }

        public override void Show()
        {
            base.Show();

            _view.OnBuyClicked += HandleBuyClicked;
            _view.OnSelectClicked += HandleSelectClicked;
            _view.OnNextClicked += HandleNextClicked;
            _view.OnPrevClicked += HandlePrevClicked;
            _view.OnMenuClicked += HandleMenuClicked;
        }

        public override void Hide()
        {
            base.Hide();
            
            _view.OnBuyClicked -= HandleBuyClicked;
            _view.OnSelectClicked -= HandleSelectClicked;
            _view.OnNextClicked -= HandleNextClicked;
            _view.OnPrevClicked -= HandlePrevClicked;
            _view.OnMenuClicked -= HandleMenuClicked;
        }

        public void SetSelectState(bool isSelected)
        {
            _view.SetSelectState(isSelected);
        }

        public void SetBuyState(long cost, bool canBuy)
        {
            _view.SetBuyState(cost, canBuy);
        }

        public void SetCurrentCoins(long count)
        {
            _view.SetCurrentCoins(count);
        }

        private void HandleMenuClicked()
        {
            OnMenuClicked?.Invoke();
            _soundsService.PlaySound(SoundType.ButtonClick);
        }

        private void HandleBuyClicked()
        {
            OnBuyClicked?.Invoke();
            _soundsService.PlaySound(SoundType.Cash);
        }

        private void HandleSelectClicked()
        {
            OnSelectClicked?.Invoke();
            _soundsService.PlaySound(SoundType.ButtonClick);
        }

        private void HandleNextClicked()
        {
            OnNextClicked?.Invoke();
            _soundsService.PlaySound(SoundType.ButtonClick);
        }

        private void HandlePrevClicked()
        {
            OnPrevClicked?.Invoke();
            _soundsService.PlaySound(SoundType.ButtonClick);
        }
    }
}