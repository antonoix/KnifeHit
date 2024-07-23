using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
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
        private readonly ShopConfig _shopConfig;
        private readonly AllWeaponsConfig _allWeaponsConfig;
        private readonly IPersistentProgressService _playerProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISpecialEffectsService _specialEffectsService;
        private ShopContext _shopContext;
        private ShopUIPresenter _shopUIPresenter;
        private CancellationTokenSource _cancellationToken;

        private int _currentShopItemIndex;
        private ShopItem CurrentShopItem => _shopContext.ShopItems[_currentShopItemIndex];
        private PlayerProgress PlayerProgress => _playerProgressService.PlayerProgress;

        public ShopService(IUiService uiService,
            ShopFactory shopFactory,
            ShopConfig shopConfig,
            AllWeaponsConfig allWeaponsConfig,
            IPersistentProgressService playerProgressService,
            ISaveLoadService saveLoadService,
            ISpecialEffectsService specialEffectsService)
        {
            _uiService = uiService;
            _shopFactory = shopFactory;
            _shopConfig = shopConfig;
            _allWeaponsConfig = allWeaponsConfig;
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
            FillShopItems();
            FocusCurrentWeapon();

            _shopUIPresenter.Show();
            UpdateUI();
            
            _shopUIPresenter.OnNextClicked += HandleNextClicked;
            _shopUIPresenter.OnPrevClicked += HandlePrevClicked;
            _shopUIPresenter.OnBuyClicked += HandleBuyClicked;
            _shopUIPresenter.OnSelectClicked += HandleSelectClicked;
        }

        private void FillShopItems()
        {
            int i = 0;
            foreach (var shopItem in _shopContext.ShopItems)
            {
                shopItem.Setup(_allWeaponsConfig.AllConfigs[i++]);
            }

            
        }

        private void FocusCurrentWeapon()
        {
            var selectedWeapon = _playerProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType();
            for (int i = 0; i < _shopContext.ShopItems.Length; i++)
            {
                if (_shopContext.ShopItems[i].CurrentWeapon.Type == selectedWeapon)
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

            FocusItem().Forget();
        }

        private void HandleNextClicked()
        {
            if (++_currentShopItemIndex >= _shopContext.ShopItems.Length)
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
            PlayerProgress.ResourcePack.Subtract(CurrentShopItem.CurrentWeapon.ResourceCost);
            PlayerProgress.PlayerState.TryAddNewWeapon(CurrentShopItem.CurrentWeapon.Type);
            _specialEffectsService.ShowEffect(SpecialEffectType.BuySparkles, CurrentShopItem.transform.position);
            
            _saveLoadService.SaveProgress();
            
            UpdateUI();
        }

        private void HandleSelectClicked()
        {
            PlayerProgress.PlayerState.SetCurrentWeapon(CurrentShopItem.CurrentWeapon.Type);
            
            _saveLoadService.SaveProgress();
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _shopUIPresenter.SetCurrentResources(PlayerProgress.ResourcePack.ToList());
            
            if (PlayerProgress.PlayerState.IsWeaponAvailable(CurrentShopItem.CurrentWeapon.Type))
            {
                _shopUIPresenter.SetSelectState(
                    PlayerProgress.PlayerState.GetCurrentWeaponType() == CurrentShopItem.CurrentWeapon.Type);
            }
            else
            {
                _shopUIPresenter.SetBuyState(CurrentShopItem.CurrentWeapon.ResourceCost,
                    PlayerProgress.ResourcePack.CheckEnoughPrice(CurrentShopItem.CurrentWeapon.ResourceCost));
            }
        }
    }
}