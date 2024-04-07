using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.PlayerProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory : IInitializable
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly LevelFactoryConfig _levelFactoryConfig;
        
        private Projectile _projectilePrefab;

        public LevelContext CreatedLevel { get; private set; }

        public LevelFactory(IInstantiator instantiator,
            LevelFactoryConfig levelFactoryConfig,
            IAssetsProvider assetsProvider,
            IPersistentProgressService persistentProgressService)
        {
            _instantiator = instantiator;
            _levelFactoryConfig = levelFactoryConfig;
            _assetsProvider = assetsProvider;
            _persistentProgressService = persistentProgressService;
        }

        public void Initialize()
        {
            _projectilePrefab = _levelFactoryConfig.AllProjectiles.FirstOrDefault(
                x => x.Type == _persistentProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType());
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

        public Projectile CreateProjectile()
        {
            return _instantiator.InstantiatePrefabForComponent<Projectile>(_projectilePrefab);
        }
    }
}
