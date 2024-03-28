using System.Linq;
using Internal.Scripts.GamePlay.Context;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.ShopSystem;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.ShopUI;

namespace Internal.Scripts.Infrastructure.ShopSystem
{
    public class ShopService : IShopService
    {
        private readonly IUiService _uiService;
        private readonly ShopContextFactory _shopContextFactory;
        private readonly IPlayerProgressService _playerProgressService;
        private ShopContext _shopContext;
        private ShopUIPresenter _shopUIPresenter;

        private int _currentShopItemIndex;
        private ShopItem CurrentShopItem => _shopContext.ShopItems[_currentShopItemIndex];

        public ShopService(IUiService uiService, ShopContext shopContext, IPlayerProgressService playerProgressService)
        {
            _uiService = uiService;
            _shopContextFactory = new ShopContextFactory(shopContext);
            _playerProgressService = playerProgressService;
        }

        public void Init()
        {
            _shopUIPresenter = (ShopUIPresenter) _uiService.GetPresenter<ShopUIPresenter>();
        }

        public void StartWork()
        {
            _shopContext = _shopContextFactory.InstantiateShopContext();
            
            FocusCurrentWeapon();

            _shopUIPresenter.Show();
            UpdateUI();
            
            _shopUIPresenter.OnNextClicked += HandleNextClicked;
            _shopUIPresenter.OnPrevClicked += HandlePrevClicked;
            _shopUIPresenter.OnBuyClicked += HandleBuyClicked;
            _shopUIPresenter.OnSelectClicked += HandleSelectClicked;
        }

        private void FocusCurrentWeapon()
        {
            var selectedWeapon = _playerProgressService.GetCurrentSelectedWeapon();
            for (int i = 0; i < _shopContext.ShopItems.Length; i++)
            {
                if (_shopContext.ShopItems[i].Type == selectedWeapon)
                {
                    _shopContext.ShopCamera.Focus(_shopContext.ShopItems[i].transform);
                    _currentShopItemIndex = i;
                    break;
                }
            }
        }

        public void StopWork()
        {
            _shopUIPresenter.Hide();
            
            _shopUIPresenter.OnNextClicked -= HandleNextClicked;
            _shopUIPresenter.OnPrevClicked -= HandlePrevClicked;
            _shopUIPresenter.OnBuyClicked -= HandleBuyClicked;
            _shopUIPresenter.OnSelectClicked -= HandleSelectClicked;
        }

        private void HandlePrevClicked()
        {
            if (--_currentShopItemIndex < 0)
                _currentShopItemIndex = _shopContext.ShopItems.Length - 1;
            _shopContext.ShopCamera.Focus(CurrentShopItem.transform);
            
            UpdateUI();
        }

        private void HandleNextClicked()
        {
            if (++_currentShopItemIndex >= _shopContext.ShopItems.Length)
                _currentShopItemIndex = 0;
            _shopContext.ShopCamera.Focus(CurrentShopItem.transform);
            
            UpdateUI();
        }

        private void HandleBuyClicked()
        {
            _playerProgressService.RemoveCoins(CurrentShopItem.Cost);
            _playerProgressService.AddAvailableWeapon(CurrentShopItem.Type);
            
            UpdateUI();
        }

        private void HandleSelectClicked()
        {
            _playerProgressService.SetCurrentSelectedWeapon(CurrentShopItem.Type);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _shopUIPresenter.SetCurrentCoins(_playerProgressService.GetCoinsCount());
            
            if (_playerProgressService.GetAllAvailableWeapons().Contains(CurrentShopItem.Type))
            {
                _shopUIPresenter.SetSelectState(_playerProgressService.GetCurrentSelectedWeapon() == CurrentShopItem.Type);
            }
            else
            {
                _shopUIPresenter.SetBuyState(CurrentShopItem.Cost, _playerProgressService.GetCoinsCount() >= CurrentShopItem.Cost);
            }
        }
    }
}