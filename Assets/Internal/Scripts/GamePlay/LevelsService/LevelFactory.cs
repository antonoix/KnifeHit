using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly LevelsConfig _levelsConfig;
        
        private WeaponProjectile _weaponProjectilePrefab;

        public LevelContext CreatedLevel { get; private set; }

        public LevelFactory(IInstantiator instantiator,
            LevelsConfig levelsConfig,
            IAssetsProvider assetsProvider,
            IPersistentProgressService persistentProgressService)
        {
            _instantiator = instantiator;
            _levelsConfig = levelsConfig;
            _assetsProvider = assetsProvider;
            _persistentProgressService = persistentProgressService;
        }

        public LevelContext CreateLevelContext(GameObject levelPrefab)
        {
            CreatedLevel = _instantiator.InstantiatePrefabForComponent<LevelContext>(levelPrefab);
            CreatedLevel.transform.position = Vector3.zero;

            return CreatedLevel;
        }

        public MainHero InstantiateHero()
        {
            var hero = _instantiator.InstantiatePrefabForComponent<MainHero>(_levelsConfig.MainHero);

            return hero;
        }

        public WeaponProjectile CreateProjectile()
        {
            _weaponProjectilePrefab ??= _levelsConfig.AllProjectiles.FirstOrDefault(
                x => x.Type == _persistentProgressService.PlayerProgress.PlayerState.GetCurrentWeaponType());
            return _instantiator.InstantiatePrefabForComponent<WeaponProjectile>(_weaponProjectilePrefab);
        }
    }
}
