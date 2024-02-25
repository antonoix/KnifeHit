using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Context
{
    public class LevelContextFactory
    {
        private readonly MainHero _heroPrefab;
        private readonly LevelContext[] _levelContextPrefabs;
        private readonly InputService _playerInputService;
        private readonly ISpecialEffectsService _specialEffects;
        private readonly ISoundsService _soundsService;

        public LevelContextFactory(MainHero heroPrefab, LevelContext[] levelContexts, InputService playerInputService, 
            ISpecialEffectsService specialEffects, ISoundsService soundsService)
        {
            _heroPrefab = heroPrefab;
            _levelContextPrefabs = levelContexts;
            _playerInputService = playerInputService;
            _specialEffects = specialEffects;
            _soundsService = soundsService;
        }

        public LevelContext InstantiateLevelContext(int levelIndex)
        {
            var levelContext = Object.Instantiate(_levelContextPrefabs[levelIndex]);
            levelContext.transform.position = Vector3.zero;

            return levelContext;
        }
    
        public MainHero InstantiateHero()
        {
            var hero = Object.Instantiate(_heroPrefab);
            hero.Setup(_playerInputService, _specialEffects, _soundsService);

            return hero;
        }
    }
}
