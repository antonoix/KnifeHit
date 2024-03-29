using Internal.Scripts.GamePlay.Context;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    [CreateAssetMenu(fileName = "ShopServiceInjector", menuName = "Injectors/ShopServiceInjector")]
    public class ShopServiceInjector : ServiceInjector<IShopService>
    {
        private IUiService _uiService;
        private ShopContext _shopContext;
        private IPlayerProgressService _playerProgressService;

        public void Construct(IUiService uiService, ShopContext shopContext, IPlayerProgressService playerProgressService)
        {
            _uiService = uiService;
            _shopContext = shopContext;
            _playerProgressService = playerProgressService;
        }
        
        public override IShopService Create()
        {
            _service = new ShopService(_uiService, _shopContext, _playerProgressService);
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            _service.Init();
        }
    }
}