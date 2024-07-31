using Internal.Scripts.GamePlay.ShopSystem.Configs;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopFactory
    {
        private readonly ShopConfig _config;

        public ShopFactory(ShopConfig config)
        {
            _config = config;
        }

        public ShopContext InstantiateShopContext()
        {
            var shopContext = Object.Instantiate(_config.ShopContextPrefab);
            shopContext.transform.position = Vector3.zero;

            return shopContext;
        }
    }
}