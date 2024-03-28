using System.Collections.Generic;
using Internal.Scripts.Infrastructure.ShopSystem;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Context
{
    public class ShopContextFactory
    {
        private readonly ShopContext _shopContext;

        public ShopContextFactory(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public ShopContext InstantiateShopContext()
        {
            var shopContext = Object.Instantiate(_shopContext);
            shopContext.transform.position = Vector3.zero;

            return shopContext;
        }
    }
}