using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly LevelFactoryConfig _levelFactoryConfig;
        private readonly IAssetsProvider _assetsProvider;

        public LevelContext CreatedLevel { get; private set; }

        public LevelFactory(IInstantiator instantiator, LevelFactoryConfig levelFactoryConfig, IAssetsProvider assetsProvider)
        {
            _instantiator = instantiator;
            _levelFactoryConfig = levelFactoryConfig;
            _assetsProvider = assetsProvider;
        }

        public async UniTask<LevelContext> CreateLevelContext(int levelIndex)
        {
            var prefab = await _assetsProvider.LoadAsync<GameObject>(_levelFactoryConfig.LevelContexts[levelIndex]);
            CreatedLevel = _instantiator.InstantiatePrefabForComponent<LevelContext>(prefab);
            CreatedLevel.transform.position = Vector3.zero;

            return CreatedLevel;
        }
    
        public MainHero InstantiateHero()
        {
            var hero = _instantiator.InstantiatePrefabForComponent<MainHero>(_levelFactoryConfig.MainHero);

            return hero;
        }
    }
}
