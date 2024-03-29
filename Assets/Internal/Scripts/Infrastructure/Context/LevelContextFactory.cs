using System.Linq;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Context
{
    public class LevelContextFactory
    {
        private readonly GameplayEntities _gameplayEntities;
        private readonly InputService _playerInputService;
        private readonly ISpecialEffectsService _specialEffects;
        private readonly ISoundsService _soundsService;
        private readonly IPlayerProgressService _playerProgressService;

        public LevelContextFactory(GameplayEntities gameplayEntities, InputService playerInputService, 
            ISpecialEffectsService specialEffects, ISoundsService soundsService, IPlayerProgressService playerProgressService)
        {
            _gameplayEntities = gameplayEntities;
            _playerInputService = playerInputService;
            _specialEffects = specialEffects;
            _soundsService = soundsService;
            _playerProgressService = playerProgressService;
        }

        public LevelContext InstantiateLevelContext(int levelIndex)
        {
            var levelContext = Object.Instantiate(_gameplayEntities.LevelContexts[levelIndex]);
            levelContext.transform.position = Vector3.zero;

            return levelContext;
        }
    
        public MainHero InstantiateHero()
        {
            var hero = Object.Instantiate(_gameplayEntities.MainHero);
            var projectile =
                _gameplayEntities.AllProjectiles.FirstOrDefault(x => x.Type == _playerProgressService.GetCurrentSelectedWeapon());
            hero.Setup(_playerInputService, _specialEffects, _soundsService, projectile);

            return hero;
        }
    }
}
