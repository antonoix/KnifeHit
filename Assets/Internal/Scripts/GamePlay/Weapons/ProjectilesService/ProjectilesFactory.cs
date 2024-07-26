using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.ProjectilesService
{
    public class ProjectilesFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly AllWeaponsConfig _allWeaponsConfig;
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetsProvider _assetsProvider;

        private GameObject _weaponProjectilePrefab;
        
        public bool IsInitialized { get; private set; }

        public ProjectilesFactory(IInstantiator instantiator,
            AllWeaponsConfig allWeaponsConfig,
            IPersistentProgressService progressService,
            IAssetsProvider assetsProvider)
        {
            _instantiator = instantiator;
            _allWeaponsConfig = allWeaponsConfig;
            _progressService = progressService;
            _assetsProvider = assetsProvider;
        }

        public async UniTask Initialize()
        {
            var weaponType = _progressService.PlayerProgress.PlayerState.GetCurrentWeaponType();
            var config = _allWeaponsConfig.AllConfigs.FirstOrDefault(
                x => x.Type == weaponType);

            if (config == null)
            {
                Debug.LogError($"Can not find weapon: {weaponType}");
                config = _allWeaponsConfig.AllConfigs[0];
            }
                
            _weaponProjectilePrefab = await _assetsProvider.LoadAsync<GameObject>(config.PrefabReference);

            IsInitialized = true;
        }
        
        public WeaponProjectile CreateProjectile()
        {
            if (!IsInitialized)
            {
                Debug.LogError("Initialize at first");
            }
            
            return _instantiator.InstantiatePrefabForComponent<WeaponProjectile>(_weaponProjectilePrefab);
        }
    }
}