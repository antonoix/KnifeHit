using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.SaveLoad;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.ShopUI;
using Zenject;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopService : IShopService, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly ShopFactory _shopFactory;
        private readonly ShopFactoryConfig _shopConfig;
        private readonly IPersistentProgressService _playerProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISpecialEffectsService _specialEffectsService;
        private ShopContext _shopContext;
        private ShopUIPresenter _shopUIPresenter;
        private CancellationTokenSource _cancellationToken;

        private int _currentShopItemIndex;
        private ShopItem CurrentShopItem => _shopConfig.ShopItems[_currentShopItemIndex];

        public ShopService(IUiService uiService,
            ShopFactory shopFactory,
            ShopFactoryConfig shopConfig,
            IPersistentProgressService playerProgressService,
            ISaveLoadService saveLoadService,
            ISpecialEffectsService specialEffectsService)
        {
            _uiService = uiService;
            _shopFactory = shopFactory;
            _shopConfig = shopConfig;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
            _specialEffectsService = specialEffectsService;
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
            for (int i = 0; i < _shopConfig.ShopItems.Length; i++)
            {
                if (_shopConfig.ShopItems[i].Type == selectedWeapon)
                {
                    _shopContext.ShopCamera.Focus(_shopConfig.ShopItems[i].transform);
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
                _currentShopItemIndex = _shopConfig.ShopItems.Length - 1;

            FocusItem().Forget();
        }

        private void HandleNextClicked()
        {
            if (++_currentShopItemIndex >= _shopConfig.ShopItems.Length)
                _currentShopItemIndex = 0;
            
            FocusItem().Forget();
        }

        private async UniTaskVoid FocusItem()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();

            await _shopContext.ShopCamera.Focus(CurrentShopItem.transform)
                .AttachExternalCancellation(_cancellationToken.Token)
                .SuppressCancellationThrow();

            UpdateUI();
            
            float delayBetweenCameraAndItemEffects = 0.2f;
            await UniTask.WaitForSeconds(delayBetweenCameraAndItemEffects)
                .AttachExternalCancellation(_cancellationToken.Token)
                .SuppressCancellationThrow();
            
            CurrentShopItem.PlayScaleEffect();
        }

        private void HandleBuyClicked()
        {
            _playerProgressService.PlayerProgress.ResourcePack.Subtract(CurrentShopItem.ResourceCost);
            _playerProgressService.PlayerProgress.PlayerState.TryAddNewWeapon(CurrentShopItem.Type);
            _specialEffectsService.ShowEffect(SpecialEffectType.BuySparkles, CurrentShopItem.transform.position);
            
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
            _shopUIPresenter.SetCurrentResources(_playerProgressService.PlayerProgress.ResourcePack.ToList());
            
            if (_playerProgressService.PlayerProgress.PlayerState.IsWeaponAvailable(CurrentShopItem.Type))
            {
                _shopUIPresenter.SetSelectState(_playerProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType() == CurrentShopItem.Type);
            }
            else
            {
                _shopUIPresenter.SetBuyState(CurrentShopItem.ResourceCost,
                    _playerProgressService.PlayerProgress.ResourcePack.CheckEnoughPrice(CurrentShopItem.ResourceCost));
            }
        }
    }
}