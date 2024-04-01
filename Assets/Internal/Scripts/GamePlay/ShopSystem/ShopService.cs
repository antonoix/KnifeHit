using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.PlayerProgressService;
using Internal.Scripts.Infrastructure.SaveLoad;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.ShopUI;
using Zenject;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopService : IShopService, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly ShopFactory _shopFactory;
        private readonly IPersistentProgressService _playerProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private ShopContext _shopContext;
        private ShopUIPresenter _shopUIPresenter;

        private int _currentShopItemIndex;
        private ShopItem CurrentShopItem => _shopContext.ShopItems[_currentShopItemIndex];

        public ShopService(IUiService uiService, ShopFactory shopFactory, IPersistentProgressService playerProgressService, ISaveLoadService saveLoadService)
        {
            _uiService = uiService;
            _shopFactory = shopFactory;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            _shopUIPresenter = (ShopUIPresenter) _uiService.GetPresenter<ShopUIPresenter>();
        }

        public void StartWork()
        {
            _shopContext = _shopFactory.InstantiateShopContext();
            
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
            var selectedWeapon = _playerProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType();
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
            _playerProgressService.PlayerProgress.ResourcePack.Subtract(CurrentShopItem.ResourceCost);
            _playerProgressService.PlayerProgress.PlayerState.TryAddNewWeapon(CurrentShopItem.Type);
            
            _saveLoadService.SaveProgress();
            
            UpdateUI();
        }

        private void HandleSelectClicked()
        {
            _playerProgressService.PlayerProgress.PlayerState.SetCurrentWeapon(CurrentShopItem.Type);
            
            _saveLoadService.SaveProgress();
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _shopUIPresenter.SetCurrentCoins(_playerProgressService.PlayerProgress.ResourcePack[0].Value);
            
            if (_playerProgressService.PlayerProgress.PlayerState.IsWeaponAvailable(CurrentShopItem.Type))
            {
                _shopUIPresenter.SetSelectState(_playerProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType() == CurrentShopItem.Type);
            }
            else
            {
                _shopUIPresenter.SetBuyState(CurrentShopItem.ResourceCost.Value,
                    _playerProgressService.PlayerProgress.ResourcePack.CheckEnoughPrice(CurrentShopItem.ResourceCost));
            }
        }
    }
}