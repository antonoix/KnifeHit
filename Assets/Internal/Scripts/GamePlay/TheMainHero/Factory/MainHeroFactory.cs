using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.TheMainHero.Factory
{
    public class MainHeroFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetsProvider _assetsProvider;
        private readonly MainHeroConfig _config;

        public MainHeroFactory(IInstantiator instantiator,
            IAssetsProvider assetsProvider,
            MainHeroConfig config)
        {
            _instantiator = instantiator;
            _assetsProvider = assetsProvider;
            _config = config;
        }
        
        public async UniTask<MainHero> InstantiateHero()
        {
            var heroPrefab = await _assetsProvider.LoadAsync<GameObject>(_config.MainHeroReference);
            var hero = _instantiator.InstantiatePrefabForComponent<MainHero>(heroPrefab);

            return hero;
        }
    }
}