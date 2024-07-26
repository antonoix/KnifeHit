using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Internal.Scripts.GamePlay.LevelsService
{
    public class LevelsService : ILevelsService
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly LevelsConfig _config;
        private readonly LevelFactory _levelFactory;

        private List<GameObject> _allLevels;

        public LevelContext CurrentLevel => _levelFactory.CreatedLevel;

        public LevelsService(IAssetsProvider assetsProvider,
            IPersistentProgressService progressService,
            LevelsConfig config,
            LevelFactory levelFactory)
        {
            _assetsProvider = assetsProvider;
            _progressService = progressService;
            _config = config;
            _levelFactory = levelFactory;
        }

        public async UniTask Initialize()
        {
            _allLevels = await _assetsProvider.LoadAssetsByLabelAsync<GameObject>(_config.LevelLabel);
        }

        public int GetAllLevelsCount()
        {
            if (!CheckIfInitialized()) return 0;

            return _allLevels.Count;
        }

        public LevelContext CreateLevelContext()
        {
            int currentIndex = GetCurrentLevelIndex();
            return _levelFactory.CreateLevelContext(_allLevels[currentIndex]);
        }

        public int GetCurrentLevelIndex()
        {
            if (!CheckIfInitialized()) return 0;
            
            var index = _progressService.PlayerProgress.PlayerState.LastCompletedLevelIndex;
            if (index < 0)
                index = 0;
            
            return index < _allLevels.Count ? 
                index 
                : Random.Range(0, _allLevels.Count);
        }

        private bool CheckIfInitialized()
        {
            if (_allLevels == null)
            {
                Debug.LogError("Initialize before");
                return false;
            }

            return true;
        }
    }
}